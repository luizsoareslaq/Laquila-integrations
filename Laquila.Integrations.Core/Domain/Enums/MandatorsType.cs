using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Laquila.Integrations.Core.Domain.Enums
{
    public enum MandatorsType
    {
        Cliente = 1,
        Fornecedor = 2,
        ClienteFornecedor = 3,
        Transportadora = 4,
        ClienteFornecedorTransportadora = 7,
    }
}