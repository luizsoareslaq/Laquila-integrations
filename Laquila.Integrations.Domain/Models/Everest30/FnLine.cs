using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Laquila.Integrations.Domain.Models.Everest30
{
    [Table("FN_LINE")]
    public class FnLine
    {
        [Key]
        [Column("FNL_ID")]
        public int FnlId { get; set; }

        [Column("FNL_FN_ID")]
        public int FnlFnId { get; set; }

        [Column("FNL_AT_ID")]
        public string FnlAtId { get; set; }

        [Column("FNL_QTY")]
        public int FnlQty { get; set; }

        [Column("FNL_MESSAGE")]
        public int FnlMessage { get; set; }

        [Column("FNL_STATUS")]
        public int FnlStatus { get; set; }

        [Column("FNL_STATUSOLD")]
        public int FnlStatusOld { get; set; }

        [Column("FNL_QTYRECV")]
        public int? FnlQtyRecv { get; set; }

        [Column("FNL_LOTNR")]
        public string? FnlLotnr { get; set; }

        [Column("FNL_PRODDATE")]
        public DateTime? FnlProdDate { get; set; }

        [Column("FNL_REASON")]
        public string? FnlReason { get; set; }

        [Column("FNL_GENTIME")]
        public DateTime FnlGentime { get; set; }

        [Column("FNL_GENUSER")]
        public string FnlGenuser { get; set; }

        [Column("FNL_MODTIME")]
        public DateTime FnlModtime { get; set; }

        [Column("FNL_MODUSER")]
        public string FnlModuser { get; set; }

        [Column("FNL_CC")]
        public int? FnlCc { get; set; }
    }
}