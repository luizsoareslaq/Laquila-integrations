using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Antiforgery;

namespace Laquila.Integrations.Core.Domain.Models
{
    public class VMWMS_BuscarRecebimentosNaoIntegrados
    {
        public int LiId { get; set; }
        public required string LiMaCnpjOwner { get; set; }
        public int LiPriority { get; set; }
        public required string FnMaCnpj { get; set; }
        public required string FnInvNumber { get; set; }
        public int FnType { get; set; }
        public required string FnSerialNr { get; set; }
        public int FnlId { get; set; }
        public required string FnlAtId { get; set; }
        public int FnlQty { get; set; }
    }
}