using System.Text.RegularExpressions;
using Galaxy.DentalTurn.Business.DTO.Request.BusinessHours;
using Galaxy.DentalTurn.Business.DTO.Response;
using Galaxy.DentalTurn.Business.Interfaces;
using Galaxy.DentalTurn.Common.Helpers;
using Galaxy.DentalTurn.DataAccess.Entities;
using Galaxy.DentalTurn.Repositories.Interfaces;
using Mapster;

namespace Galaxy.DentalTurn.Business.Implementations
{
    public class BusinessHoursService : IBusinessHoursService
    {
        private static readonly Regex TimeRegex = new(@"^([01]\d|2[0-3]):[0-5]\d$", RegexOptions.Compiled);
        private readonly IBusinessHoursRepository _repository;

        public BusinessHoursService(IBusinessHoursRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<List<BusinessHoursResponse>>> ListAsync()
        {
            var rows = await _repository.GetAllOrderedByDayAsync();

            if (rows.Count == 0)
            {
                for (int day = 0; day <= 6; day++)
                {
                    var isOpen = day >= 1 && day <= 5;
                    var entity = await _repository.CreateAsync(new BusinessHours
                    {
                        DayOfWeek = day,
                        IsOpen = isOpen,
                        OpenTime = "09:00",
                        CloseTime = "18:00",
                        Status = true
                    });
                    rows.Add(entity);
                }
            }

            var response = rows.Adapt<List<BusinessHoursResponse>>();
            return Result.Success(response);
        }

        public async Task<Result> UpdateAllAsync(UpdateBusinessHoursRequest request)
        {
            if (request.Days is null || request.Days.Count == 0)
            {
                return Result.Failure("Debe enviar los horarios de atención.");
            }

            foreach (var day in request.Days)
            {
                if (day.DayOfWeek < 0 || day.DayOfWeek > 6)
                {
                    return Result.Failure($"Día de la semana inválido: {day.DayOfWeek}");
                }

                if (!TimeRegex.IsMatch(day.OpenTime) || !TimeRegex.IsMatch(day.CloseTime))
                {
                    return Result.Failure("Revisa los horarios ingresados: Hora inválida");
                }

                if (day.IsOpen && string.CompareOrdinal(day.OpenTime, day.CloseTime) >= 0)
                {
                    return Result.Failure("Revisa los horarios ingresados: La hora de fin debe ser posterior a la hora de inicio");
                }
            }

            var existingRows = await _repository.GetAllOrderedByDayAsync();
            var byDay = existingRows.ToDictionary(x => x.DayOfWeek);

            foreach (var day in request.Days)
            {
                if (byDay.TryGetValue(day.DayOfWeek, out var existing))
                {
                    existing.IsOpen = day.IsOpen;
                    existing.OpenTime = day.OpenTime;
                    existing.CloseTime = day.CloseTime;
                    existing.Status = true;
                    existing.UpdatedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified);
                }
                else
                {
                    await _repository.CreateAsync(new BusinessHours
                    {
                        DayOfWeek = day.DayOfWeek,
                        IsOpen = day.IsOpen,
                        OpenTime = day.OpenTime,
                        CloseTime = day.CloseTime,
                        Status = true
                    });
                }
            }

            await _repository.UpdateAsync();
            return Result.Success("Horario de atención actualizado");
        }
    }
}
