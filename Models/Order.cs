namespace OrderManagement.Client.Models;

/// <summary>
/// Represents a customer order stored in the browser's localStorage.
/// </summary>
public class Order
{
    public int Id { get; set; }
    public string OrderNo { get; set; } = string.Empty;
    public OrderStatus Status { get; set; }
}
