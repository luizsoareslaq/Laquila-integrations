using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Laquila.Integrations.Domain.Models
{
    public class LaqApiStatus 
    {
        public LaqApiStatus(int statusId, string description)
        {
            StatusId = statusId;
            Description = description;
        }
        
        [Key]
        [Column("status_id")]
        public int StatusId { get; set; }
        [Column("description")]
        public string Description { get; set; }
    }
}