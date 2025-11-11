using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Laquila.Integrations.Core.Domain.Models;
using Laquila.Integrations.Core.Infra.Interfaces;
using Laquila.Integrations.Infrastructure.Contexts;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.EntityFrameworkCore;

namespace Laquila.Integrations.Infrastructure.Repositories.Everest30
{
    public class ViewsRepository : IViewsRepository
    {
        private readonly Everest30Context _db;

        public ViewsRepository(Everest30Context db)
        {
            _db = db;
        }

        public async Task<(IEnumerable<VMWMS_BuscarItensNaoIntegrados>, int totalCount)> GetItemsAsync(List<string>? itens, int pageSize = 10)
        {
            var query = _db.VMWMS_BuscarItensNaoIntegrados.AsQueryable();

            if (itens != null && itens.Count > 0)
            {
                query = query.Where(i => itens.Contains(i.AtId));
            }

            var totalCount = await query.CountAsync();

            var result = await query.Take(pageSize).ToListAsync();

            return (result, totalCount);
        }

        public async Task<(IEnumerable<VMWMS_BuscarCadastrosNaoIntegrados>, int totalCount)> GetMandatorsAsync(List<string>? cadastros, string cnpjOuCodigo, int pageSize = 10)
        {
            var query = _db.VMWMS_BuscarCadastrosNaoIntegrados.AsQueryable();

            if (cadastros != null && cadastros.Count > 0)
            {
                if (cnpjOuCodigo == "cnpj")
                {
                    query = query.Where(c => cadastros.Contains(c.MaCnpj));
                }
                else
                {
                    List<long> codigos = cadastros
                                    .Where(c => long.TryParse(c, out _))
                                    .Select(long.Parse)
                                    .ToList();

                    query = query.Where(c => codigos.Contains(c.MaCode));
                }
            }

            var totalCount = await query.CountAsync();

            var result = await query.Take(pageSize).ToListAsync();

            return (result, totalCount);
        }

        public async Task<(IEnumerable<VMWMS_BuscarPrenotasNaoIntegradas>, int totalCount)> GetOrdersAsync(List<(string loMaCnpjOwner, long oeErpOrder)>? prenotas, int pageSize = 10)
        {
            var query = _db.VMWMS_BuscarPrenotasNaoIntegradas.AsQueryable();

            if (prenotas != null && prenotas.Count > 0)
            {
                query = query.Where(i =>
                    prenotas.Any(p =>
                        p.loMaCnpjOwner == i.LoMaCnpjOwner &&
                        p.oeErpOrder == i.OeErpOrder));
            }

            var result = await query.Take(pageSize).ToListAsync();

            var totalCount = result.Select(x=>x.LoOe).Distinct().Count();

            return (result, totalCount);
        }
    }
}