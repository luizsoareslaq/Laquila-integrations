using Laquila.Integrations.Core.Domain.DTO.MasterData.Items;
using Laquila.Integrations.Core.Domain.DTO.MasterData.Mandators.Request;
using Laquila.Integrations.Core.Domain.DTO.Shared;
using Laquila.Integrations.Core.Domain.Filters;
using Laquila.Integrations.Core.Domain.Services.Interfaces;
using Laquila.Integrations.Core.Infra.Interfaces;
using Laquila.Integrations.Core.Shared;
using Laquila.Integrations.Domain.Interfaces.Repositories.Everest30;
using Laquila.Integrations.Domain.Models.Everest30;

namespace Laquila.Integrations.Application.Services.Everest30
{
    public class MasterDataService : IMasterDataService
    {
        private readonly IViewsRepository _viewsRepository;
        private readonly IMasterDataRepository _masterDataRepository;
        public MasterDataService(IViewsRepository viewsRepository
                                , IMasterDataRepository masterDataRepository)
        {
            _viewsRepository = viewsRepository;
            _masterDataRepository = masterDataRepository;
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
            }
            ;

            return mandators;
        }

        public async Task<ResponseDto> HandleItemsAsync(MasterDataItemsPackageDTO dto)
        {
            var distinctItems = dto.ItemAttributes
                .DistinctBy(x => x.AtId)
                .ToList();

            var atIds = distinctItems
                .Select(x => x.AtId)
                .ToList();

            var existingItems = await _masterDataRepository.GetItemsByAtIdAsync(atIds);
            var existingIds = existingItems.Select(e => e.AtId).ToHashSet();

            List<Article> itemsToInsert = new();
            List<Article> itemsToUpdate = new();

            foreach (var itemDto in distinctItems)
            {
                var model = MappingHelpers.ArticleDtoToModel(itemDto);

                if (existingIds.Contains(model.AtId))
                    itemsToUpdate.Add(model);
                else
                    itemsToInsert.Add(model);
            }

            if (itemsToInsert.Count > 0)
                await _masterDataRepository.InsertItemsAsync(itemsToInsert);

            if (itemsToUpdate.Count > 0)
                await _masterDataRepository.UpdateItemsAsync(itemsToUpdate);

            var response = new ResponseDto
            {
                Data = new ResponseDataDto
                {
                    StatusCode = "200",
                    Message = "Items processed successfully."
                },
            };

            return response;
        }

        public async Task<ResponseDto> HandleMandatorsAsync(MasterDataMandatorsDTO dto)
        {
            List<long> maCodes = dto.Mandators.Select(m => m.MaCode).ToList();
            var existingMandators = await _masterDataRepository.GetMandatorsByMaCodeAsync(maCodes);

            List<Mandator> mandatorsToInsert = new List<Mandator>();
            List<Mandator> mandatorsToUpdate = new List<Mandator>();

            foreach (var mandatorDto in dto.Mandators)
            {
                var existingMandator = existingMandators.FirstOrDefault(m => m.MaCode == mandatorDto.MaCode);
                if (existingMandator != null)
                {
                    var updatedMandator = MappingHelpers.MandatorDtoToModel(mandatorDto);
                    mandatorsToUpdate.Add(updatedMandator);
                }
                else
                {
                    var newMandator = MappingHelpers.MandatorDtoToModel(mandatorDto);
                    mandatorsToInsert.Add(newMandator);
                }
            }

            if (mandatorsToInsert.Count > 0)
                await _masterDataRepository.InsertMandatorsAsync(mandatorsToInsert);

            if (mandatorsToUpdate.Count > 0)
                await _masterDataRepository.UpdateMandatorsAsync(mandatorsToUpdate);

            var response = new ResponseDto
            {
                Data = new ResponseDataDto
                {
                    StatusCode = "200",
                    Message = "Mandators processed successfully."
                },
            };

            return response;
        }
    }
}