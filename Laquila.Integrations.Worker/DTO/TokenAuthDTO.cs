using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Laquila.Integrations.Worker.DTO
{
    public class TokenAuthDTO
    {
        public string? Login { get; set; }
        public string? Password { get; set; }
        public string? Token { get; set; }
    }
}