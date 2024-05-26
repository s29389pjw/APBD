using TripsApp.Entities;

namespace TripsApp.Services;

public interface IClientService
{
    Task<Client?> DeleteClient(int idClient);
}