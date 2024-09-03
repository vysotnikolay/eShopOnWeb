using Azure.Storage.Blobs;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices(services =>
    {
        
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
        services.AddSingleton<BlobServiceClient>(provider =>
        {
            var config = provider.GetRequiredService<IConfiguration>();
            var connString = config.GetValue<string>("BlobConnStr");
            if (connString == null)
            {
				// TODO: Change BlobConnStr
                connString = "DefaultEndpointsProtocol=https;AccountName=eshoponwebstorageaccount;AccountKey=Z6GFAt1PYZyL7JSwws6SNrQMCp+8tBmx2cMzl879RsZSFcxwjpGMAYoDp99sYjUbC8nQFbuvNpB3+AStpJKI1w==;EndpointSuffix=core.windows.net";
            }
            return new BlobServiceClient(connString);
        });
    })
    .Build();

host.Run();
