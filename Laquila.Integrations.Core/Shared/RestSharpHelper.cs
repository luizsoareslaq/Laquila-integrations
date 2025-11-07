using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Laquila.Integrations.Core.Domain.DTO.Shared;
using Microsoft.AspNetCore.Mvc;
using RestSharp;

namespace Laquila.Integrations.Core.Shared
{
    public static class RestSharpHelper
    {
        public static (RestClient, RestRequest) NewRestSharpClient(
            string url,
            object? body,
            object? queryParams,
            string? authType,
            TokenAuthDTO? authDto,
            string methodType)
        {
            var client = new RestClient(url);

            var method = methodType.ToLower() switch
            {
                "get" => Method.Get,
                "post" => Method.Post,
                "put" => Method.Put,
                "patch" => Method.Patch,
                "delete" => Method.Delete,
                _ => Method.Post
            };

            var request = new RestRequest(url, method);
            request.AddHeader("Content-Type", "application/json");

            if (!string.IsNullOrEmpty(authType) && authType.Equals("Bearer", StringComparison.OrdinalIgnoreCase) && authDto != null)
            {
                request.AddHeader("Authorization", $"Bearer {authDto.Token}");
            }

            if (queryParams != null)
            {
                var props = queryParams.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

                foreach (var prop in props)
                {
                    var value = prop.GetValue(queryParams);
                    if (value == null) continue;

                    var attr = prop.GetCustomAttribute<FromQueryAttribute>();
                    var paramName = attr?.Name ?? ToSnakeCase(prop.Name);

                    if (value is DateOnly dateOnly)
                        value = dateOnly.ToString("yyyy-MM-dd");
                    else if (value is DateTime dateTime)
                        value = dateTime.ToString("yyyy-MM-ddTHH:mm:ss");

                    request.AddQueryParameter(paramName, value.ToString());
                }
            }

            if (body != null)
                request.AddJsonBody(body);

            return (client, request);
        }

        private static string ToSnakeCase(string input) =>
            Regex.Replace(input, "([a-z0-9])([A-Z])", "$1_$2").ToLower();
    }
}