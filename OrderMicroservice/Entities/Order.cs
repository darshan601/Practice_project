namespace OrderMicroservice.Entities;

public class Order
{
    public int OrderId { get; set; }

    public DateTime orderDate { get; set; }

    public int UserId { get; set; }
    
    public int MovieId { get; set; }
}