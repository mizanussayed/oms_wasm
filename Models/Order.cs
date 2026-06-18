namespace OrderManagement.Client.Models;

/// <summary>
/// Represents a customer order stored in the browser's localStorage.
/// </summary>
public class Order
{
    public int Id { get; set; }
    public string OrderNo { get; set; } = string.Empty;
    public OrderStatus Status { get; set; }
    /// <summary>
    /// Optional customer phone number (international format, e.g. 8801XXXXXXXXX).
    /// Used to send a WhatsApp message when the order is marked as Done.
    /// </summary>
    public string? PhoneNumber { get; set; }
}
