using TripsApp.Models;

namespace TripsApp.Services;

public interface ITripService
{
    public Task<IEnumerable<TripDTO>> GetTrips();
    public Task<PaginatedTripsDTO> GetTrips(int page, int pageSize);
    
    public Task<bool> AssignClientToTrip(int idTrip, string pesel, DateTime? paymentDate);
}