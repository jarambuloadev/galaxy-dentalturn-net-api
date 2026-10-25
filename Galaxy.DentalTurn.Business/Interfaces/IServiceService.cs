using Galaxy.DentalTurn.Business.DTO.Request;
using Galaxy.DentalTurn.Business.DTO.Request.Service;
using Galaxy.DentalTurn.Business.DTO.Response;
using Galaxy.DentalTurn.Common.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Galaxy.DentalTurn.Business.Interfaces
{
    public interface IServiceService
    {
        Task<Result> CreateAsync(UpdateServiceRequest request);
        Task<Result<GetServiceResponse>> GetByIdAsync(int id);
        Task<Result> UpdateAsync(int id, UpdateServiceRequest request);

        Task<Result> DeleteAsync(int id);
        Task<Result<List<ListServiciosResponse>>> ListAsync(PagedRequest request);

    }
}
