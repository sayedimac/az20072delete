using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using System.Text;
using TheChatApp.Controllers;
using Xunit;

namespace TheChatApp.Tests.Controllers;

public class HomeControllerTests
{
    [Fact]
    public async Task Document_WhenStorageIsNotConfigured_ReturnsNotFound()
    {
        var controller = CreateController();

        var result = await controller.Document("document.pdf", CancellationToken.None);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Document_WhenBlobNameIsMissing_ReturnsBadRequest()
    {
        var serviceClient = new Mock<BlobServiceClient>();
        var controller = CreateController("documents", serviceClient.Object);

        var result = await controller.Document(null, CancellationToken.None);

        Assert.IsType<BadRequestResult>(result);
    }

    [Fact]
    public async Task Document_WhenBlobExists_ReturnsStreamWithContentType()
    {
        var content = new MemoryStream(Encoding.UTF8.GetBytes("test document"));
        var download = BlobsModelFactory.BlobDownloadStreamingResult(
            content,
            BlobsModelFactory.BlobDownloadDetails(contentType: "application/pdf"));
        var blobClient = new Mock<BlobClient>();
        blobClient
            .Setup(client => client.DownloadStreamingAsync(
                It.IsAny<BlobDownloadOptions>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(Response.FromValue(download, Mock.Of<Response>()));
        var controller = CreateController("documents", CreateServiceClient(blobClient.Object, "report.pdf"));

        var result = await controller.Document("report.pdf", CancellationToken.None);

        var fileResult = Assert.IsType<FileStreamResult>(result);
        Assert.Same(content, fileResult.FileStream);
        Assert.Equal("application/pdf", fileResult.ContentType);
    }

    [Fact]
    public async Task Document_WhenBlobDoesNotExist_ReturnsNotFound()
    {
        var blobClient = new Mock<BlobClient>();
        blobClient
            .Setup(client => client.DownloadStreamingAsync(
                It.IsAny<BlobDownloadOptions>(),
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(new RequestFailedException(404, "Blob not found"));
        var controller = CreateController("documents", CreateServiceClient(blobClient.Object, "missing.pdf"));

        var result = await controller.Document("missing.pdf", CancellationToken.None);

        Assert.IsType<NotFoundResult>(result);
    }

    private static HomeController CreateController(
        string? containerName = null,
        BlobServiceClient? serviceClient = null)
    {
        var settings = new Dictionary<string, string?>();
        if (containerName != null)
            settings["BlobStorage:ContainerName"] = containerName;

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(settings)
            .Build();

        return new HomeController(
            configuration,
            NullLogger<HomeController>.Instance,
            serviceClient);
    }

    private static BlobServiceClient CreateServiceClient(BlobClient blobClient, string blobName)
    {
        var containerClient = new Mock<BlobContainerClient>();
        containerClient
            .Setup(client => client.GetBlobClient(blobName))
            .Returns(blobClient);

        var serviceClient = new Mock<BlobServiceClient>();
        serviceClient
            .Setup(client => client.GetBlobContainerClient("documents"))
            .Returns(containerClient.Object);

        return serviceClient.Object;
    }
}
