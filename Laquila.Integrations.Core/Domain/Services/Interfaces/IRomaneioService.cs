using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Laquila.Integrations.Core.Domain.DTO.Romaneio.Response;
using Laquila.Integrations.Core.Domain.DTO.Shared;
using Laquila.Integrations.Core.Domain.Filters;
using Laquila.Integrations.Core.Domain.Models;

namespace Laquila.Integrations.Core.Domain.Services.Interfaces
{
    public interface IRomaneioService
    {
        Task<PagedResult<RomaneioResponseDTO>> GetRomaneiosAsync(LAQFilters filters, CancellationToken ct);
    }
}