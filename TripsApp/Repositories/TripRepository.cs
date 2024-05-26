using Microsoft.EntityFrameworkCore;
using TripsApp.Context;
using TripsApp.Entities;
using TripsApp.Models;
using TripsApp.Services;

namespace TripsApp.Repositories;

public class TripRepository(MasterContext context) : ITripRepository
{
    public async Task<IEnumerable<TripDTO>> GetTrips()
    {
        var trips = await context.Trips
            .Include(t => t.CountryTrips)
            .ThenInclude(ct => ct.Country)
            .Include(t => t.ClientTrips)
            .ThenInclude(ct => ct.IdClientNavigation)
            .OrderByDescending(t => t.DateFrom)
            .Select(t => new TripDTO(
                t.Name,
                t.Description,
                t.DateFrom,
                t.DateTo,
                t.MaxPeople,
                t.CountryTrips.Select(ct => new CountryDTO(ct.Country.Name)).ToList(),
                t.ClientTrips.Select(ct => new ClientDTO(ct.IdClientNavigation.FirstName, ct.IdClientNavigation.LastName)).ToList()
            ))
            .ToListAsync();

        return trips;
    }

    public async Task<PaginatedTripsDTO> GetTrips(int page, int pageSize)
    {
        var totalTrips = await context.Trips.CountAsync();
        var trips = await context.Trips
            .Include(t => t.CountryTrips)
            .ThenInclude(ct => ct.Country)
            .Include(t => t.ClientTrips)
            .ThenInclude(ct => ct.IdClientNavigation)
            .OrderByDescending(t => t.DateFrom)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(t => new TripDTO(
                t.Name,
                t.Description,
                t.DateFrom,
                t.DateTo,
                t.MaxPeople,
                t.CountryTrips.Select(ct => new CountryDTO(ct.Country.Name)).ToList(),
                t.ClientTrips.Select(ct => new ClientDTO(ct.IdClientNavigation.FirstName, ct.IdClientNavigation.LastName)).ToList()
            ))
            .ToListAsync();

        var allPages = (int) Math.Ceiling((double)totalTrips / pageSize);

        return new PaginatedTripsDTO(page, pageSize, allPages, trips);
    }
    
    public async Task<Trip?> GetTripById(int idTrip)
    {
        return await context.Trips.FindAsync(idTrip);
    }

    public async Task<bool> IsClientAssignedToTrip(int idTrip, int idClient)
    {
        return await context.ClientTrips.AnyAsync(ct => ct.IdTrip == idTrip && ct.IdClient == idClient);
    }

    public async Task AssignClientToTrip(int idTrip, int idClient, DateTime? paymentDate, DateTime registeredAt)
    {
        var clientTrip = new ClientTrip
        {
            IdTrip = idTrip,
            IdClient = idClient,
            PaymentDate = paymentDate,
            RegisteredAt = registeredAt
        };

        context.ClientTrips.Add(clientTrip);
        await context.SaveChangesAsync();
    }
}