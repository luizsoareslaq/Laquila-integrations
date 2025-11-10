using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Laquila.Integrations.Domain.Models.Everest30
{
    public class Article
    {
        [Key]
        [Column("AT_ID")]
        public string AtId { get; set; } = null!;
        [Column("AT_LAST_UPDATE_API")]
        public DateTime? AtLastUpdateApi { get; set; }
    }
}