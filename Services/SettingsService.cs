using OrderManagement.Client.Data;
using OrderManagement.Client.Models;

namespace OrderManagement.Client.Services;

/// <summary>
/// Manages WhatsApp admin settings (phone number + message template)
/// persisted in browser localStorage.
/// </summary>
public class SettingsService
{
    private const string SettingsKey = "om_settings";
    private readonly LocalStorageService _storage;

    public SettingsService(LocalStorageService storage)
    {
        _storage = storage;
    }

    public async Task<WhatsAppSettings> GetSettingsAsync()
    {
        return await _storage.GetAsync<WhatsAppSettings>(SettingsKey)
               ?? new WhatsAppSettings();
    }

    public async Task SaveSettingsAsync(WhatsAppSettings settings)
    {
        await _storage.SetAsync(SettingsKey, settings);
    }

    public async Task<string?> BuildWhatsAppUrlAsync(string? customerPhone, string orderNo)
    {
        var settings = await GetSettingsAsync();

        // Prefer customer phone; fall back to admin phone
        var phone = !string.IsNullOrWhiteSpace(customerPhone)
            ? customerPhone
            : settings.AdminPhone;

        if (string.IsNullOrWhiteSpace(phone)) return null;

        var template = string.IsNullOrWhiteSpace(settings.MessageTemplate)
            ? "আপনার অর্ডার {OrderNo} সম্পন্ন হয়েছে। ধন্যবাদ! 🎉"
            : settings.MessageTemplate;

        var message = template.Replace("{OrderNo}", orderNo);
        var encoded = Uri.EscapeDataString(message);

        // Sanitise phone: keep only digits
        var digits = new string(phone.Where(char.IsDigit).ToArray());

        return $"https://wa.me/{digits}?text={encoded}";
    }
}
