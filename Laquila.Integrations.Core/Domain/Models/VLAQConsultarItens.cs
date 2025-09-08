
using System.ComponentModel.DataAnnotations.Schema;

namespace Laquila.Integrations.Core.Domain.Models
{
    public class VLAQConsultarItens
    {
        [Column("cd_item")]
        public required string CdItem { get; set; }
        [Column("ds_item")]
        public required string DsITem { get; set; }
        [Column("at_situacao_item")]
        public int AtSituacaoItem { get; set; }
        [Column("situacao_item")]
        public required string SituacaoItem { get; set; }
        [Column("at_situacao_item_embalagem")]
        public int AtSituacaoItemEmbalagem { get; set; }
        [Column("situacao_item_embalagem")]
        public required string SituacaoItemEmbalagem { get; set; }
        [Column("at_situacao_embalagem")]
        public int AtSituacaoEmbalagem { get; set; }
        [Column("situacao_embalagem")]
        public required string SituacaoEmbalagem { get; set; }
    }
}