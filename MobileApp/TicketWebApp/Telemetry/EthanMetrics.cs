using System.Diagnostics.Metrics;
using System.Reflection.Metadata.Ecma335;

namespace TicketWebApp.Telemetry;

public static class EthanMetrics
{
    public static int EventsCalled = 0;
    public static int _totalTicketsUnscanned = 0;
    public static int TotalUsersActive = 0;
    public static readonly string Name = "EthanMetrics";
    public static Meter Meter = new(Name, "1.0.0");
    public static Counter<int> hitCount = Meter.CreateCounter<int>("hit_count");
    public static UpDownCounter<int> WheneverIFeelLikeIt = Meter.CreateUpDownCounter<int>("who_knows");
    public static ObservableCounter<int> totalEventsGrabbed = Meter.CreateObservableCounter<int>("events_seen_total", () => EventsCalled);
    public static ObservableUpDownCounter<int> TotalTicketsUnscanned = Meter.CreateObservableUpDownCounter<int>("total_ticket", () => _totalTicketsUnscanned);
    public static ObservableGauge<int> currentSecondOfTheDay = Meter.CreateObservableGauge<int>("seconds_of_day", () => DateTime.Now.Second);
    public static Histogram<double> totalEmailTime = Meter.CreateHistogram<double>("email_time");

    static EthanMetrics()
    {

    }
}
