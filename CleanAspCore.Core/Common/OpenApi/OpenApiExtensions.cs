﻿using System.Text.Json.Serialization;
using MartinCostello.OpenApi;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Web;
using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;

namespace CleanAspCore.Core.Common.OpenApi;

public static class OpenApiExtensions
{
    public static void AddOpenApiServices<TProgram>(this WebApplicationBuilder builder, JsonSerializerContext jsonSerializerContext)
    {
        builder.Services.AddOpenApi(options =>
        {
            options.AddDocumentTransformer<SecuritySchemeTransformer>();
            options.AddDocumentTransformer((document, context, ct) =>
            {
                document.Info.Description = "This is a template repository showing how one can implement a clean api with ASP.NET using minimal apis";
                document.Info.License = new OpenApiLicense { Name = "MIT" };
                return Task.CompletedTask;
            });
        });
        builder.Services.AddOpenApiExtensions((options) =>
        {
            options.AddXmlComments<TProgram>();
            options.AddExamples = true;
            options.AddServerUrls = true;
            options.SerializationContexts.Add(jsonSerializerContext);
        });
    }

    public static void UseOpenApi(this WebApplication app)
    {
        var config = app.Configuration.GetRequiredSection(Constants.AzureAd).Get<MicrosoftIdentityOptions>()!;

        app.MapOpenApi()
            .AllowAnonymous()
            .CacheOutput();

        app.MapScalarApiReference(options =>
        {
            options.WithPreferredScheme("AzureAd");
            options.WithOAuth2Authentication(oauth2Options =>
            {
                oauth2Options.ClientId = config.ClientId;
                oauth2Options.Scopes = [$"api://{config.ClientId}/default"];
            });

            var devToken = app.Configuration.GetValue<string>("DevToken");
            if (!string.IsNullOrEmpty(devToken))
            {
                options.WithHttpBearerAuthentication(httpBearerOptions =>
                {
                    httpBearerOptions.Token = devToken;
                });
            }
        }).AllowAnonymous();
    }
}
