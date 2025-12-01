---
applyTo: "src/api"
---

- This is a C# Azure Functions app targeting .NET 8.0 (Isolated Worker)
- Connects to Azure services via connection strings in environment variables (e.g., Azure Storage Account)
- One function retrieves a list of blobs from a specified container (container name as parameter)
- Another function reads data from Azure Table Storage using partition key and row key (parameters)
- Use latest SDKs compatible with .NET 8 (Azure.Storage.Blobs, Azure.Data.Tables) and the isolated worker (Microsoft.Azure.Functions.Worker)
