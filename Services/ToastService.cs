namespace OrderManagement.Client.Services;

/// <summary>
/// Represents a single toast notification message.
/// </summary>
public class ToastMessage
{
    public string Text { get; set; } = string.Empty;
    public bool IsSuccess { get; set; }
    public Guid Id { get; set; } = Guid.NewGuid();
}

/// <summary>
/// Provides a simple event-driven toast notification system for the UI.
/// </summary>
public class ToastService
{
    public event Action<ToastMessage>? OnShow;

    public void ShowSuccess(string message)
    {
        OnShow?.Invoke(new ToastMessage { Text = message, IsSuccess = true });
    }

    public void ShowError(string message)
    {
        OnShow?.Invoke(new ToastMessage { Text = message, IsSuccess = false });
    }
}
