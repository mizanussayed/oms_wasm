using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using OrderManagement.Client;
using OrderManagement.Client.Data;
using OrderManagement.Client.Repositories;
using OrderManagement.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");


builder.Services.AddScoped(sp =>
    new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// Data layer — localStorage-backed (singleton so the same instance is reused across pages)
builder.Services.AddSingleton<LocalStorageService>();
builder.Services.AddSingleton<AppDbContext>();

// Repository and Service (scoped is fine — they resolve the singleton context)
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderService, OrderService>();

// Toast notification service (singleton to survive page navigation)
builder.Services.AddSingleton<ToastService>();

// WhatsApp settings service (singleton — persists across pages)
builder.Services.AddSingleton<SettingsService>();

var host = builder.Build();

// Seed the database before launching the app.
// We resolve AppDbContext here — but IJSRuntime is not yet available at this point
// in the startup pipeline, so seeding is deferred to the first page load via
// AppDbContext.EnsureInitAsync(), which is called internally by ReadAllAsync().

await host.RunAsync();
