using Galaxy.DentalTurn.Business.DTO.Request.BusinessHours;
using Galaxy.DentalTurn.Business.DTO.Response;
using Galaxy.DentalTurn.Business.Interfaces;
using Galaxy.DentalTurn.Common.Helpers;

namespace Galaxy.DentalTurn.API.Endpoints
{
    public static class BusinessHoursEndpoints
    {
        public static RouteGroupBuilder MapBusinessHoursEndpoints(this RouteGroupBuilder group)
        {
            group.MapGet("/", async (IBusinessHoursService service) =>
            {
                var result = await service.ListAsync();
                return Results.Ok(result);
            })
            .WithName("GetBusinessHours")
            .WithSummary("Obtiene los horarios de atención por día de la semana")
            .Produces<Result<List<BusinessHoursResponse>>>(StatusCodes.Status200OK);

            group.MapPut("/", async (UpdateBusinessHoursRequest request, IBusinessHoursService service) =>
            {
                var result = await service.UpdateAllAsync(request);
                if (result.IsFailure)
                {
                    return Results.BadRequest(result);
                }
                return Results.Ok(result);
            })
            .WithName("UpdateBusinessHours")
            .WithSummary("Actualiza los horarios de atención de los 7 días de la semana")
            .Produces<Result>(StatusCodes.Status200OK)
            .Produces<Result>(StatusCodes.Status400BadRequest);

            return group;
        }
    }
}
