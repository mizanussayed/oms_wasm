using OrderManagement.Client.Data;
using OrderManagement.Client.Models;

namespace OrderManagement.Client.Repositories;

/// <summary>
/// localStorage-backed implementation of IOrderRepository.
/// All data is persisted as JSON in the browser's localStorage via AppDbContext.
/// </summary>
public class OrderRepository : IOrderRepository
{
    private readonly AppDbContext _context;

    public OrderRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Order>> GetAllAsync()
    {
        return await _context.ReadAllAsync();
    }

    public async Task<Order?> GetByIdAsync(int id)
    {
        var orders = await _context.ReadAllAsync();
        return orders.FirstOrDefault(o => o.Id == id);
    }

    public async Task<int> AddAsync(Order order)
    {
        var orders = await _context.ReadAllAsync();

        // Auto-increment: find next available ID
        order.Id = orders.Count > 0 ? orders.Max(o => o.Id) + 1 : 1;
        orders.Add(order);

        await _context.WriteAllAsync(orders);
        return order.Id;
    }

    public async Task<int> UpdateAsync(Order order)
    {
        var orders = await _context.ReadAllAsync();
        var index = orders.FindIndex(o => o.Id == order.Id);

        if (index < 0) return 0;

        orders[index] = order;
        await _context.WriteAllAsync(orders);
        return 1;
    }

    public async Task<int> DeleteAsync(int id)
    {
        var orders = await _context.ReadAllAsync();
        var removed = orders.RemoveAll(o => o.Id == id);
        await _context.WriteAllAsync(orders);
        return removed;
    }

    public async Task<List<Order>> SearchAsync(string? orderNo, OrderStatus? status, bool sortByOrderNo = false)
    {
        var orders = await _context.ReadAllAsync();

        // Filter by status
        if (status.HasValue)
            orders = orders.Where(o => o.Status == status.Value).ToList();

        // Filter by order number (case-insensitive contains)
        if (!string.IsNullOrWhiteSpace(orderNo))
            orders = orders.Where(o => o.OrderNo.Contains(orderNo, StringComparison.OrdinalIgnoreCase)).ToList();

        // Sort if requested
        if (sortByOrderNo)
            orders = orders.OrderBy(o => o.OrderNo).ToList();

        return orders;
    }

    public async Task<int> GetTotalCountAsync()
    {
        var orders = await _context.ReadAllAsync();
        return orders.Count;
    }

    public async Task<int> GetCountByStatusAsync(OrderStatus status)
    {
        var orders = await _context.ReadAllAsync();
        return orders.Count(o => o.Status == status);
    }
}
