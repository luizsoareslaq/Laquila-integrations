using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Laquila.Integrations.Domain.Models.Everest30
{
    [Table("LOAD_OUT")]
    public class LoadOut
    {
        [Key]
        [Column("LO_ID")]
        public int LoId { get; set; }

        [Column("LO_MA_CNPJ_OWNER")]
        public string LoMaCnpjOwner { get; set; }

        [Column("LO_MA_CNPJ")]
        public string LoMaCnpj { get; set; }

        [Column("LO_MA_CNPJ_CARRIER")]
        public string LoMaCnpjCarrier { get; set; }

        [Column("LO_OE")]
        public int LoOe { get; set; }

        [Column("LO_TYPE")]
        public int LoType { get; set; }

        [Column("LO_PRIORITY")]
        public int LoPriority { get; set; }

        [Column("LO_SITUATION")]
        public int LoSituation { get; set; }

        [Column("LO_STATUS")]
        public int LoStatus { get; set; }

        [Column("LO_STATUSOLD")]
        public int LoStatusOld { get; set; }

        [Column("LO_DTINI_PICKING")]
        public DateTime? LoDtIniPicking { get; set; }

        [Column("LO_DTEND_PICKING")]
        public DateTime? LoDtEndPicking { get; set; }

        [Column("LO_PICKING_USER")]
        public string? LoPickingUser { get; set; }

        [Column("LO_DTINI_CONF")]
        public DateTime? LoDtIniConf { get; set; }

        [Column("LO_DTEND_CONF")]
        public DateTime? LoDtEndConf { get; set; }

        [Column("LO_CONF_USER")]
        public string? LoConfUser { get; set; }

        [Column("LO_DTINI_LOADING")]
        public DateTime? LoDtIniLoading { get; set; }

        [Column("LO_DTEND_LOADING")]
        public DateTime? LoDtEndLoading { get; set; }

        [Column("LO_LOADING_USER")]
        public string? LoLoadingUser { get; set; }

        [Column("LO_GENTIME")]
        public DateTime LoGenTime { get; set; }

        [Column("LO_GENUSER")]
        public required string LoGenUser { get; set; }

        [Column("LO_MODTIME")]
        public DateTime LoModTime { get; set; }

        [Column("LO_MODUSER")]
        public required string LoModUser { get; set; }

        [Column("LO_CC")]
        public int? LoCc { get; set; }

        [Column("LO_REASON")]
        public string? LoReason { get; set; }

        [Column("LO_MA_CNPJ_REDESPACHO")]
        public string? LoCnpjRedespacho { get; set; }

        [Column("LO_CONTRA_ORDEM")]
        public string? LoContraOrdem { get; set; }

        [Column("LO_CARIMBO")]
        public string? LoCarimbo { get; set; }

        [Column("LO_PRINT_REDESPACHO")]
        public int? LoPrintRedespacho { get; set; }

        public virtual ICollection<Orders> Orders { get; set; } = new List<Orders>();
    }
}