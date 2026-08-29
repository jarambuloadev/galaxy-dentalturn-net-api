using Galaxy.DentalTurn.Business.Interfaces;
using Galaxy.DentalTurn.Business.DTO.Request.Patient;
using Galaxy.DentalTurn.Business.DTO.Response;
using Microsoft.AspNetCore.Http;

namespace Galaxy.DentalTurn.API.Endpoints
{
    public static class PatientEndpoints
    {
        public static RouteGroupBuilder MapPatientEndpoints(this RouteGroupBuilder group)
        {
            group.MapPost("/", async (UpdatePatientRequest request, IPatientService patientService) =>
            {
                var result = await patientService.CreateAsync(request);
                return Results.Ok(result);
            })
            .WithName("CreatePatient")
            .Produces<UpdatePatientRequest>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest);

            group.MapGet("/{id:int}", async (int id, IPatientService patientService) =>
            {
                var patient = await patientService.GetByIdAsync(id);
                if (patient == null)
                {
                    return Results.NotFound();
                }
                return Results.Ok(patient);
            })
            .WithName("GetByIdPatient")
            .WithSummary("Get a patient by its ID")
            .Produces<GetPatientResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

            group.MapPut("/{id:int}", async (int id, UpdatePatientRequest request, IPatientService patientService) =>
            {
                var result = await patientService.UpdateAsync(id, request);
                return Results.Ok(result);
            })
            .WithName("UpdatePatient")
            .WithSummary("Update a patient by its ID")
            .Produces<UpdatePatientRequest>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);

            group.MapDelete("/{id:int}", async (int id, IPatientService patientService) =>
            {
                var service = await patientService.DeleteAsync(id);
                return Results.Ok(service);
            })
            .WithName("DeletePatient");

            group.MapGet("/", async (int pageNumber, int pageSize, IPatientService patientService) =>
            {
                var patients = await patientService.ListAsync(pageNumber, pageSize);
                if (patients == null)
                {
                    return Results.NotFound();
                }
                return Results.Ok(patients);
            })
            .WithName("GetListPatient")
            .WithSummary("Get a List of Patients")
            .Produces<ListPatientsResponse>(StatusCodes.Status200OK);

            return group;
        }
    }
}
