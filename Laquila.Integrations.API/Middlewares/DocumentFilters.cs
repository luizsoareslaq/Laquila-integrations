using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Resources;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

public class RoleBasedDocumentFilter : IDocumentFilter
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public RoleBasedDocumentFilter(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        var httpContext = _httpContextAccessor.HttpContext;

        if (httpContext == null)
            return;

        var token = httpContext.Request.Headers["Authorization"].FirstOrDefault()?.Replace("Bearer ", "");
        if (string.IsNullOrEmpty(token))
            return;

        var handler = new JwtSecurityTokenHandler();
        JwtSecurityToken jwt;


        try
        {
            jwt = handler.ReadJwtToken(token);
        }
        catch
        {
            swaggerDoc.Paths.Clear();
            swaggerDoc.Components.Schemas.Clear();
            swaggerDoc.Tags?.Clear();             
            return; // token inválido
        }

        var userRoles = jwt.Claims
            .Where(c => c.Type == "role" || c.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role")
            .Select(c => c.Value.ToLower())
            .ToList();

        if (userRoles.Contains("admin"))
            return;

        var allowedPaths = new HashSet<string>();

        foreach (var desc in context.ApiDescriptions)
        {
            var path = "/" + (!string.IsNullOrEmpty(desc.RelativePath) && desc is not null ? desc.RelativePath.TrimEnd('/') : string.Empty);
            var method = desc.HttpMethod?.ToLowerInvariant();

            if (string.IsNullOrWhiteSpace(path) || string.IsNullOrWhiteSpace(method))
                continue;

            var actionMetadata = desc.ActionDescriptor?.EndpointMetadata;

            // Verifica se tem Authorize
            var authorizeAttr = actionMetadata?.OfType<AuthorizeAttribute>().FirstOrDefault();

            if (authorizeAttr == null || string.IsNullOrEmpty(authorizeAttr.Roles))
            {
                allowedPaths.Add(path); // endpoint público
                continue;
            }

            var requiredRoles = authorizeAttr.Roles
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(r => r.Trim().ToLower());

            if (userRoles.Any(r => requiredRoles.Contains(r)))
            {
                allowedPaths.Add(path); 
            }
        }

        var keysToRemove = swaggerDoc.Paths
            .Where(p => !allowedPaths.Contains(p.Key))
            .Select(p => p.Key)
            .ToList();

        foreach (var key in keysToRemove)
        {
            swaggerDoc.Paths.Remove(key);
        }

    }
}
