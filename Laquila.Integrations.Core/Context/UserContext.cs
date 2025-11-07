using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Laquila.Integrations.Core.Context
{
    public static class UserContext
    {
        private static readonly AsyncLocal<string?> _companyCnpj = new();
        private static readonly AsyncLocal<string?> _language = new ();

        public static string? CompanyCnpj
        {
            get => _companyCnpj.Value;
            set => _companyCnpj.Value = value;
        }

        public static string? Language
        {
            get => _language.Value;
            set => _language.Value = value;
        }

        public static void Clear()
        {
            _companyCnpj.Value = null;
            _language.Value = null;
        }
    }
}