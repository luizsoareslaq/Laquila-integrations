using Laquila.Integrations.Application.Interfaces;
using Laquila.Integrations.Core.Domain.DTO.Prenota.Request;
using Laquila.Integrations.Core.Domain.DTO.Shared;
using Laquila.Integrations.Core.Infra.Interfaces;

namespace Laquila.Integrations.Infrastructure.ExternalServices
{

    public class ExternalService : IExternalService
    {
        private readonly IPrenotaRepository _prenotaRepository;
        public ExternalService(IPrenotaRepository prenotaRepository)
        {
            _prenotaRepository = prenotaRepository;
        }

        public async Task<ResponseDto> SendPrenotasAsync(PrenotaDTO dto)
        {
            return null;
        }
    }
}