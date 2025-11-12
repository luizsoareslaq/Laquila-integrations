using System.ComponentModel.DataAnnotations.Schema;

namespace Laquila.Integrations.Core.Domain.Models
{
    public class VMWMS_BuscarNotasNaoIntegradas
    {
        public required string LoMaCnpjOwner { get; set; }
        public required string LoMaCnpj { get; set; }
        public string? LoMaCnpjCarrier { get; set; }
        public string? LoMaCnpjRedespacho { get; set; }
        public int LoOe { get; set; }
        public long OeInvNumber { get; set; }
        public required string OeSerialNr { get; set; }
        public int OelId { get; set; }
        public required string OelAtId { get; set; }
        public int OelQtyAtend { get; set; }
        public int OeId { get; set; }
        public int OeErpOrder { get; set; }
        // public string PrecisaAtualizarItem { get; set; } = "N";
        // public string PrecisaAtualizarCliente { get; set; } = "N";
        // public string PrecisaAtualizarTransportador { get; set; } = "N";
    }
}