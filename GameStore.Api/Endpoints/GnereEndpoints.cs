using GameStore.Api.Data;
using GameStore.Api.Mapping;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Endpoints;

public static class GnereEndpoints
{
    public static RouteGroupBuilder MapGenreEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("genre").WithParameterValidation();

        group.MapGet(
            "/",
            async (GameStoreContext dbContext) =>
                await dbContext.Genres.Select(genre => genre.ToDto()).AsNoTracking().ToListAsync()
        );

        return group;
    }
}
