using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Channels;
using System.Threading.Tasks;
using Laquila.Integrations.Domain.Models;

namespace Laquila.Integrations.API.Logging
{
    public static class LogChannel
    {
        public static readonly Channel<LaqApiLogs> Channel =
            System.Threading.Channels.Channel.CreateUnbounded<LaqApiLogs>();
    }
}