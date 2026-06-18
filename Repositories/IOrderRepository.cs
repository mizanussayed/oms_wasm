using OrderManagement.Client.Models;

namespace OrderManagement.Client.Repositories;

/// <summary>
/// Defines data access operations for the Order entity.
/// </summary>
public interface IOrderRepository
{
    Task<List<Order>> GetAllAsync();
    Task<Order?> GetByIdAsync(int id);
    Task<int> AddAsync(Order order);
    Task<int> UpdateAsync(Order order);
    Task<int> DeleteAsync(int id);

    /// <summary>
    /// Returns filtered and optionally sorted orders.
    /// </summary>
    Task<List<Order>> SearchAsync(string? orderNo, OrderStatus? status, bool sortByOrderNo = false);

    Task<int> GetTotalCountAsync();
    Task<int> GetCountByStatusAsync(OrderStatus status);
}
