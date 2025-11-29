using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Identity.Web;
using Microsoft.OpenApi;

namespace CleanAspCore.Core.Common.OpenApi;

internal sealed class SecuritySchemeTransformer(IConfiguration configuration, IWebHostEnvironment environment) : IOpenApiDocumentTransformer
{
    public Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
    {
        var config = configuration.GetRequiredSection(Constants.AzureAd).Get<MicrosoftIdentityOptions>()!;

        var requirements = new Dictionary<string, IOpenApiSecurityScheme>
        {
            ["AzureAd"] = new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.OAuth2,
                Scheme = "bearer", // "bearer" refers to the header name here
                In = ParameterLocation.Header,
                BearerFormat = "Json Web Token",
                Flows = new OpenApiOAuthFlows
                {
                    AuthorizationCode = new OpenApiOAuthFlow
                    {
                        AuthorizationUrl = new Uri($"https://login.microsoftonline.com/{config.TenantId}/oauth2/v2.0/authorize"),
                        TokenUrl = new Uri($"https://login.microsoftonline.com/{config.TenantId}/oauth2/v2.0/token"),
                        Scopes = new Dictionary<string, string>
                        {
                            { $"api://{config.ClientId}/default", "read" },
                        },
                        Extensions = new Dictionary<string, IOpenApiExtension> { ["x-usePkce"] = new JsonNodeExtension("SHA-256") },
                    }
                }
            }
        };

        if (environment.IsDevelopment())
        {
            requirements.Add(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                Scheme = "bearer", // "bearer" refers to the header name here
                In = ParameterLocation.Header,
                BearerFormat = "Json Web Token"
            });
        }

        document.Components!.SecuritySchemes = requirements;

        return Task.CompletedTask;
    }
}
