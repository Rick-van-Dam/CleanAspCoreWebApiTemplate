using Azure.Core;
using Azure.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CleanAspCore.Core.Common.Identity;

public static class HostBuilderExtensions
{
    public static void AddIdentityServices(this IHostApplicationBuilder builder)
    {
        builder.Services.AddSingleton<TokenCredential>(_ => new ChainedTokenCredential(new AzureCliCredential(), new ManagedIdentityCredential()));
        builder.Services.AddSingleton<ITokenProvider, CachedTokenProvider>();
    }
}
