using OrderManagement.Client.Models;

namespace OrderManagement.Client.Services;

/// <summary>
/// Defines business logic operations for managing orders.
/// </summary>
public interface IOrderService
{
    Task<List<Order>> GetAllOrdersAsync();
    Task<Order?> GetOrderByIdAsync(int id);
    Task<(bool Success, string Message)> CreateOrderAsync(Order order);
    Task<(bool Success, string Message)> UpdateOrderAsync(Order order);
    Task<(bool Success, string Message)> DeleteOrderAsync(int id);
    Task<List<Order>> SearchOrdersAsync(string? orderNo, OrderStatus? status, bool sortByOrderNo = false);
    Task<DashboardStats> GetDashboardStatsAsync();
}

/// <summary>
/// Holds aggregated statistics for the dashboard page.
/// </summary>
public class DashboardStats
{
    public int Total { get; set; }
    public int Pending { get; set; }
    public int Done { get; set; }
    public int Delivered { get; set; }
}
