using System.ComponentModel.DataAnnotations.Schema;

namespace Laquila.Integrations.Core.Domain.Models
{
    public class VMWMS_BuscarPrenotasNaoIntegradas
    {
        public required string LoMaCnpjOwner { get; set; }
        public required string LoMaCnpj { get; set; }
        public string? LoMaCnpjCarrier { get; set; }
        public string? LoMaCnpjRedespacho { get; set; }
        public int LoPriority { get; set; }
        public int LoType { get; set; }
        public long LoOe { get; set; }
        public long OeErpOrder { get; set; }
        public long OelId { get; set; }
        public required string OelAtId { get; set; }
        public int OelQtyReq { get; set; }
        public string PrecisaAtualizarItem { get; set; } = "N";
        public string PrecisaAtualizarCliente { get; set; } = "N";
        public string PrecisaAtualizarTransportador { get; set; } = "N";
    }
}