namespace Laquila.Integrations.Domain.Filters
{
    public class CompanyFilters
    {
        public Guid? Id { get; set; }
        public int? ErpCode { get; set; }
        public string? Document { get; set; }
        public int? StatusId { get; set; }
    }
}