using System.Diagnostics;

namespace TicketWebApp.Telemetry;

public static class EthanSecondTraces
{
    public static readonly string Name = "GOONERS";
    public static readonly ActivitySource MyActivitySource = new(Name);
}