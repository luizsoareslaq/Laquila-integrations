using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Laquila.Integrations.Core.Domain.Models
{
    public class VLAQConsultarEmbalagens
    {
        [Column("id_embalagem")]
        public long IdEmbalagem { get; set; }
        [Column("ds_embalagem")]
        public required string DsEmbalagem { get; set; }
        [Column("qt_item")]
        public decimal QtItem { get; set; }
        [Column("qt_empilhamento")]
        public decimal QtEmpilhamento { get; set; }
        [Column("qt_unidade_pallet")]
        public decimal QtUnidadePallet { get; set; }
        [Column("qt_unidade_camada_pallet")]
        public decimal QtUnidadeCamadaPallet { get; set; }
        [Column("at_situacao")]
        public int AtSituacao { get; set; }
        [Column("situacao_embalagem")]
        public required string SituacaoEmbalagem { get; set; }
    }
}