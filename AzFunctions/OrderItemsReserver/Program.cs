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
                connString = "DefaultEndpointsProtocol=https;AccountName=task7storage;AccountKey=oV1RYjyVl+Zw2BPICQQu9eXC14wQ9UoYOM0Z6Xd4+grwQ/qI2dxoN30KD48ulw/bPyhc0in1dac5+AStdxQtAA==;EndpointSuffix=core.windows.net";
            }
            return new BlobServiceClient(connString);
        });
    })
    .Build();

host.Run();
