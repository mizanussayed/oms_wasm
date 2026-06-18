using OrderManagement.Client.Models;
using OrderManagement.Client.Repositories;

namespace OrderManagement.Client.Services;

/// <summary>
/// Implements order business logic, including validation and coordination with the repository.
/// </summary>
public class OrderService : IOrderService
{
    private readonly IOrderRepository _repository;

    public OrderService(IOrderRepository repository)
    {
        _repository = repository;
    }

    public Task<List<Order>> GetAllOrdersAsync() =>
        _repository.GetAllAsync();

    public Task<Order?> GetOrderByIdAsync(int id) =>
        _repository.GetByIdAsync(id);

    public async Task<(bool Success, string Message)> CreateOrderAsync(Order order)
    {
        if (string.IsNullOrWhiteSpace(order.OrderNo))
            return (false, "অর্ডার নম্বর প্রয়োজন।");

        if (order.OrderNo.Length > 100)
            return (false, "অর্ডার নম্বর সর্বোচ্চ ১০০ অক্ষরের হতে হবে।");

        await _repository.AddAsync(order);
        return (true, "অর্ডার সফলভাবে তৈরি হয়েছে।");
    }

    public async Task<(bool Success, string Message)> UpdateOrderAsync(Order order)
    {
        if (string.IsNullOrWhiteSpace(order.OrderNo))
            return (false, "অর্ডার নম্বর প্রয়োজন।");

        var existing = await _repository.GetByIdAsync(order.Id);
        if (existing is null)
            return (false, "অর্ডারটি পাওয়া যায়নি।");

        await _repository.UpdateAsync(order);
        return (true, "অর্ডার সফলভাবে আপডেট হয়েছে।");
    }

    public async Task<(bool Success, string Message)> DeleteOrderAsync(int id)
    {
        var existing = await _repository.GetByIdAsync(id);
        if (existing is null)
            return (false, "অর্ডারটি পাওয়া যায়নি।");

        await _repository.DeleteAsync(id);
        return (true, "অর্ডার সফলভাবে মুছে ফেলা হয়েছে।");
    }

    public Task<List<Order>> SearchOrdersAsync(string? orderNo, OrderStatus? status, bool sortByOrderNo = false) =>
        _repository.SearchAsync(orderNo, status, sortByOrderNo);

    public async Task<DashboardStats> GetDashboardStatsAsync()
    {
        return new DashboardStats
        {
            Total     = await _repository.GetTotalCountAsync(),
            Pending   = await _repository.GetCountByStatusAsync(OrderStatus.Pending),
            Done      = await _repository.GetCountByStatusAsync(OrderStatus.Done),
            Delivered = await _repository.GetCountByStatusAsync(OrderStatus.Delivered),
        };
    }
}
