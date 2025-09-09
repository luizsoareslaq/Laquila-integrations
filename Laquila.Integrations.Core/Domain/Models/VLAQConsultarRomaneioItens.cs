

using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;
using System.Text.Json.Serialization;

namespace Laquila.Integrations.Core.Domain.Models
{
    public class VLAQConsultarRomaneioItens
    {
        public required string CnpjEmpresa { get; set; }
        public int CdEmpresa { get; set; }
        public long IdRomaneio { get; set; }
        public DateTime DhEmissao { get; set; }
        public long CdCliente { get; set; }
        public required string CpfCnpjCliente { get; set; }
        public required string RazaoCliente { get; set; }
        public required string FantasiaCliente { get; set; }
        public long? CdTransportador { get; set; }
        public string? CnpjTransportadora { get; set; }
        public string? RazaoTransportadora { get; set; }
        public int AtSituacaoRomaneio { get; set; }
        public required string SituacaoRomaneio { get; set; }
        public required string CdItem { get; set; }
        public required string DsItem { get; set; }
        public int IdEmbalagem { get; set; }
        public required string DsEmbalagem { get; set; }
        public long IdRomaneioDocumentoItem { get; set; }
        public decimal QtItem { get; set; }
        public decimal QtBaixa { get; set; }
        public decimal QtCortada { get; set; }
        public decimal QtFaturada { get; set; }
        public required string Mensagem { get; set; }
    }
}