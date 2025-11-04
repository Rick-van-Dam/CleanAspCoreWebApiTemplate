using System.Net.Http.Json;
using Microsoft.AspNetCore.Http;

namespace CleanAspCore.TestUtils.Assertions;

public static class HttpAssertionExtensions
{
    public static async Task AssertStatusCode(this HttpResponseMessage response, HttpStatusCode expected)
    {
        if (expected != HttpStatusCode.BadRequest)
        {
            using AssertionScope _ = new();
            response.StatusCode.Should().Be(expected);
            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                var problemDetails = await response.Content.ReadFromJsonAsync<HttpValidationProblemDetails>();
                if (problemDetails is not null)
                {
                    var message = string.Join(Environment.NewLine, problemDetails.Errors.Select(x => $"{x.Key}: {x.Value[0]}"));
                    _.AddPreFormattedFailure(message);
                }
            }
        }
        else
        {
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }

    public static async Task AssertBadRequest(this HttpResponseMessage response, params string[] expectedErrors)
    {
        using AssertionScope _ = new();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var problemDetails = await response.Content.ReadFromJsonAsync<HttpValidationProblemDetails>();
        problemDetails.Should().NotBeNull();
        problemDetails.Errors.Select(x => x.Key).Should().BeEquivalentTo(expectedErrors);
    }

    public static async Task AssertJsonBodyIsEquivalentTo<T>(this HttpResponseMessage response, T expected)
    {
        using AssertionScope _ = new();
        response.Content.Headers.ContentType.Should().NotBeNull();
        response.Content.Headers.ContentType.MediaType.Should().Be("application/json");
        response.Content.Headers.ContentType.CharSet.Should().Be("utf-8");

        T? body = await response.Content.ReadFromJsonAsync<T>();
        body.Should().BeEquivalentTo(expected);
    }
}
