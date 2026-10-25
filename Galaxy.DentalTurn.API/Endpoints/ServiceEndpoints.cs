using Galaxy.DentalTurn.Business.Interfaces;
using Galaxy.DentalTurn.Business.DTO.Request.Service;
using Galaxy.DentalTurn.Business.DTO.Request;
using Galaxy.DentalTurn.Business.DTO.Response;

namespace Galaxy.DentalTurn.API.Endpoints
{
    public static class ServiceEndpoints
    {
        public static async Task<RouteGroupBuilder> MapServiceEndpoints(this RouteGroupBuilder group)
        {
            group.MapPost("/", async (UpdateServiceRequest request, IServiceService iserviceService) =>
               {
                   var result = await iserviceService.CreateAsync(request);
                   return Results.Ok(result);
               })
                .WithName("CreateService")
            .Produces<UpdateServiceRequest>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest);


            group.MapGet("/{id:int}", async (int id, IServiceService iserviceService) =>
            {
                var service = await iserviceService.GetByIdAsync(id);
                if(service == null)
                  {
                    return Results.NotFound();
                }
                return Results.Ok(service);
            })
                 .WithName("GetByIdService")
                 .WithSummary("Get a service by its ID")
            .Produces<GetServiceResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);


            group.MapPut("/{id:int}", async (int id, UpdateServiceRequest request, IServiceService iserviceService) =>
            {
               var result =  await iserviceService.UpdateAsync(id, request);
                return Results.Ok(result);
            })
                .WithName("UpdateService")
                .WithSummary("Update a service by its ID")
            .Produces<UpdateServiceRequest>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);

            group.MapGet("/", async ( [AsParameters] PagedRequest request, IServiceService iserviceService) =>
            {
                var service = await iserviceService.ListAsync(request);
                if (service == null)
                {
                    return Results.NotFound();
                }
                return Results.Ok(service);
            })
                .WithName("GetListService")
                .WithSummary("Get a List of Services")
           .Produces<GetServiceResponse>(StatusCodes.Status200OK);

            group.MapDelete("/{id:int}", async (int id, IServiceService iserviceService) =>
            {
                var result = await iserviceService.DeleteAsync(id);
                return Results.Ok(result);
            })
                .WithName("DeleteService")
                .WithSummary("Delete a service by its ID")                
            .Produces<GetServiceResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);


            return group;
        }


    }
}
