
using Microsoft.EntityFrameworkCore;
using DbContexts;
using Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<BasketPlayersTeamsDb>(opt => opt.UseInMemoryDatabase("BasketballPlayerList"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();

var basketballPlayers = app.MapGroup("/BasketballPlayers");

basketballPlayers.MapGet("/BasketballPlayers", async (BasketPlayersTeamsDb db) =>
    await db.Players.ToListAsync());


basketballPlayers.MapGet("/BasketballPlayer/{id}", async (int id, BasketPlayersTeamsDb db) =>
    await db.Players.FindAsync(id)
        is BasketballPlayer player
            ? Results.Ok(player)
            : Results.NotFound());

basketballPlayers.MapPost("/BasketballPlayer", async (BasketballPlayer player, BasketPlayersTeamsDb db) =>
{
    db.Players.Add(player);
    await db.SaveChangesAsync();

    return Results.Created($"/BasketballPlayer/{player.Id}", player);
});

basketballPlayers.MapPut("/BasketballPlayeritems/{id}", async (int id, BasketballPlayer inputBasketballPlayer, BasketPlayersTeamsDb db) =>
{
    var BasketballPlayer = await db.Players.FindAsync(id);

    if (BasketballPlayer is null) return Results.NotFound();

    BasketballPlayer.Name = inputBasketballPlayer.Name;
    BasketballPlayer.Position = inputBasketballPlayer.Position;

    await db.SaveChangesAsync();

    return Results.NoContent();
});

basketballPlayers.MapDelete("/BasketballPlayer/{id}", async (int id, BasketPlayersTeamsDb db) =>
{
    if (await db.Players.FindAsync(id) is BasketballPlayer BasketballPlayer)
    {
        db.Players.Remove(BasketballPlayer);
        await db.SaveChangesAsync();
        return Results.Ok(BasketballPlayer);
    }

    return Results.NotFound();
});

app.UseSwagger();
app.UseSwaggerUI();

app.Run();