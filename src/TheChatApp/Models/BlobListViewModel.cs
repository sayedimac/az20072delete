namespace TheChatApp.Models;

public record BlobItem(string Name, long Size, DateTimeOffset? LastModified);

public class BlobListViewModel
{
    public string ContainerName { get; set; } = string.Empty;
    public List<BlobItem> Blobs { get; } = [];
    public string? Error { get; set; }
    public bool IsConfigured => !string.IsNullOrWhiteSpace(ContainerName);
}
