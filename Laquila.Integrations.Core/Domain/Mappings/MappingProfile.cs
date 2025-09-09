using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Laquila.Integrations.Core.Domain.DTO.Notas.Response;
using Laquila.Integrations.Core.Domain.DTO.Romaneio.Response;
using Laquila.Integrations.Core.Domain.Models;

namespace Laquila.Integrations.Core.Domain.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile() {
            CreateMap<VLAQConsultarNotas, NotasResponseDTO>().ReverseMap();
            CreateMap<VLAQConsultarRomaneioItens, RomaneioResponseDTO>().ReverseMap();
        }
    }
}