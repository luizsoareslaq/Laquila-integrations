using Laquila.Integrations.Core.Domain.DTO.MasterData.Items;
using Laquila.Integrations.Core.Domain.DTO.MasterData.Mandators.Request;
using Laquila.Integrations.Core.Domain.DTO.Outbound.Invoices.Request;
using Laquila.Integrations.Core.Domain.DTO.Prenota.Request;
using Laquila.Integrations.Core.Domain.DTO.Shared;

namespace Laquila.Integrations.Application.Interfaces
{
    public interface IExternalService
    {
        Task<ResponseDto> SendInvoicesAsync(InvoiceDTO dto, Guid apiIntegrationId);
        Task<ResponseDto> SendOrdersAsync(PrenotaDTO dto, Guid apiIntegrationId);
        Task<ResponseDto> SendItemsAsync(MasterDataItemsPackageDTO dto, Guid apiIntegrationId);
        Task<ResponseDto> SendMandatorAsync(MasterDataMandatorsDTO dto, Guid apiIntegrationId);
    }
}