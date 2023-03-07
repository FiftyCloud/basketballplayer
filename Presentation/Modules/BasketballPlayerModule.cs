using Carter;
using Models;
using DbContexts;
using Microsoft.EntityFrameworkCore;
using Bogus;

namespace Modules;


public class BasketballPlayerModule : CarterModule
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
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

        basketballPlayers.MapGet("/FakeBasketBallPlayers", () =>
        {
            var players = new Faker<BasketballPlayer>()
                .RuleFor(p => p.Name, f => f.Name.FullName())
                .RuleFor(p => p.Age, f => f.Random.Int(18, 40))
                .RuleFor(p => p.Height, f => f.Random.Float(1.5f, 2.2f))
                .RuleFor(p => p.Weight, f => f.Random.Float(50f, 120f));

            return players.Generate(10);
        });

    }
}