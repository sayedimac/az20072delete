using System.Diagnostics;
using Azure;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Mvc;
using TheChatApp.Models;

namespace TheChatApp.Controllers;

public class HomeController : Controller
{
    private readonly BlobServiceClient? _blobServiceClient;
    private readonly IConfiguration _config;
    private readonly ILogger<HomeController> _logger;

    public HomeController(
        IConfiguration config,
        ILogger<HomeController> logger,
        BlobServiceClient? blobServiceClient = null)
    {
        _config = config;
        _logger = logger;
        _blobServiceClient = blobServiceClient;
    }

    public async Task<IActionResult> Index()
    {
        var model = new BlobListViewModel
        {
            ContainerName = _config["BlobStorage:ContainerName"] ?? string.Empty
        };

        if (_blobServiceClient != null && !string.IsNullOrWhiteSpace(model.ContainerName))
        {
            try
            {
                var container = _blobServiceClient.GetBlobContainerClient(model.ContainerName);
                await foreach (var blob in container.GetBlobsAsync())
                    model.Blobs.Add(new BlobItem(blob.Name, blob.Properties.ContentLength ?? 0, blob.Properties.LastModified));
            }
            catch (Exception ex)
            {
                model.Error = ex.Message;
            }
        }

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Document(string? blobName, CancellationToken cancellationToken)
    {
        var containerName = _config["BlobStorage:ContainerName"];
        if (_blobServiceClient == null || string.IsNullOrWhiteSpace(containerName))
            return NotFound();

        if (string.IsNullOrWhiteSpace(blobName))
            return BadRequest();

        try
        {
            var blob = _blobServiceClient
                .GetBlobContainerClient(containerName)
                .GetBlobClient(blobName);
            var download = await blob.DownloadStreamingAsync(cancellationToken: cancellationToken);
            var contentType = string.IsNullOrWhiteSpace(download.Value.Details.ContentType)
                ? "application/octet-stream"
                : download.Value.Details.ContentType;

            return File(download.Value.Content, contentType);
        }
        catch (RequestFailedException ex) when (ex.Status == StatusCodes.Status404NotFound)
        {
            return NotFound();
        }
        catch (RequestFailedException ex)
        {
            _logger.LogError(ex, "Unable to open blob {BlobName} from container {ContainerName}.", blobName, containerName);
            return StatusCode(StatusCodes.Status502BadGateway);
        }
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
