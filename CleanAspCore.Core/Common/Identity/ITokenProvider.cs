using Azure.Core;

namespace CleanAspCore.Core.Common.Identity;

public interface ITokenProvider
{
    Task<AccessToken> GetAccessTokenAsync(string scope, CancellationToken cancellationToken = default);
    AccessToken GetAccessToken(string scope);
}
