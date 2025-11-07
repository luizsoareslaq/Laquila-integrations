using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Laquila.Integrations.Core.Domain.Models;

namespace Laquila.Integrations.Core.Infra.Interfaces
{
    public interface IViewsRepository
    {
        Task<(IEnumerable<VMWMS_BuscarPrenotasNaoIntegradas>, int totalCount)> GetOrdersAsync(List<(string loMaCnpjOwner,long oeErpOrder)>? prenotas, int pageSize = 10);
        Task<(IEnumerable<VMWMS_BuscarItensNaoIntegrados>, int totalCount)> GetItemsAsync(List<string>? itens, int pageSize = 10);
        Task<(IEnumerable<VMWMS_BuscarCadastrosNaoIntegrados>, int totalCount)> GetMandatorsAsync(List<string>? cadastros, string cnpjOuCodigo, int pageSize = 10);
    }
}