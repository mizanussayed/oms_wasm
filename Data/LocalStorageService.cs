using Microsoft.JSInterop;
using System.Text.Json;

namespace OrderManagement.Client.Data;

/// <summary>
/// Provides typed read/write access to the browser's localStorage
/// using IJSRuntime and System.Text.Json serialization.
/// </summary>
public class LocalStorageService
{
    private readonly IJSRuntime _js;

    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        WriteIndented = false
    };

    public LocalStorageService(IJSRuntime js)
    {
        _js = js;
    }

    /// <summary>Reads and deserializes a value from localStorage.</summary>
    public async Task<T?> GetAsync<T>(string key)
    {
        try
        {
            var json = await _js.InvokeAsync<string?>("localStorage.getItem", key);
            return string.IsNullOrEmpty(json) ? default : JsonSerializer.Deserialize<T>(json, _jsonOptions);
        }
        catch
        {
            return default;
        }
    }

    /// <summary>Serializes and stores a value in localStorage.</summary>
    public async Task SetAsync<T>(string key, T value)
    {
        var json = JsonSerializer.Serialize(value, _jsonOptions);
        await _js.InvokeVoidAsync("localStorage.setItem", key, json);
    }

    /// <summary>Removes a key from localStorage.</summary>
    public async Task RemoveAsync(string key)
    {
        await _js.InvokeVoidAsync("localStorage.removeItem", key);
    }

    /// <summary>Checks whether a key exists in localStorage.</summary>
    public async Task<bool> ContainsKeyAsync(string key)
    {
        var val = await _js.InvokeAsync<string?>("localStorage.getItem", key);
        return val is not null;
    }
}
