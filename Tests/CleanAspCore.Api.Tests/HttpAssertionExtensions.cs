namespace CleanAspCore.Api.Tests;

internal static class HttpAssertionExtensions
{
    public static Guid GetGuidFromLocationHeader(this HttpResponseMessage response)
    {
        var segments = response.Headers.Location!.Segments;
        return Guid.Parse(segments.Last());
    }
}
