using System.Diagnostics.Metrics;
using System.Reflection.Metadata.Ecma335;

namespace TicketWebApp.Telemetry;

public static class EthanMetrics
{
    public static int EventsCalled = 0;
    public static int TotalTicketsScanned = 0;
    public static int TotalUsersActive = 0;
    public static readonly string Name = "EthanMetrics";
    public static Meter Meter = new(Name, "1.0.0");
    public static Counter<int> hitCount = Meter.CreateCounter<int>("hit_count");
    public static UpDownCounter<int> ActiveUsers = Meter.CreateUpDownCounter<int>("active_users");
    public static ObservableCounter<int> totalEventsGrabbed = Meter.CreateObservableCounter<int>("events_seen_total", () => EventsCalled);
    public static ObservableUpDownCounter<int> currentTicketsScanned = Meter.CreateObservableUpDownCounter<int>("Total tickets scanned", () => TotalTicketsScanned);
    public static ObservableGauge<int> currentActiveUsers = Meter.CreateObservableGauge<int>("Total Active Users", () => TotalUsersActive);
    public static Histogram<TimeSpan> totalEmailTime = Meter.CreateHistogram<TimeSpan>("Time it takes an email to send");
}
