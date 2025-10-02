using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Laquila.Integrations.Domain.Models.Everest30
{
    [Table("FISCAL_NOTES")]
    public class FiscalNotes
    {
        [Key]
        [Column("FN_ID")]
        public int FnId { get; set; }

        [Column("FN_LI_ID")]
        public int FnLiId { get; set; }

        [Column("FN_MA_CNPJ")]
        public string FnMaCnpj { get; set; }

        [Column("FN_INVNUMBER")]
        public string FnInvnumber { get; set; }

        [Column("FN_TYPE")]
        public int FnType { get; set; }

        [Column("FN_STATUS")]
        public int FnStatus { get; set; }

        [Column("FN_STATUSOLD")]
        public int FnStatusold { get; set; }

        [Column("FN_SERIALNR")]
        public string? FnSerialnr { get; set; }

        [Column("FN_GENTIME")]
        public DateTime FnGentime { get; set; }

        [Column("FN_GENUSER")]
        public string FnGenuser { get; set; }

        [Column("FN_MODTIME")]
        public DateTime FnModtime { get; set; }

        [Column("FN_MODUSER")]
        public string FnModuser { get; set; }

        [Column("FN_CC")]
        public int? FnCc { get; set; }

        [Column("FN_REASON")]
        public string? FnReason { get; set; }

        [Column("FN_DI")]
        public string? FnDi { get; set; }
    }
}