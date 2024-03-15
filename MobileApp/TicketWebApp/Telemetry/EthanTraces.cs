using System.Diagnostics;

namespace TicketWebApp.Telemetry;

public static class EthanTraces
{
    public static readonly string Name = "thingy";
    public static readonly ActivitySource MyActivitySource = new(Name);
}