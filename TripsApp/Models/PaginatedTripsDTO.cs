namespace TripsApp.Models;

public record PaginatedTripsDTO(
    int PageNum,
    int PageSize,
    int AllPages,
    List<TripDTO> Trips
);