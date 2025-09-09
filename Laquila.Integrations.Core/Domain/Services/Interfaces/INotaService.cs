using Laquila.Integrations.Core.Domain.DTO.Notas.Response;
using Laquila.Integrations.Core.Domain.DTO.Shared;
using Laquila.Integrations.Core.Domain.Filters;

namespace Laquila.Integrations.Core.Domain.Services.Interfaces
{
    public interface INotaService
    {
        Task<PagedResult<NotasResponseDTO>> GetNotasAsync(LAQFilters filters, CancellationToken ct);
    }
}