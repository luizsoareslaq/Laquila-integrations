using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Laquila.Integrations.Application.Exceptions
{
    public class ApplicationException
    {
        public class NotFoundException : Exception
        {
            public NotFoundException(string message) : base(message) { }
        }

        public class BadRequestException : Exception
        {
            public BadRequestException(string message) : base(message) { }
        }

        public class UnauthorizedError : Exception
        {
            public UnauthorizedError(string message) : base(message) { }
        }

        public class EntityNotFoundAfterCreated : Exception
        {
            public EntityNotFoundAfterCreated(string entity) : base($"{entity} was not found after creation.") { }
        }

        public class EntityNotFoundAfterUpdated : Exception
        {
            public EntityNotFoundAfterUpdated(string entity) : base($"{entity} was not found after updated.") { }
        }
    }
}