using System.Text;
using Dapper;
using Laquila.Integrations.Core.Domain.Filters;

namespace Laquila.Integrations.Core.Domain.DTO.Shared
{
    public class QueryHelpers
    {
        public static (StringBuilder sql, DynamicParameters parameters) FilterStringQuery(StringBuilder sql, LAQFilters filters)
        {
            var parameters = new DynamicParameters();

            sql.AppendLine(" AND CdEmpresa = @CdEmpresa");
            parameters.Add("CdEmpresa", filters.CdEmpresa);

            sql.AppendLine(" AND CONVERT(DATE,DhEmissao) between @DataEmissaoInicial and @DataEmissaoFinal");

            var dataInicio = new DateTime(filters.DataEmissaoInicial.Year, filters.DataEmissaoInicial.Month, filters.DataEmissaoInicial.Day);
            var dataFim = new DateTime(filters.DataEmissaoFinal.Year, filters.DataEmissaoFinal.Month, filters.DataEmissaoFinal.Day, 23, 59, 59);

            parameters.Add("DataEmissaoInicial", dataInicio);
            parameters.Add("DataEmissaoFinal", dataFim);

            if (filters.CdCliente != null)
            {
                sql.AppendLine(" AND CdCliente = @CdCliente");
                parameters.Add("CdCliente", filters.CdCliente);
            }

            if (filters.CpfCnpjCliente != null)
            {
                sql.AppendLine(" AND CpfCnpjCliente = @CpfCnpjCliente");
                parameters.Add("CpfCnpjCliente", filters.CpfCnpjCliente);
            }

            if (filters.IdRomaneio != null)
            {
                sql.AppendLine(" AND IdRomaneio = @IdRomaneio");
                parameters.Add("IdRomaneio", filters.IdRomaneio);
            }

            if (filters.IdNotaEmitida != null)
            {
                sql.AppendLine(" AND IdNotaEmitida = @IdNotaEmitida");
                parameters.Add("IdNotaEmitida", filters.IdNotaEmitida);
            }

            if (filters.NrNota != null)
            {
                sql.Append(" AND NrNota = @NrNota");
                parameters.Add("NrNota", filters.NrNota);
            }

            if (filters.CdTransportador != null)
            {
                sql.Append(" AND CdTransportador = @CdTransportador");
                parameters.Add("CdTransportador", filters.CdTransportador);
            }

            if (filters.CdRedespacho != null)
            {
                sql.Append(" AND CdRedespacho = @CdRedespacho");
                parameters.Add("CdRedespacho", filters.CdRedespacho);
            }

            return (sql, parameters);
        }
    }
}