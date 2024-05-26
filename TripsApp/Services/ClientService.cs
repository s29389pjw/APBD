using TripsApp.Entities;
using TripsApp.Repositories;

namespace TripsApp.Services;

public class ClientService(IClientRepository clientRepository) : IClientService
{
    public async Task<Client?> DeleteClient(int idClient)
    {
        if (await clientRepository.HasTrips(idClient))
        {
            return null;
        }

        return await clientRepository.DeleteClient(idClient);
    }
}