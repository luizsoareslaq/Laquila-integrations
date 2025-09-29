
namespace Laquila.Integrations.Application.DTO.Company.Request
{
    public class CompanyDTO
    {
        public CompanyDTO(int erpCode, string companyName, string document, int statusId = 1)
        {
            ErpCode = erpCode;
            CompanyName = companyName;
            Document = document;
            StatusId = statusId;
        }
        public int ErpCode { get; set; }
        public string CompanyName { get; set; }
        public string Document { get; set; }
        public int StatusId { get; set; } = 1;
    }
}