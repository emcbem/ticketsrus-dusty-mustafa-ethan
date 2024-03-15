using System.Diagnostics.Metrics;

namespace TicketWebApp.Telemetry;

public static class EthanMetrics
{
    public static readonly string Name = "EthanMetrics";
    public static Meter Meter = new(Name, "1.0.0");
    public static Counter<int> hitCount = Meter.CreateCounter<int>("hit_count");
}