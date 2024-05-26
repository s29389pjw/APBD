using TripsApp.Entities;

namespace TripsApp.Repositories;

public interface IClientRepository
{
    Task<bool> HasTrips(int idClient);
    Task<Client?> DeleteClient(int idClient);
    Task<Client?> GetClientByPesel(string pesel);
}