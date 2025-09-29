using System.Data;

namespace Laquila.Integrations.Core.Infra.Interfaces
{
    public interface IDbConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}