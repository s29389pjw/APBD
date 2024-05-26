using TripsApp.Models;
using TripsApp.Repositories;

namespace TripsApp.Services
{
    public class TripService(ITripRepository tripRepository, IClientRepository clientRepository)
        : ITripService
    {
        public async Task<IEnumerable<TripDTO>> GetTrips()
        {
            return await tripRepository.GetTrips();
        }

        public async Task<PaginatedTripsDTO> GetTrips(int page, int pageSize)
        {
            return await tripRepository.GetTrips(page, pageSize);
        }

        public async Task<bool> AssignClientToTrip(int idTrip, string pesel, DateTime? paymentDate)
        {
            // Check if the client with the given PESEL already exists
            var existingClient = await clientRepository.GetClientByPesel(pesel);
            if (existingClient == null)
            {
                return false; // Client does not exist
            }

            // Check if the client is already assigned to the trip
            if (await tripRepository.IsClientAssignedToTrip(idTrip, existingClient.IdClient))
            {
                return false; // Client is already assigned to the trip
            }

            // Check if the trip exists and if DateFrom is in the future
            var trip = await tripRepository.GetTripById(idTrip);
            if (trip == null || trip.DateFrom <= DateTime.Now)
            {
                return false; // Trip does not exist or DateFrom is in the past
            }

            // Assign the client to the trip
            await tripRepository.AssignClientToTrip(idTrip, existingClient.IdClient, paymentDate, DateTime.Now);
            return true;
        }
    }
}