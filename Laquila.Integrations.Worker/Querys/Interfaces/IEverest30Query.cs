using Laquila.Integrations.Worker.Clients.Inbound;
using Laquila.Integrations.Worker.Clients.MasterData;
using Laquila.Integrations.Worker.Clients.Outbound;

namespace Laquila.Integrations.Worker.Querys.Interfaces
{
    public interface IEverest30Query
    {
        IInboundClient Inbound { get; }
        IMasterDataClient MasterData { get; }
        IOutboundClient Outbound { get; }
    }
}