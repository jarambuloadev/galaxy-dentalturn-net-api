using Galaxy.DentalTurn.Business.DTO.Request.Appointments;
using Galaxy.DentalTurn.Business.DTO.Response;
using Galaxy.DentalTurn.Business.Interfaces;
using Galaxy.DentalTurn.Common.Helpers;

namespace Galaxy.DentalTurn.API.Endpoints
{
    public static class AppointmentEndpoints
    {
        public static RouteGroupBuilder MapAppointmentEndpoints(this RouteGroupBuilder group)
        {
            group.MapGet("/availability", async (
                string date,
                int serviceId,
                int practitionerId,
                IAppointmentService service) =>
            {
                var result = await service.GetAvailableSlotsAsync(date, serviceId, practitionerId);
                return Results.Ok(result);
            })
            .WithName("GetAvailableSlots")
            .WithSummary("Obtiene los horarios disponibles para una fecha, servicio y odontólogo")
            .Produces<Result<List<AvailableSlotResponse>>>(StatusCodes.Status200OK);

            group.MapPost("/", async (CreateAppointmentRequest request, IAppointmentService service) =>
            {
                var result = await service.CreateAsync(request);
                if (result.IsFailure)
                {
                    return Results.BadRequest(result);
                }
                return Results.Ok(result);
            })
            .WithName("CreateAppointment")
            .WithSummary("Crea un nuevo turno odontológico")
            .Produces<Result<AppointmentResponse>>(StatusCodes.Status200OK)
            .Produces<Result<AppointmentResponse>>(StatusCodes.Status400BadRequest);

            group.MapGet("/{id:int}", async (int id, IAppointmentService service) =>
            {
                var result = await service.GetByIdAsync(id);
                if (result.IsFailure)
                {
                    return Results.NotFound(result);
                }
                return Results.Ok(result);
            })
            .WithName("GetAppointmentById")
            .WithSummary("Obtiene un turno por su ID")
            .Produces<Result<AppointmentResponse>>(StatusCodes.Status200OK)
            .Produces<Result<AppointmentResponse>>(StatusCodes.Status404NotFound);

            group.MapGet("/", async ([AsParameters] AppointmentFilterRequest filter, IAppointmentService service) =>
            {
                var result = await service.ListAsync(filter);
                return Results.Ok(result);
            })
            .WithName("ListAppointments")
            .WithSummary("Lista los turnos aplicando filtros opcionales")
            .Produces<Result<List<AppointmentResponse>>>(StatusCodes.Status200OK);

            group.MapPut("/{id:int}/status", async (int id, UpdateAppointmentStatusRequest request, IAppointmentService service) =>
            {
                var result = await service.UpdateStatusAsync(id, request.Status);
                if (result.IsFailure)
                {
                    return Results.BadRequest(result);
                }
                return Results.Ok(result);
            })
            .WithName("UpdateAppointmentStatus")
            .WithSummary("Actualiza el estado de un turno")
            .Produces<Result>(StatusCodes.Status200OK)
            .Produces<Result>(StatusCodes.Status400BadRequest);

            return group;
        }
    }
}
