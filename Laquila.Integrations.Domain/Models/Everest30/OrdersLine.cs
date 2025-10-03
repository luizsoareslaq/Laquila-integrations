using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Laquila.Integrations.Domain.Models.Everest30
{
    [Table("ORDERS_LINE")]
    public class OrdersLine
    {
        [Key]
        [Column("OEL_ID")]
        public int OelId { get; set; }
        [Column("OEL_OE_ID")]
        public int OelOeId { get; set; }
        [Column("OEL_AT_ID")]
        public string OelAtId { get; set; }
        [Column("OEL_QTYREQ")]
        public decimal OelQtyReq { get; set; }
        [Column("OEL_STATUS")]
        public int OelStatus { get; set; }
        [Column("OEL_LOTNR")]
        public string? OelLotNr { get; set; }
        [Column("OEL_GENTIME")]
        public DateTime OelGenTime { get; set; }
        [Column("OEL_MODTIME")]
        public DateTime OelModTime { get; set; }
        [Column("OEL_GENUSER")]
        public string OelGenUser { get; set; }
        [Column("OEL_MODUSER")]
        public string OelModUser { get; set; }
        [Column("OEL_CC")]
        public int OelCc { get; set; }
        [Column("OEL_QTYATEND")]
        public decimal? OelQtyAtend { get; set; }
        [Column("OEL_QTYRENOUNCED")]
        public decimal? OelQtyRenounced { get; set; }
        [Column("OEL_STATUSOLD")]
        public int OelStatusOld { get; set; }
        [Column("OEL_CONFBOXID")]
        public string OelConfBoxId { get; set; }
        [Column("OEL_SITUATION")]
        public int OelSituation { get; set; }
        [Column("OEL_RENOUNCEREASON")]
        public int? OelRenounceReason { get; set; }
        [Column("OEL_MESSAGE")]
        public int OelMessage { get; set; }

        [JsonIgnore]
        [NotMapped]
        public virtual Orders Orders { get; set; }
    }
}