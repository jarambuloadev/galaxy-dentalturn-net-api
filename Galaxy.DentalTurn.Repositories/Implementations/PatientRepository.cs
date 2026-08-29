using Galaxy.DentalTurn.DataAccess.Context;
using Galaxy.DentalTurn.DataAccess.Entities;
using Galaxy.DentalTurn.Repositories.Interfaces;

namespace Galaxy.DentalTurn.Repositories.Implementations
{
    public class PatientRepository(DentalTurnDbContext context) : BaseRepository<Patient>(context), IPatientRepository
    {
    }
}
