using Galaxy.DentalTurn.Business.DTO.Request;
using Galaxy.DentalTurn.Business.DTO.Request.Practitioner;
using Galaxy.DentalTurn.Business.DTO.Response;
using Galaxy.DentalTurn.Business.Interfaces;
using Galaxy.DentalTurn.Common.Helpers;
using Galaxy.DentalTurn.DataAccess.Entities;
using Galaxy.DentalTurn.Repositories.Interfaces;

namespace Galaxy.DentalTurn.Business.Implementations
{
    public class PractitionerService : IPractitionerService
    {
        private readonly IPractitionerRepository _practitionerRepository;

        public PractitionerService(IPractitionerRepository practitionerRepository)
        {
            _practitionerRepository = practitionerRepository;
        }

        public async Task<Result> CreateAsync(UpdatePractitionerRequest request)
        {
            var practitioner = new Practitioner
            {
                Name = request.Name,
                Active = request.Active,
                Status = true,
                CreatedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified),
                UpdatedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified)
            };

            var created = await _practitionerRepository.CreateAsync(practitioner);
            if (request.ServiceIds is not null)
            {
                await _practitionerRepository.UpdateServicesAsync(created.Id, request.ServiceIds);
            }

            return Result.Success("Odontólogo creado correctamente.");
        }

        public async Task<Result<GetPractitionerResponse>> GetByIdAsync(int id)
        {
            var p = await _practitionerRepository.GetByIdWithServicesAsync(id);
            if (p is null)
            {
                return Result.Failure<GetPractitionerResponse>($"Practitioner with id {id} not found.");
            }

            var activeServices = p.Services
                .Where(ps => ps.Service != null && ps.Service.Status)
                .Select(ps => new PractitionerServiceItemResponse
                {
                    Id = ps.Service.Id,
                    Name = ps.Service.Name
                })
                .ToList();

            var response = new GetPractitionerResponse
            {
                Id = p.Id,
                Name = p.Name,
                Active = p.Active,
                ServiceIds = activeServices.Select(s => s.Id).ToList(),
                Services = activeServices,
                CreatedAt = p.CreatedAt,
                CreatedBy = p.CreatedBy
            };

            return Result.Success(response);
        }

        public async Task<Result> UpdateAsync(int id, UpdatePractitionerRequest request)
        {
            var existing = await _practitionerRepository.GetByIdAsync(id);
            if (existing is null)
            {
                return Result.Failure($"Practitioner with id {id} not found.");
            }

            existing.Name = request.Name;
            existing.Active = request.Active;
            existing.UpdatedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified);

            await _practitionerRepository.UpdateAsync();
            if (request.ServiceIds is not null)
            {
                await _practitionerRepository.UpdateServicesAsync(id, request.ServiceIds);
            }

            return Result.Success("Odontólogo actualizado correctamente.");
        }

        public async Task<Result> DeleteAsync(int id)
        {
            var existing = await _practitionerRepository.GetByIdAsync(id);
            if (existing is null)
            {
                return Result.Failure($"Practitioner with id {id} not found.");
            }

            await _practitionerRepository.DeleteAsync(id);
            return Result.Success("Odontólogo eliminado correctamente.");
        }

        public async Task<Result<List<ListPractitionersResponse>>> ListAsync(PagedRequest request)
        {
            var result = await _practitionerRepository.ListAsync(
                predicate: p => p.Status &&
                    (string.IsNullOrEmpty(request.Search) ||
                     p.Name.Contains(request.Search) ||
                     p.Services.Any(ps => ps.Service != null && ps.Service.Status && ps.Service.Name.Contains(request.Search))),
                selector: p => new ListPractitionersResponse
                {
                    Id = p.Id,
                    Name = p.Name,
                    Active = p.Active,
                    ServiceIds = p.Services
                        .Where(ps => ps.Service != null && ps.Service.Status)
                        .Select(ps => ps.ServiceId)
                        .ToList(),
                    Services = p.Services
                        .Where(ps => ps.Service != null && ps.Service.Status)
                        .Select(ps => new PractitionerServiceItemResponse
                        {
                            Id = ps.Service.Id,
                            Name = ps.Service.Name
                        })
                        .ToList()
                },
                pageNumber: request.PageNumber,
                pageSize: request.PageSize);

            return Result.Success(result.Result.ToList());
        }
    }
}
