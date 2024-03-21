using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;
using TicketClassLib.Data;
using TicketClassLib.Services;
using TicketWebApp.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;
using TicketWebApp.Telemetry;

namespace TicketWebApp.Services;

public partial class ApiEventService(IDbContextFactory<PostgresContext> dbFactory, ILogger<ApiEventService> logger) : IEventService
{

    [LoggerMessage(Level = LogLevel.Information, Message = "Event Service: {Description}")]
    static partial void LogInformationMessage(ILogger logger, string description);


    public async Task<Event> AddEvent(string name, DateTime date)
    {
        LogInformationMessage(logger, $"Adding an event named {name}");
        using var context = await dbFactory.CreateDbContextAsync();
        Event newEvent = new Event()
        {
            Id = 0,
            Name = name,
            Eventdate = date
        };

        var value = await context.Events.AddAsync(newEvent);
        await context.SaveChangesAsync();

        return newEvent;
    }

    public async Task<Event> AddEvent(Event newEvent)
    {
        using var context = await dbFactory.CreateDbContextAsync();

        var value = await context.Events.AddAsync(newEvent);
        await context.SaveChangesAsync();

        return newEvent;
    }

    public async Task<List<Event>> GetAll()
    {
        LogInformationMessage(logger, "Getting all the events");
        using var currentTrace = EthanTraces.MyActivitySource.StartActivity("thingy");
        using var context = await dbFactory.CreateDbContextAsync();

        currentTrace?.AddEvent(new("Getting all events"));
        return await context.Events
            .Include(e => e.Tickets)
            .ToListAsync();
    }

    public async Task<Event?> GetEvent(int id)
    {
        using var context = await dbFactory.CreateDbContextAsync();

        return await context.Events
            .Include(e => e.Tickets)
            .Where(e => e.Id == id)
            .FirstOrDefaultAsync();
    }
}
