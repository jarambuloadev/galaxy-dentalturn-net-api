using Galaxy.DentalTurn.Business.DTO.Request;
using Galaxy.DentalTurn.Business.DTO.Request.Practitioner;
using Galaxy.DentalTurn.Business.DTO.Response;
using Galaxy.DentalTurn.Business.Interfaces;

namespace Galaxy.DentalTurn.API.Endpoints
{
    public static class PractitionerEndpoints
    {
        public static RouteGroupBuilder MapPractitionerEndpoints(this RouteGroupBuilder group)
        {
            group.MapPost("/", async (UpdatePractitionerRequest request, IPractitionerService service) =>
            {
                var result = await service.CreateAsync(request);
                return Results.Ok(result);
            })
            .WithName("CreatePractitioner")
            .WithSummary("Create a new practitioner")
            .Produces<UpdatePractitionerRequest>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest);

            group.MapGet("/{id:int}", async (int id, IPractitionerService service) =>
            {
                var result = await service.GetByIdAsync(id);
                if (result == null)
                {
                    return Results.NotFound();
                }
                return Results.Ok(result);
            })
            .WithName("GetByIdPractitioner")
            .WithSummary("Get a practitioner by its ID")
            .Produces<GetPractitionerResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

            group.MapPut("/{id:int}", async (int id, UpdatePractitionerRequest request, IPractitionerService service) =>
            {
                var result = await service.UpdateAsync(id, request);
                return Results.Ok(result);
            })
            .WithName("UpdatePractitioner")
            .WithSummary("Update a practitioner by its ID")
            .Produces<UpdatePractitionerRequest>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);

            group.MapGet("/", async ([AsParameters] PagedRequest request, IPractitionerService service) =>
            {
                var result = await service.ListAsync(request);
                if (result == null)
                {
                    return Results.NotFound();
                }
                return Results.Ok(result);
            })
            .WithName("GetListPractitioner")
            .WithSummary("Get a List of Practitioners")
            .Produces<ListPractitionersResponse>(StatusCodes.Status200OK);

            group.MapDelete("/{id:int}", async (int id, IPractitionerService service) =>
            {
                var result = await service.DeleteAsync(id);
                return Results.Ok(result);
            })
            .WithName("DeletePractitioner")
            .WithSummary("Delete a practitioner by its ID")
            .Produces<GetPractitionerResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);

            return group;
        }
    }
}
