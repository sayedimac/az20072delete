using System.Diagnostics;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Mvc;
using TheChatApp.Models;

namespace TheChatApp.Controllers;

public class HomeController : Controller
{
    private readonly BlobServiceClient? _blobServiceClient;
    private readonly IConfiguration _config;

    public HomeController(IConfiguration config, BlobServiceClient? blobServiceClient = null)
    {
        _config = config;
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
