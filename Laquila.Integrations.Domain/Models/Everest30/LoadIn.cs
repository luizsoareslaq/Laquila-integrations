using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Laquila.Integrations.Domain.Models.Everest30
{
    [Table("LOAD_IN")]
    public class LoadIn
    {
        [Key]
        [Column("LI_ID")]
        public int LiId { get; set; }
        [Column("LI_TYPE")]
        public int LiType { get; set; }
        [Column("LI_PRIORITY")] 
        public int LiPriority { get; set; }
        [Column("LI_MA_CNPJ_OWNER")] 
        public string LiMaCnpjOwner { get; set; }
        [Column("LI_STATUS")] 
        public int LiStatus { get; set; }
        [Column("LI_STATUSOLD")] 
        public int LiStatusOld { get; set; }
        [Column("LI_DTINI_UNLOAD")] 
        public DateTime? LiDtIniUnload { get; set; }
        [Column("LI_DTEND_UNLOAD")] 
        public DateTime? LiDtEndUnload { get; set; }
        [Column("LI_UNLOAD_USER")] 
        public string? LiUnloadUser { get; set; }
        [Column("LI_DTINI_CONF")] 
        public DateTime? LiDtIniConf { get; set; }
        [Column("LI_DTEND_CONF")] 
        public DateTime? LiDtEndConf { get; set; }
        [Column("LI_CONF_USER")] 
        public string? LiConfUser { get; set; }
        [Column("LI_DTINI_STORE")] 
        public DateTime? LiDtIniStore { get; set; }
        [Column("LI_DTEND_STORE")] 
        public DateTime? LiDtEndStore { get; set; }
        [Column("LI_STORE_USER")] 
        public string? LiStoreUser { get; set; }
        [Column("LI_REASON")] 
        public string? LiReason { get; set; }
        [Column("LI_GENTIME")] 
        public DateTime LiGenTime { get; set; }
        [Column("LI_GENUSER")] 
        public string LiGenUser { get; set; }
        [Column("LI_MODTIME")] 
        public DateTime LiModTime { get; set; }
        [Column("LI_MODUSER")] 
        public string LiModUser { get; set; }
        [Column("LI_CC")] 
        public int? LiCc { get; set; }
        [Column("LI_OE")] 
        public string? LiOe { get; set; }
    }
}