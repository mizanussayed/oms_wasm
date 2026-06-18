namespace OrderManagement.Client.Models;

public sealed class WhatsAppSettings
{
    public string AdminPhone { get; set; } = string.Empty;
    public string MessageTemplate { get; set; } =
        "আপনার অর্ডার {OrderNo} সম্পন্ন হয়েছে। ধন্যবাদ!";
}
