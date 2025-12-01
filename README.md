# az20072delete

## Project Structure

- `src/api`: Azure Functions app (dotnet isolated, net8.0)
  - Functions: `ListBlobs` (HTTP trigger), `ReadTableEntity` (HTTP trigger)
- `src/web`: Blazor WebAssembly app (net10.0)

## Prerequisites

- .NET SDK 8.0+ installed
- Azure Functions Core Tools (`func`) installed
- Azure Storage account connection strings available via environment variables

## First-Time Setup

```powershell
# Verify tools
func --version
dotnet --info

# Build projects
Push-Location "d:\Repos\az20072delete\src\api"; dotnet restore; dotnet build; Pop-Location
Push-Location "d:\Repos\az20072delete\src\web"; dotnet restore; dotnet build; Pop-Location
```

## Run Locally

```powershell
# Start Functions app
Push-Location "d:\Repos\az20072delete\src\api"; func start; Pop-Location

# Serve Blazor WASM (static files)
Push-Location "d:\Repos\az20072delete\src\web"; dotnet serve -d bin\Debug\net10.0\wwwroot; Pop-Location
```

## Environment Configuration

Set these environment variables before running Functions:

- `AzureWebJobsStorage`: connection string for Azure Storage
- `BlobContainerName`: container name to list in `ListBlobs`
- `TableAccountConnection`: connection string for the Table Storage account

You can set variables in PowerShell like:

```powershell
$env:AzureWebJobsStorage = "<storage-connection-string>"
$env:BlobContainerName = "<container>"
$env:TableAccountConnection = "<table-connection-string>"
```

## Next Steps

- Implement blob listing and table reads in `src/api` functions.
- Wire up `src/web` to call the Functions endpoints and display results.
