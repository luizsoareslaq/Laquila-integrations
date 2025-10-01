using System.Data;
using System.Text;
using Dapper;
using Laquila.Integrations.Core.Domain.DTO.Shared;
using Laquila.Integrations.Core.Domain.Filters;
using Laquila.Integrations.Core.Domain.Models;
using Laquila.Integrations.Core.Infra.Interfaces;

namespace Laquila.Integrations.Core.Infra.Repositories
{
    public class PrenotaRepository : IPrenotaRepository
    {
        private readonly IDbConnectionFactory _dbFactory;

        public PrenotaRepository(IDbConnectionFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public async Task<(IEnumerable<VMWMS_BuscarPrenotasNaoIntegradas>, int TotalCount)> GetPrenotasAsync(LAQFilters filters, CancellationToken ct)
        {
            var sqlBase = new StringBuilder("FROM VMWMS_BuscarPrenotasNaoIntegradas WHERE 1=1");
            (sqlBase, var parameters) = QueryHelpers.FilterPrenotasQuery(sqlBase, filters);

            var sqlIds = new StringBuilder();
            sqlIds.AppendLine("SELECT DISTINCT LoOe");
            sqlIds.Append(sqlBase);
            sqlIds.AppendLine(" ORDER BY LoOe OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY");

            var sqlCount = new StringBuilder("SELECT COUNT(DISTINCT LoOe) ");
            sqlCount.Append(sqlBase);

            parameters.Add("Offset", (filters.Page - 1) * filters.PageSize);
            parameters.Add("PageSize", filters.PageSize);

            using var connection = _dbFactory.CreateConnection();

            var ids = (await connection.QueryAsync<long>(sqlIds.ToString(), parameters)).ToList();

            if (!ids.Any())
                return (Enumerable.Empty<VMWMS_BuscarPrenotasNaoIntegradas>(), 0);

            var sqlDados = new StringBuilder("SELECT * FROM VMWMS_BuscarPrenotasNaoIntegradas WHERE LoOe IN @Ids");
            var dadosParams = new DynamicParameters();
            dadosParams.Add("Ids", ids);

            var dados = await connection.QueryAsync<VMWMS_BuscarPrenotasNaoIntegradas>(
                 sqlDados.ToString(),
                 param: dadosParams,
                 commandTimeout: 30,
                 commandType: CommandType.Text
             );

            var total = await connection.ExecuteScalarAsync<int>(
                sqlCount.ToString(), param: parameters, transaction: null, commandTimeout: 30, commandType: CommandType.Text);

            return (dados, total);
        }
    }
}