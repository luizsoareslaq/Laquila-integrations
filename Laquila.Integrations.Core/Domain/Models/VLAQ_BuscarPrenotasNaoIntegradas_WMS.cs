using System.ComponentModel.DataAnnotations.Schema;

namespace Laquila.Integrations.Core.Domain.Models
{
    public class VLAQ_BuscarPrenotasNaoIntegradas_WMS
    {
        [Column("lo_oe")]
        public required long LoOe { get; set; }

        [Column("lo_ma_cnpj_owner")]
        public required string LoMaCnpjOwner { get; set; }
    }
}