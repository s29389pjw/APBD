namespace TripsApp.Models

{
    public record TripDTO(
        string Name,
        string Description,
        DateTime DateFrom,
        DateTime DateTo,
        int MaxPeople,
        List<CountryDTO> Countries,
        List<ClientDTO> Clients
    );
}