using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Laquila.Integrations.Core.Domain.DTO.MasterData.Items;
using Laquila.Integrations.Core.Domain.DTO.MasterData.Mandators.Request;
using Laquila.Integrations.Core.Domain.DTO.Shared;
using Laquila.Integrations.Core.Domain.Filters;
using Laquila.Integrations.Core.Domain.Services.Interfaces;
using Laquila.Integrations.Core.Infra.Interfaces;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using Microsoft.AspNetCore.Mvc.ViewEngines;

namespace Laquila.Integrations.Application.Services.Everest30
{
    public class MasterDataService : IMasterDataService
    {
        private readonly IViewsRepository _viewsRepository;
        public MasterDataService(IViewsRepository viewsRepository)
        {
            _viewsRepository = viewsRepository;
        }

        public async Task<MasterDataItemsPackageDTO> GetUnsentItemsAsync(LAQFilters filters, CancellationToken ct)
        {
            (var itemsList, int TotalCount) = await _viewsRepository.GetItemsAsync(new List<string>(), filters.PageSize);

            var itens = new MasterDataItemsPackageDTO(new List<ItemsDetailsDTO>());

            if (itemsList.Count() > 0)
            {
                itens.ItemAttributes.AddRange(
                    itemsList.Select(item => new ItemsDetailsDTO
                    {
                        AtId = item.AtId,
                        AtDesc = item.AtDesc,
                        AtType = item.AtType,
                        AtbBoxId = item.AtbBoxId,
                        AtbGrossWeight = item.AtbGrossWeight,
                        AtbNetWeight = item.AtbNetWeight,
                        AtbFlStandardPackage = item.AtbFlStandardPackage,
                        AtbHeight = item.AtbHeight,
                        AtbWidth = item.AtbWidth,
                        AtbLength = item.AtbLength,
                        AtbM3 = item.AtbM3,
                        AtbUnitsPerPallet = item.AtbUnitsPerPallet,
                        AtbUnitsPerLayer = item.AtbUnitsPerLayer,
                        AtbMaxStacking = item.AtbMaxStacking,
                        AtbVolumeFactor = item.AtbVolumeFactor,
                        AtbQtyPerPackage = item.AtbQtyPerPackage,
                        AtbEanType = item.AtbEanType,
                        AtbEanBarcode = item.AtbEanBarcode
                    })
                );

            }
            ;

            return itens;
        }

        public async Task<MasterDataMandatorsDTO> GetUnsentMandatorAsync(LAQFilters filters, CancellationToken ct)
        {
            (var mandatorsList, int TotalCount) = await _viewsRepository.GetMandatorsAsync(new List<string>(), "cnpj", filters.PageSize);

            var mandators = new MasterDataMandatorsDTO(new List<MandatorsAttributesDTO>());

            if (mandatorsList.Count() > 0)
            {
                mandators.Mandators.AddRange(
                    mandatorsList.Select(mandator => new MandatorsAttributesDTO
                    {
                        MaName = mandator.MaName,
                        MaCnpj = mandator.MaCnpj,
                        MaZip = mandator.MaZip,
                        MaAddress = mandator.MaAddress,
                        MaCity = mandator.MaCity,
                        MaTel = mandator.MaTel,
                        MaFax = mandator.MaFax,
                        MaType = mandator.MaType,
                        MaCode = mandator.MaCode,
                        MaDistrict = mandator.MaDistrict
                    })
                );
            };

            return mandators;
        }
    }
}