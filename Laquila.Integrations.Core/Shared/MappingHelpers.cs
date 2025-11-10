using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Laquila.Integrations.Core.Domain.DTO.MasterData.Items;
using Laquila.Integrations.Core.Domain.DTO.MasterData.Mandators.Request;
using Laquila.Integrations.Core.Domain.Enums;
using Laquila.Integrations.Domain.Models.Everest30;

namespace Laquila.Integrations.Core.Shared
{
    public static class MappingHelpers
    {
        public static Article ArticleDtoToModel(ItemsDetailsDTO dto)
        {
            return new Article()
            {
                AtId = dto.AtId,
                AtLastUpdateApi = DateTime.Now
            };
        }

        public static Mandator MandatorDtoToModel(MandatorsAttributesDTO dto)
        {
            return new Mandator()
            {
                MaName = dto.MaName,
                MaCnpj = dto.MaCnpj,
                MaZip = dto.MaZip,
                MaAddress = dto.MaAddress,
                MaCity = dto.MaCity,
                MaTel = dto.MaTel,
                MaFax = dto.MaFax,
                MaType = (int)dto.MaType,
                MaCode = dto.MaCode,
                MaDistrict = dto.MaDistrict,
                MaState = dto.MaState,
                MaLastUpdateApi = DateTime.Now
            };
        }

        
    }
}