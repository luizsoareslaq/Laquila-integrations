using Laquila.Integrations.Application.Interfaces;
using Laquila.Integrations.Core.Domain.DTO.Prenota.Request;
using Laquila.Integrations.Core.Domain.DTO.Shared;
using Laquila.Integrations.Core.Infra.Interfaces;
using static Laquila.Integrations.Application.Exceptions.ApplicationException;

namespace Laquila.Integrations.Infrastructure.ExternalServices
{

    public class ExternalService : IExternalService
    {
        public async Task<ResponseDto> SendPrenotasAsync(PrenotaDTO dto)
        {
            try
            {
                throw new Exception();

                return new ResponseDto { Data = new ResponseDataDto { StatusCode = "200", Message = "Success" } };
            }
            catch (Exception ex)
            {
                throw new CustomErrorException(450,"oi teste","lo_oe","Orders","123456");
            }
        }
    }
}