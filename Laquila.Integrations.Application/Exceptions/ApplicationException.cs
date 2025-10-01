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

        public class CustomErrorException : Exception
        {
            public int StatusCode { get; }
            public string Key { get; }
            public string Entity { get; }
            public string Value { get; }

            public CustomErrorException(int statusCode, string message, string key = "", string entity = "", string value = "")
                : base(message)
            {
                StatusCode = statusCode;
                Key = key;
                Value = value;
                Entity = entity;                
            }
        }
    }
}