

using System.ComponentModel.DataAnnotations.Schema;

namespace Laquila.Integrations.Core.Domain.Models
{
    public class VLAQConsultarRomaneioItens
    {
        [Column("id_romaneio")]
        public long IdRomaneio { get; set; }
        [Column("dh_emissao")]
        public DateTime DhEmissao { get; set; }
        [Column("cd_transportador")]
        public long CdTransportador { get; set; }
        [Column("at_situacao")]
        public int AtSituacao { get; set; }
        [Column("situacao_romaneio")]
        public required string SituacaoRomaneio { get; set; }
        [Column("cd_item")]
        public required string CdItem { get; set; }
        [Column("ds_item")]
        public required string DsItem { get; set; }
        [Column("id_embalagem")]
        public int IdEmbalagem { get; set; }
        [Column("ds_embalagem")]
        public required string DsEmbalagem { get; set; }
        [Column("qt_item")]
        public decimal QtItem { get; set; }
        [Column("qt_baixa")]
        public decimal QtBaixa { get; set; }
        [Column("qt_cortada")]
        public decimal QtCortada { get; set; }
        [Column("qt_faturada")]
        public decimal QtFaturada { get; set; }
        [Column("mensagem")]
        public required string Mensagem { get; set; }
    }
}