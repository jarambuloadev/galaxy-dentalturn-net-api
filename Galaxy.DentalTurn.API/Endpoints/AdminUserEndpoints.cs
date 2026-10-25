using Galaxy.DentalTurn.Business.DTO.Request;
using Galaxy.DentalTurn.Business.DTO.Request.AdminUser;
using Galaxy.DentalTurn.Business.DTO.Response;
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
                return Results.Ok(result);
            })
            .WithName("CreateAdminUser")
            .WithSummary("Creates a new admin user.")
            .Produces<Result>(StatusCodes.Status200OK)
            .Produces<Result>(StatusCodes.Status400BadRequest);

            group.MapGet("/{id:int}", async (int id, IAdminUserService adminUserService) =>
            {
                var result = await adminUserService.GetByIdAsync(id);
                if (result == null)
                {
                    return Results.NotFound();
                }
                return Results.Ok(result);
            })
            .WithName("GetByIdAdminUser")
            .WithSummary("Get an admin user by its ID")
            .Produces<GetAdminUserResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

            group.MapPut("/{id:int}", async (int id, UpdateAdminUserRequest request, IAdminUserService adminUserService) =>
            {
                var result = await adminUserService.UpdateAsync(id, request);
                return Results.Ok(result);
            })
            .WithName("UpdateAdminUser")
            .WithSummary("Update an admin user by its ID")
            .Produces<Result>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);

            group.MapGet("/", async ([AsParameters] PagedRequest request, IAdminUserService adminUserService) =>
            {
                var result = await adminUserService.ListAsync(request);
                if (result == null)
                {
                    return Results.NotFound();
                }
                return Results.Ok(result);
            })
            .WithName("GetListAdminUser")
            .WithSummary("Get a list of admin users")
            .Produces<ListAdminUsersResponse>(StatusCodes.Status200OK);

            group.MapDelete("/{id:int}", async (int id, IAdminUserService adminUserService) =>
            {
                var result = await adminUserService.DeleteAsync(id);
                return Results.Ok(result);
            })
            .WithName("DeleteAdminUser")
            .WithSummary("Delete an admin user by its ID")
            .Produces<Result>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);

            return group;
        }
    }
}
