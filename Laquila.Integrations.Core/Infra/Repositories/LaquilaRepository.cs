using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Laquila.Integrations.Core.Domain.Filters;
using Laquila.Integrations.Core.Domain.Interfaces;
using Laquila.Integrations.Core.Domain.Models;

namespace Laquila.Integrations.Core.Infra.Repositories
{
    public class LaquilaRepository : ILaquilaRepository
    {
        private readonly IDbConnectionFactory _dbFactory;

        public LaquilaRepository(IDbConnectionFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }
        public async Task<IEnumerable<VLAQConsultarNotas>> ConsultarNotasAsync(LAQFilters filters, CancellationToken ct)
        {
            var sql = new StringBuilder("SELECT * FROM VLAQ_ConsultarNotas WHERE 1=1");
            var parameters = new DynamicParameters();

            sql.AppendLine(" AND cd_empresa = @CdEmpresa");
            parameters.Add("CdEmpresa", filters.CdEmpresa);
            
            sql.AppendLine(" AND dh_emissao between @DataEmissaoInicial and @DataEmissaoFinal");
            parameters.Add("DataEmissaoInicial", filters.DataEmissaoInicial);    
            parameters.Add("DataEmissaoFinal", filters.DataEmissaoFinal);    
            

            if (filters.CdCliente != null)
            {
                sql.AppendLine(" AND cd_cliente = @CdCliente");
                parameters.Add("CdCliente", filters.CdCliente);
            }

            if (filters.CpfCnpjCliente != null)
            {
                sql.AppendLine(" AND cpf_cnpj_cliente = @CpfCnpjCliente");
                parameters.Add("CpfCnpjCliente", filters.CpfCnpjCliente);
            }

            if (filters.IdRomaneio != null)
            {
                sql.AppendLine(" AND id_romaneio = @IdRomaneio");
                parameters.Add("IdRomaneio", filters.IdRomaneio);    
            }

            if (filters.IdNotaEmitida != null)
            {
                sql.AppendLine(" AND id_notaemitida = @IdNotaEmitida");
                parameters.Add("IdNotaEmitida", filters.IdNotaEmitida);
            }

            if (filters.NrNota != null)
            {
                sql.Append(" AND nr_nota = @NrNota");
                parameters.Add("NrNota", filters.NrNota);
            }

            if (filters.CdTransportador != null)
            {
                sql.Append(" AND cd_transportador_nota = @CdTransportador");
                parameters.Add("CdTransportador", filters.CdTransportador);
            }

            if (filters.CdRedespacho != null)
            {
                sql.Append(" AND cd_redespacho_nota = @CdRedespacho");
                parameters.Add("CdRedespacho", filters.CdRedespacho);
            }
            
            using var connection = _dbFactory.CreateConnection();
            return await connection.QueryAsync<VLAQConsultarNotas>(
                        sql.ToString(),
                        param: parameters,
                        transaction: null,
                        commandTimeout: 30,
                        commandType: CommandType.Text
                    );
        }
    }
}