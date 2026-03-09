namespace Marketplace.Domain.Entities;

public class Order
{
    public int Id { get; set; }

    // Foreign Key to User
    public int UserId { get; set; }
    public User User { get; set; } = null!;

    // Foreign Key to Product
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;

    // Order Details
    public int Quantity { get; set; }
    public decimal TotalPrice { get; set; }
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
}
