using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace api;

public class ReadTableEntity
{
    private readonly ILogger<ReadTableEntity> _logger;

    public ReadTableEntity(ILogger<ReadTableEntity> logger)
    {
        _logger = logger;
    }

    [Function("ReadTableEntity")]
    public IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");
        return new OkObjectResult("Welcome to Azure Functions!");
    }
}
