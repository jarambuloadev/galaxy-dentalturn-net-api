
using Galaxy.DentalTurn.Business.DTO.Request.Service;
using Galaxy.DentalTurn.Business.DTO.Response;
using Galaxy.DentalTurn.Business.Interfaces;
using Galaxy.DentalTurn.Common.Helpers;
using Galaxy.DentalTurn.DataAccess.Entities;
using Galaxy.DentalTurn.Repositories.Implementations;
using Galaxy.DentalTurn.Repositories.Interfaces;
using Mapster;

namespace Galaxy.DentalTurn.Business.Implementations
{
    public class ServiceService : IServiceService
    {
        private readonly IServiceRepository _serviceRepository;

        public ServiceService(IServiceRepository serviceRepository)
        {
            _serviceRepository = serviceRepository;
        }

        public async Task<Result> CreateAsync(UpdateServiceRequest request)
        {
            var service = request.Adapt<Service>();
            var result = await _serviceRepository.CreateAsync(service);
            return Result.Success("Service created successfully.");
        }

        public async Task<Result<GetServiceResponse>> GetByIdAsync(int id)
        {
            var result = await _serviceRepository.GetByIdAsync(id);
            if (result is null)
            {
                return Result.Failure<GetServiceResponse>($"Service with id {id} not found.");                
            }
            return Result.Success(result.Adapt<GetServiceResponse>());
        }

        public async Task<Result> UpdateAsync(int id, UpdateServiceRequest request)
        {
            var result = await _serviceRepository.GetByIdAsync(id);
            if (result is null)
            {
                return Result.Failure<GetServiceResponse>($"Service with id {id} not found.");
            }
            var service = request.Adapt(result);
            await _serviceRepository.UpdateAsync();
            return Result.Success("Service updated successfully.");
        }

        public async Task<Result> DeleteAsync(int id)
        {
            var result = await _serviceRepository.GetByIdAsync(id);
            if (result is null)
            {
                return Result.Failure<GetServiceResponse>($"Service with id {id} not found.");
            }
            
            await _serviceRepository.DeleteAsync(id);
            return Result.Success("Service deleted successfully.");
        }

        public async Task<Result<List<ListServiciosResponse>>> ListAsync(int pageNumnber = 1, int pageSize = 10)
        {
            var result = await _serviceRepository.ListAsync(
                predicate: p => p.Status,
                selector: p => new ListServiciosResponse
                {
                    Name = p.Name,
                    Description = p.Description,
                    DurationMin = p.DurationMin,
                    Active = p.Status
                },
                pageNumber: pageNumnber,
                pageSize: pageSize);
            return Result.Success(result.Result.ToList());
        }
    }
}
