using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Laquila.Integrations.Core.Domain.Filters;
using Laquila.Integrations.Core.Domain.Models;

namespace Laquila.Integrations.Core.Domain.Interfaces
{
    public interface ILaquilaRepository
    {
        Task<IEnumerable<VLAQConsultarNotas>> ConsultarNotasAsync(LAQFilters filters, CancellationToken ct );
    }
}