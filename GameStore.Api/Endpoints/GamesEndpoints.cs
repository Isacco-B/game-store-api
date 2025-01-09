using GameStore.Api.Data;
using GameStore.Api.Dtos;
using GameStore.Api.Entities;
using GameStore.Api.Mapping;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Endpoints;

public static class GamesEndpoints
{
    const string GetGameEndpointName = "GetGameById";

    public static RouteGroupBuilder MapGamesEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("games").WithParameterValidation();

        group.MapGet(
            "/",
            async (GameStoreContext dbContext) =>
                await dbContext
                    .Games.Include(game => game.Genre)
                    .Select(game => game.ToGameSummaryDto())
                    .AsNoTracking()
                    .ToListAsync()
        );

        // Get /games/{id}
        group
            .MapGet(
                "/{id}",
                async (int id, GameStoreContext dbContext) =>
                {
                    Game? game = await dbContext.Games.FindAsync(id);

                    if (game is null)
                        return Results.NotFound();

                    return Results.Ok(game.ToGameDetailsDto());
                }
            )
            .WithName(GetGameEndpointName);

        // Post /games
        group.MapPost(
            "/",
            async (CreateGameDto newGame, GameStoreContext dbContext) =>
            {
                Game game = newGame.ToEntity();

                await dbContext.Games.AddAsync(game);
                await dbContext.SaveChangesAsync();

                return Results.CreatedAtRoute(
                    GetGameEndpointName,
                    new { id = game.Id },
                    game.ToGameDetailsDto()
                );
            }
        );

        // Put /games/{id}
        group.MapPut(
            "/{id}",
            async (int id, UpdateGameDto updateGame, GameStoreContext dbContext) =>
            {
                var existingGame = await dbContext.Games.FindAsync(id);

                if (existingGame is null)
                    return Results.NotFound();

                dbContext.Entry(existingGame).CurrentValues.SetValues(updateGame.ToEntity(id));
                await dbContext.SaveChangesAsync();

                return Results.NoContent();
            }
        );

        // Delete /games/{id}
        group.MapDelete(
            "/{id}",
            async (int id, GameStoreContext dbContext) =>
            {
                await dbContext.Games.Where(game => game.Id == id).ExecuteDeleteAsync();

                return Results.NoContent();
            }
        );

        return group;
    }
}
