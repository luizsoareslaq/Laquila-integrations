using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Laquila.Integrations.Core.Domain.DTO.Shared;
using static Laquila.Integrations.Application.Exceptions.ApplicationException;

namespace Laquila.Integrations.Application.Helpers
{
    public class ErrorCollector
    {
        private readonly List<ResponseErrorsDto> _errors = new();

        public void Add(string entity, string key, string value, string message,bool thr = false,  int statusCode = 400)
        {
            _errors.Add(new ResponseErrorsDto
            {
                Entity = entity,
                Key = key,
                Value = value,
                Message = message,
                StatusCode = statusCode
            });
            
            if (thr)
                ThrowIfAny();
        }

        public void ThrowIfAny()
        {
            if (_errors.Any())
                throw new ResponseErrorException(_errors);
        }
    }
}