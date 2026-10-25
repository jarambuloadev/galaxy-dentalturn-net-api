using Galaxy.DentalTurn.DataAccess.Context;
using Galaxy.DentalTurn.DataAccess.Entities;
using Galaxy.DentalTurn.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Galaxy.DentalTurn.Repositories.Implementations
{
    public class PractitionerRepository(DentalTurnDbContext context) : BaseRepository<Practitioner>(context), IPractitionerRepository
    {
        public async Task<Practitioner?> GetByIdWithServicesAsync(int id)
        {
            return await _context.Practitioners
                .Include(p => p.Services)
                    .ThenInclude(ps => ps.Service)
                .FirstOrDefaultAsync(p => p.Status && p.Id == id);
        }

        public async Task UpdateServicesAsync(int practitionerId, List<int> serviceIds)
        {
            var existing = await _context.PractitionerServices
                .Where(ps => ps.PractitionerId == practitionerId)
                .ToListAsync();

            if (existing.Count > 0)
            {
                _context.PractitionerServices.RemoveRange(existing);
            }

            var distinctIds = serviceIds.Distinct().ToList();
            var now = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified);

            foreach (var serviceId in distinctIds)
            {
                _context.PractitionerServices.Add(new PractitionerService
                {
                    PractitionerId = practitionerId,
                    ServiceId = serviceId,
                    Status = true,
                    CreatedAt = now,
                    UpdatedAt = now
                });
            }

            await _context.SaveChangesAsync();
        }
    }
}
