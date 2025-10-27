using Azure.Core;
using Microsoft.Extensions.Caching.Memory;

namespace CleanAspCore.Core.Common.Identity;

public class CachedTokenProvider(TokenCredential credential, IMemoryCache cache) : ITokenProvider
{
    public async Task<AccessToken> GetAccessTokenAsync(string scope, CancellationToken cancellationToken = default)
    {
        return await cache.GetOrCreateAsync(scope, async cacheEntry =>
        {
            var tokenRequestContext = new TokenRequestContext([scope]);
            var token = await credential.GetTokenAsync(tokenRequestContext, cancellationToken);

            cacheEntry.SetAbsoluteExpiration(token.ExpiresOn.AddMinutes(-10));

            return token;
        });
    }

    public AccessToken GetAccessToken(string scope)
    {
        return cache.GetOrCreate(scope, cacheEntry =>
        {
            var tokenRequestContext = new TokenRequestContext([scope]);
            var token = credential.GetToken(tokenRequestContext, CancellationToken.None);

            cacheEntry.SetAbsoluteExpiration(token.ExpiresOn.AddMinutes(-10));

            return token;
        });
    }
}
