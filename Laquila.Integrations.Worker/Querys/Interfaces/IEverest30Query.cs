using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Laquila.Integrations.Core.Domain.DTO.Prenota.Request;
using Laquila.Integrations.Core.Domain.DTO.Shared;
using Laquila.Integrations.Core.Domain.Filters;

namespace Laquila.Integrations.Worker.Querys.Interfaces
{
    public interface IEverest30Query
    {
        Task<PagedResult<PrenotaDTO>> GetPrenotas(LAQFilters filters, CancellationToken ct);
    }
}