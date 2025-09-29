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

            sql.AppendLine(" AND lo_ma_cnpj_owner = @lo_ma_cnpj_owner");
            parameters.Add("lo_ma_cnpj_owner", filters.LoMaCnpjOwner);

            sql.AppendLine(" AND CONVERT(DATE,DhEmissao) between @DataEmissaoInicial and @DataEmissaoFinal");

            var dataInicio = new DateTime(filters.LoIniGenTime.Year, filters.LoIniGenTime.Month, filters.LoIniGenTime.Day);
            var dataFim = new DateTime(filters.LoEndGenTime.Year, filters.LoEndGenTime.Month, filters.LoEndGenTime.Day, 23, 59, 59);

            parameters.Add("lo_ini_gen_time", dataInicio);
            parameters.Add("lo_end_gen_time", dataFim);

            if (filters.LoOe != null)
            {
                sql.AppendLine(" AND lo_oe = @lo_oe");
                parameters.Add("lo_oe", filters.LoOe);
            }

            if (filters.OeErpOrder != null)
            {
                sql.AppendLine(" AND oe_erp_order = @oe_erp_order");
                parameters.Add("oe_erp_order", filters.OeErpOrder);
            }

            if (filters.LoMaCnpj != null)
            {
                sql.AppendLine("AND lo_ma_cnpj = @lo_ma_cnpj");
                parameters.Add("lo_ma_cnpj", filters.LoMaCnpj);
            }

            if (filters.OeInvNumber != null)
            {
                sql.AppendLine(" AND oe_invnumber = @oe_invnumber");
                parameters.Add("oe_invnumber", filters.OeInvNumber);
            }

            if (filters.OeSerialNr != null)
            {
                sql.Append(" AND oe_serialnr = @oe_serialnr");
                parameters.Add("oe_serialnr", filters.OeSerialNr);
            }

            if (filters.LoMaCnpjCarrier != null)
            {
                sql.Append(" AND lo_ma_cnpj_carrier = @lo_ma_cnpj_carrier");
                parameters.Add("lo_ma_cnpj_carrier", filters.LoMaCnpjCarrier);
            }

            if (filters.LoMaCnpjRedespacho != null)
            {
                sql.Append(" AND lo_ma_cnpj_carrier = @lo_ma_cnpj_carrier");
                parameters.Add("lo_ma_cnpj_carrier", filters.LoMaCnpjRedespacho);
            }

            return (sql, parameters);
        }

        public static (StringBuilder sql, DynamicParameters parameters) FilterPrenotasQuery(StringBuilder sql, LAQFilters filters)
        {
            var parameters = new DynamicParameters();

            sql.AppendLine(" AND lo_ma_cnpj_owner = @lo_ma_cnpj_owner");
            parameters.Add("lo_ma_cnpj_owner", filters.LoMaCnpjOwner);

            sql.AppendLine(" AND CONVERT(DATE,DhEmissao) between @DataEmissaoInicial and @DataEmissaoFinal");

            var dataInicio = new DateTime(filters.LoIniGenTime.Year, filters.LoIniGenTime.Month, filters.LoIniGenTime.Day);
            var dataFim = new DateTime(filters.LoEndGenTime.Year, filters.LoEndGenTime.Month, filters.LoEndGenTime.Day, 23, 59, 59);

            parameters.Add("lo_ini_gen_time", dataInicio);
            parameters.Add("lo_end_gen_time", dataFim);

            if (filters.LoOe != null)
            {
                sql.AppendLine(" AND lo_oe = @lo_oe");
                parameters.Add("lo_oe", filters.LoOe);
            }

            if (filters.OeErpOrder != null)
            {
                sql.AppendLine(" AND oe_erp_order = @oe_erp_order");
                parameters.Add("oe_erp_order", filters.OeErpOrder);
            }

            if (filters.LoMaCnpj != null)
            {
                sql.AppendLine("AND lo_ma_cnpj = @lo_ma_cnpj");
                parameters.Add("lo_ma_cnpj", filters.LoMaCnpj);
            }

            if (filters.LoMaCnpjCarrier != null)
            {
                sql.Append(" AND lo_ma_cnpj_carrier = @lo_ma_cnpj_carrier");
                parameters.Add("lo_ma_cnpj_carrier", filters.LoMaCnpjCarrier);
            }

            return (sql, parameters);
        }
    }
}