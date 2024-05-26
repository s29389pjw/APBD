using TripsApp.Entities;
using TripsApp.Models;

namespace TripsApp.Repositories;

public interface ITripRepository
{
    public Task<IEnumerable<TripDTO>> GetTrips();
    public Task<PaginatedTripsDTO> GetTrips(int page, int pageSize);
    
    public Task<Trip?> GetTripById(int idTrip);
    public Task<bool> IsClientAssignedToTrip(int idTrip, int idClient);
    public Task AssignClientToTrip(int idTrip, int idClient, DateTime? paymentDate, DateTime registeredAt);
}