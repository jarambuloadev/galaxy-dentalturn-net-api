

using Galaxy.DentalTurn.Business.DTO.Request.AdminUser;
using Galaxy.DentalTurn.Business.Interfaces;
using Galaxy.DentalTurn.Common.Helpers;

namespace Galaxy.DentalTurn.API.Endpoints
{
    public static class AdminUserEndpoints
    {
        public static RouteGroupBuilder MapAdminUserEndpoints(this RouteGroupBuilder group)
        {
            group.MapPost("/", async (CreateAdminUserRequest request, IAdminUserService adminUserService) =>
            {
                var result = await adminUserService.RegisterAsync(request);
                return Results.Created("/api/adminusers", result);
            })
            .WithName("CreateAdminUser")
            .WithSummary("Creates a new admin user.")
            .Produces<Result>(StatusCodes.Status201Created)
            .Produces<Result>(StatusCodes.Status400BadRequest);

            return group;
        }
    }
}
