using Laquila.Integrations.Application.DTO.Auth.Request;
using Laquila.Integrations.Application.DTO.Auth.Response;
using Laquila.Integrations.Application.Helpers;
using Laquila.Integrations.Core.Domain.DTO.Inbound.Invoices.Request;
using Laquila.Integrations.Core.Domain.DTO.MasterData.Items;
using Laquila.Integrations.Core.Domain.DTO.MasterData.Mandators.Request;
using Laquila.Integrations.Core.Domain.DTO.Prenota.Request;
using Laquila.Integrations.Core.Domain.DTO.Shared;
using Laquila.Integrations.Core.Domain.Filters;
using Laquila.Integrations.Core.Shared;
using Laquila.Integrations.Worker.Clients.Inbound;
using Laquila.Integrations.Worker.Clients.MasterData;
using Laquila.Integrations.Worker.Clients.Outbound;
using Laquila.Integrations.Worker.Context;
using Laquila.Integrations.Worker.Querys.Interfaces;
using Microsoft.Extensions.Hosting;
using RestSharp;


namespace Laquila.Integrations.Worker.Querys
{
    public class Everest30Query : IEverest30Query
    {
        public IOutboundClient Outbound { get; }
        public IInboundClient Inbound { get; }
        public IMasterDataClient MasterData { get; }

        public Everest30Query(
            IOutboundClient outbound,
            IInboundClient inbound,
            IMasterDataClient masterData)
        {
            Outbound = outbound;
            Inbound = inbound;
            MasterData = masterData;
        }
    }
}