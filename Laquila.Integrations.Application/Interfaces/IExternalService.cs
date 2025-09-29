using Laquila.Integrations.Core.Domain.DTO.Prenota.Request;
using Laquila.Integrations.Core.Domain.DTO.Shared;

namespace Laquila.Integrations.Application.Interfaces
{
    public interface IExternalService
    {
        public Task<ResponseDto> SendPrenotasAsync(PrenotaDTO dto);
    }
}