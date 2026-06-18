using OrderManagement.Client.Models;

namespace OrderManagement.Client.Data;

/// <summary>
/// Manages the Orders collection in browser localStorage.
/// Handles table creation (first-time key setup) and data seeding.
/// </summary>
public class AppDbContext
{
    private const string OrdersKey    = "om_orders";
    private const string SeededKey    = "om_seeded";
    private const int    NextIdKey_Start = 1;

    private readonly LocalStorageService _storage;
    private bool _initialized = false;

    public AppDbContext(LocalStorageService storage)
    {
        _storage = storage;
    }

    /// <summary>
    /// Initializes the store: creates the orders list and seeds sample data
    /// on the very first run. Safe to call multiple times.
    /// </summary>
    public async Task InitializeAsync()
    {
        if (_initialized) return;

        bool seeded = await _storage.ContainsKeyAsync(SeededKey);
        if (!seeded)
        {
            var seedData = new List<Order>
            {
                new() { Id = 1, OrderNo = "ORD-2024-001", Status = OrderStatus.Pending   },
                new() { Id = 2, OrderNo = "ORD-2024-002", Status = OrderStatus.Done       },
                new() { Id = 3, OrderNo = "ORD-2024-003", Status = OrderStatus.Delivered  },
                new() { Id = 4, OrderNo = "ORD-2024-004", Status = OrderStatus.Pending    },
                new() { Id = 5, OrderNo = "ORD-2024-005", Status = OrderStatus.Done       },
            };

            await _storage.SetAsync(OrdersKey, seedData);
            await _storage.SetAsync(SeededKey, true);
        }

        _initialized = true;
    }

    // ── Internal helpers used by OrderRepository ────────────────────────────

    public async Task<List<Order>> ReadAllAsync()
    {
        await EnsureInitAsync();
        return await _storage.GetAsync<List<Order>>(OrdersKey) ?? new List<Order>();
    }

    public async Task WriteAllAsync(List<Order> orders)
    {
        await _storage.SetAsync(OrdersKey, orders);
    }

    private async Task EnsureInitAsync()
    {
        if (!_initialized) await InitializeAsync();
    }
}
