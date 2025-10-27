using System.Diagnostics;
using OpenTelemetry;
using OpenTelemetry.Logs;

namespace CleanAspCore.Core.Common.Telemetry;

internal sealed class EnrichLogsProcessor : BaseProcessor<LogRecord>
{
    public override void OnEnd(LogRecord data)
    {
        if (Activity.Current is null || !Activity.Current.Baggage.Any())
            return;

        var updatedAttributes = data.Attributes?.ToList() ?? [];
        foreach (var baggage in Activity.Current.Baggage)
        {
            if (baggage.Value == null)
                continue;
            updatedAttributes.Add(new KeyValuePair<string, object?>(baggage.Key, baggage.Value));
        }

        data.Attributes = updatedAttributes;
    }
}
