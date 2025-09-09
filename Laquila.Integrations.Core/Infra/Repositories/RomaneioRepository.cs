using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Laquila.Integrations.Core.Domain.DTO.Shared;
using Laquila.Integrations.Core.Domain.Filters;
using Laquila.Integrations.Core.Domain.Models;
using Laquila.Integrations.Core.Infra.Interfaces;

namespace Laquila.Integrations.Core.Infra.Repositories
{
    public class RomaneioRepository : IRomaneioRepository
    {
        private readonly IDbConnectionFactory _dbFactory;

        public RomaneioRepository(IDbConnectionFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public async Task<(IEnumerable<VLAQConsultarRomaneioItens>, int TotalCount)> GetRomaneiosAsync(LAQFilters filters, CancellationToken ct)
        {
            var sqlBase = new StringBuilder("FROM VLAQ_ConsultarRomaneioItens WHERE 1=1");
            (sqlBase, var parameters) = QueryHelpers.FilterStringQuery(sqlBase, filters);

            var sqlIds = new StringBuilder();
            sqlIds.AppendLine("SELECT DISTINCT IdRomaneio");
            sqlIds.Append(sqlBase);
            sqlIds.AppendLine(" ORDER BY IdRomaneio OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY");

            var sqlCount = new StringBuilder("SELECT COUNT(DISTINCT IdRomaneio) ");
            sqlCount.Append(sqlBase);

            parameters.Add("Offset", (filters.Page - 1) * filters.PageSize);
            parameters.Add("PageSize", filters.PageSize);

            using var connection = _dbFactory.CreateConnection();

            var ids = (await connection.QueryAsync<long>(sqlIds.ToString(), parameters)).ToList();

            if (!ids.Any())
                return (Enumerable.Empty<VLAQConsultarRomaneioItens>(), 0);

            var sqlDados = new StringBuilder("SELECT * FROM VLAQ_ConsultarRomaneioItens WHERE IdRomaneio IN @Ids");
            var dadosParams = new DynamicParameters();
            dadosParams.Add("Ids", ids);


           var dados = await connection.QueryAsync<VLAQConsultarRomaneioItens>(
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