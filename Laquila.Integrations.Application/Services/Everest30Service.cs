using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Laquila.Integrations.Application.Interfaces;
using Laquila.Integrations.Domain.Interfaces.Repositories;
using Laquila.Integrations.Domain.Models.Everest30;

namespace Laquila.Integrations.Application.Services
{
    public class Everest30Service : IEverest30Service
    {
        private readonly IEverest30Repository _everest30Repository;
        public Everest30Service(IEverest30Repository everest30Repository)
        {
            _everest30Repository = everest30Repository;
        }

        public async Task<LoadOut> GetLoadOutByLoOe(long loOe, string companyCnpj)
        {
            return await _everest30Repository.GetLoadOutByLoOe(loOe, companyCnpj);
        }
    }
}