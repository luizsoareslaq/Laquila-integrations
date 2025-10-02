using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Laquila.Integrations.Domain.Models.Everest30;

namespace Laquila.Integrations.Application.Interfaces
{
    public interface IEverest30Service
    {
        Task<LoadOut> GetLoadOutByLoOe(long loOe, string companyCnpj);
    }
}