using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Laquila.Integrations.Domain.Models.Everest30
{
    [Table("ORDERS")]
    public class Orders
    {
        [Key]
        [Column("OE_ID")]
        public int OeId { get; set; }
        [Column("OE_LO_ID")]
        public int OeLoId { get; set; }
        [Column("OE_STATUS")]
        public int OeStatus { get; set; }
        [Column("OE_GENTIME")]
        public DateTime OeGenTime { get; set; }
        [Column("OE_MODTIME")]
        public DateTime OeModTime { get; set; }
        [Column("OE_GENUSER")]
        public string OeGenUser { get; set; }
        [Column("OE_MODUSER")]
        public string OeModUser { get; set; }
        [Column("OE_CC")]
        public int OeCc { get; set; }
        [Column("OE_INVNUMBER")]
        public string? OeInvNumber { get; set; }
        [Column("OE_STATUSOLD")]
        public int OeOldStatus { get; set; }
        [Column("OE_ERP_ORDER")]
        public int OeErpOrder { get; set; }
        [Column("OE_SERIALNR")]
        public string? OeSerialNr { get; set; }
        [Column("OE_DTINI_BILLING")]
        public DateTime? OeDtIniBilling { get; set; }
        [Column("OE_DTEND_BILLING")]
        public DateTime? OeDtEndBilling { get; set; }
    }
}