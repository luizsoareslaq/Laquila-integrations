using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Laquila.Integrations.Application.Interfaces;
using Laquila.Integrations.Domain.Interfaces.Repositories.Everest30;
using Laquila.Integrations.Domain.Models.Everest30;
using Laquila.Integrations.Infrastructure.Contexts;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace Laquila.Integrations.Infrastructure.Repositories.Everest30
{
    public class MasterDataRepository : IMasterDataRepository
    {
        private readonly Everest30Context _db;
        private readonly IUnitOfWork _unitOfWork;
        public MasterDataRepository(Everest30Context db
                                 , IUnitOfWork unitOfWork)
        {
            _db = db;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<Article>> GetItemsByAtIdAsync(List<string> atIds)
        {
            var itens = await _db.Article.Where(a => atIds.Contains(a.AtId))
                            .AsNoTracking()
                            .ToListAsync();

            return itens;
        }
        public async Task InsertItemsAsync(List<Article> items)
        {
            await _unitOfWork.DetachEverest30EntitiesAsync(items);

            _db.ChangeTracker.Clear();
            _db.Article.AddRange(items);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateItemsAsync(List<Article> items)
        {
            _db.ChangeTracker.Clear();

            foreach (var item in items)
            {
                _db.Attach(item);
                _db.Entry(item).State = EntityState.Modified;
            }

            await _db.SaveChangesAsync();
        }


        public async Task<List<Mandator>> GetMandatorsByMaCodeAsync(List<long> maCodes)
        {
            var mandators = await _db.Mandator.Where(m => maCodes.Contains(m.MaCode))
                            .AsNoTracking()
                            .ToListAsync();

            return mandators;
        }
        public async Task InsertMandatorsAsync(List<Mandator> mandators)
        {
            _db.ChangeTracker.Clear();
            _db.Mandator.AddRange(mandators);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateMandatorsAsync(List<Mandator> mandators)
        {
            _db.ChangeTracker.Clear();

            foreach (var mandator in mandators)
            {
                _db.Attach(mandator);
                _db.Entry(mandator).State = EntityState.Modified;
            }

            await _db.SaveChangesAsync();
        }
    }
}