namespace TripsApp.Models;

public class AssignClientRequest
{
    public string Pesel { get; set; }
    public DateTime? PaymentDate { get; set; }
}