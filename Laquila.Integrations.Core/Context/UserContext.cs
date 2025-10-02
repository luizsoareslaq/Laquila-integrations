using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Laquila.Integrations.Core.Context
{
    public static class UserContext
    {
        private static readonly AsyncLocal<string?> _companyCnpj = new();

        public static string? CompanyCnpj
        {
            get => _companyCnpj.Value;
            set => _companyCnpj.Value = value;
        }

        public static void Clear()
        {
            _companyCnpj.Value = null;
        }
    }
}