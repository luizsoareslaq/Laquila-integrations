
using System.ComponentModel.DataAnnotations.Schema;

namespace Laquila.Integrations.Core.Domain.Models
{
    public class VLAQConsultarNotas
    {
        public required string CpfCnpjEmpresa { get; set; }
        public int CdEmpresa { get; set; }
        public long IdNotaEmitida { get; set; }
        public long NrNota { get; set; }
        public required string CdChaveAcessoNota { get; set; }
        public int DiaEmissao { get; set; }
        public int MesEmissao { get; set; }
        public int AnoEmissao { get; set; }
        public DateTime DhEmissao { get; set; }
        public long CdCliente { get; set; }
        public required string CpfCnpjCliente { get; set; }
        public required string RazaoCliente { get; set; }
        public required string FantasiaCliente { get; set; }
        public decimal VlMercadoriaNota { get; set; }
        public decimal VlTotalNota { get; set; }
        public long? CdTransportador { get; set; }
        public string? CnpjTransportadora { get; set; }
        public string? RazaoTransportadora { get; set; }
        public long? CdRedespacho { get; set; }
        public string? CnpjRedespacho { get; set; }
        public string? RazaoRedespacho { get; set; }
        public int AtSituacaoNota { get; set; }
        public required string SituacaoNota { get; set; }
        public int IdRomaneio { get; set; }
        public DateTime? DhEmissaoRomaneio { get; set; }
        public int? AtSituacaoRomaneio { get; set; }
        public string? SituacaoRomaneio { get; set; }
        public required string CdItem { get; set; }
        public required string DsItem { get; set; }
        public int IdEmbalagem { get; set; }
        public required string DsEmbalagem { get; set; }
        public int NrOrdem { get; set; }
        public decimal QtItem { get; set; }
        public decimal VlTotalItem { get; set; }
    }
}