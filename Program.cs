using voting_api.Domain;
using Microsoft.EntityFrameworkCore;
using voting_api.Data;

var builder = WebApplication.CreateBuilder(args);

// Add console logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Add this to ensure proper container URLs
builder.WebHost.UseUrls("http://*:80");

// Add user secrets for local development
builder.Configuration.AddUserSecrets<Program>();
Environment.SetEnvironmentVariable("HTTP_HOST", builder.Configuration["host"]);
Environment.SetEnvironmentVariable("HTTP_TOKEN", builder.Configuration["token"]);

// Configure database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

var app = builder.Build();

// Log all incoming requests (optional but useful)
app.Use(async (context, next) =>
{
    Console.WriteLine($"Request: {context.Request.Method} {context.Request.Path}");
    await next();
});

// Add endpoint logging
app.MapPost("/polls", async (CreatePollDto dto, AppDbContext db) =>
{
    Console.WriteLine("Creating new poll...");
    var poll = new Poll(
        dto.Name,
        dto.Description,
        dto.StartDate,
        dto.EndDate,
        dto.IsAnonymous);

    foreach (var optionDto in dto.Options)
    {
        Console.WriteLine($"Adding option: {optionDto.Name}");
        poll.AddOption(new Option(optionDto.Name));
    }

    db.Polls.Add(poll);
    await db.SaveChangesAsync();
    Console.WriteLine($"Created poll with ID: {poll.Id}");

    return Results.Created($"/polls/{poll.Id}", poll);
}).RequireAuthorization();

app.MapGet("/polls/{pollId}", async (Guid pollId, AppDbContext db) =>
{
    Console.WriteLine($"Fetching poll with ID: {pollId}");
    var poll = await db.Polls
        .Include(p => p.Options)
        .FirstOrDefaultAsync(p => p.Id == pollId);

    return poll is null ? Results.NotFound() : Results.Ok(poll);
}).RequireAuthorization();

app.MapGet("/polls", async (AppDbContext db) =>
{
    Console.WriteLine("Fetching all polls");
    return await db.Polls.ToListAsync();
}).RequireAuthorization();

app.MapPost("/polls/{pollId}/votes", async (
    Guid pollId,
    VoteDto dto,
    AppDbContext db) =>
{
    Console.WriteLine($"Processing vote for poll {pollId}, option {dto.OptionId}");
    var poll = await db.Polls.FindAsync(pollId);
    if (poll is null) return Results.NotFound("Poll not found");

    var option = await db.Options.FindAsync(dto.OptionId);
    if (option is null) return Results.NotFound("Option not found");

    if (DateTime.UtcNow < poll.StartDate || DateTime.UtcNow > poll.EndDate)
        return Results.BadRequest("Voting period is closed");

    var vote = new Vote
    {
        PollId = pollId,
        OptionId = dto.OptionId,
        SubmittedAt = DateTime.UtcNow
    };
    option.VoteCount++;
    db.Votes.Add(vote);
    await db.SaveChangesAsync();
    Console.WriteLine($"Vote recorded for option {dto.OptionId}");

    return Results.Ok();
}).RequireAuthorization();

// Add startup message
app.Lifetime.ApplicationStarted.Register(() =>
{
    Console.WriteLine("Application started!");
    Console.WriteLine("Available endpoints:");
    Console.WriteLine("POST   /polls");
    Console.WriteLine("GET    /polls");
    Console.WriteLine("GET    /polls/{pollId}");
    Console.WriteLine("POST   /polls/{pollId}/votes");
});

app.Run();