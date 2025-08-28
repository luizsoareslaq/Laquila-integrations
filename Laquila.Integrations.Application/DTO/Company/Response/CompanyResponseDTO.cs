using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Laquila.Integrations.Application.DTO.Users;

namespace Laquila.Integrations.Application.DTO.Company.Response
{
    public class CompanyResponseDTO
    {
        public CompanyResponseDTO(Guid id, int erpCode, string companyName, string document, string? status)
        {
            Id = id;
            ErpCode = erpCode;
            CompanyName = companyName;
            Document = document;
            Status = status;
        }
        public Guid Id { get; set; }
        public int ErpCode { get; set; }
        public string CompanyName { get; set; }
        public string Document { get; set; }
        public string? Status { get; set; }

        [JsonIgnore]
        public virtual ICollection<UserResponseDTO> Users { get; set; }
    }
}