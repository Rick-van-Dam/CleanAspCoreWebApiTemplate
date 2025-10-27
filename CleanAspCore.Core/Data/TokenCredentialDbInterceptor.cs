using System.Data.Common;
using CleanAspCore.Core.Common.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace CleanAspCore.Core.Data;

internal sealed class TokenCredentialDbInterceptor(ITokenProvider tokenProvider) : DbConnectionInterceptor
{
    public override async ValueTask<InterceptionResult> ConnectionOpeningAsync(
        DbConnection connection,
        ConnectionEventData eventData,
        InterceptionResult result,
        CancellationToken cancellationToken = default)
    {
        var sqlConnection = (SqlConnection)connection;

        var connectionStringBuilder = new SqlConnectionStringBuilder(sqlConnection.ConnectionString);
        if (connectionStringBuilder.DataSource.Contains("database.windows.net", StringComparison.OrdinalIgnoreCase) && string.IsNullOrEmpty(connectionStringBuilder.UserID))
        {
            sqlConnection.AccessToken = (await tokenProvider.GetAccessTokenAsync("https://database.windows.net//.default", cancellationToken)).Token;
        }

        return await base.ConnectionOpeningAsync(connection, eventData, result, cancellationToken);
    }

    public override InterceptionResult ConnectionOpening(DbConnection connection, ConnectionEventData eventData, InterceptionResult result)
    {
        var sqlConnection = (SqlConnection)connection;

        var connectionStringBuilder = new SqlConnectionStringBuilder(sqlConnection.ConnectionString);
        if (connectionStringBuilder.DataSource.Contains("database.windows.net", StringComparison.OrdinalIgnoreCase) && string.IsNullOrEmpty(connectionStringBuilder.UserID))
        {
            sqlConnection.AccessToken = tokenProvider.GetAccessToken("https://database.windows.net//.default").Token;
        }

        return base.ConnectionOpening(connection, eventData, result);
    }
}
