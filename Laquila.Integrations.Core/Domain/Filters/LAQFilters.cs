using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Laquila.Integrations.Core.Domain.Filters
{
    public class LAQFilters
    {
        [Required]
        [FromQuery(Name = "page")]
        public int Page { get; set; } = 1;

        [Required]
        [FromQuery(Name = "page_size")]
        public int PageSize { get; set; } = 20;

        [FromQuery(Name = "lo_ma_cnpj_owner")]
        public string? LoMaCnpjOwner { get; set; }

        [FromQuery(Name = "lo_ini_gentime")]
        public DateOnly LoIniGenTime { get; set; } = DateOnly.FromDateTime(DateTime.Now);

        [FromQuery(Name = "lo_end_gentime")]
        public DateOnly LoEndGenTime { get; set; } = DateOnly.FromDateTime(DateTime.Now);

        [FromQuery(Name = "lo_ma_cnpj")]
        public string? LoMaCnpj { get; set; }

        [FromQuery(Name = "lo_ma_cnpj_transportador")]
        public string? LoMaCnpjCarrier { get; set; }

        [FromQuery(Name = "lo_ma_cnpj_redespacho")]
        public string? LoMaCnpjRedespacho { get; set; }

        [FromQuery(Name = "lo_oe")]
        public long? LoOe { get; set; }

        [FromQuery(Name = "oe_erp_order")]
        public long? OeErpOrder { get; set; }

        [FromQuery(Name = "oe_invnumber")]
        public long? OeInvNumber { get; set; }

        [FromQuery(Name = "oe_serialnr")]
        public string? OeSerialNr { get; set; }
    }
}