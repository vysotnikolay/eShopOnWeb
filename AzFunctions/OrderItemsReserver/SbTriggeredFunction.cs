using System;
using System.Text;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Azure.Storage.Blobs;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;

namespace SbTriggerFunction
{
    public class SbTriggeredFunction
    {
        private readonly ILogger<SbTriggeredFunction> _logger;
        private readonly BlobServiceClient _blobServiceClient;

        public SbTriggeredFunction(ILogger<SbTriggeredFunction> logger, BlobServiceClient blobServiceClient)
        {
            _logger = logger;
            _blobServiceClient = blobServiceClient;
        }

        [Function(nameof(SbTriggeredFunction))]
        public async Task Run(
            [ServiceBusTrigger("orders", Connection = "SbConnString")]
             ServiceBusReceivedMessage message,
            ServiceBusMessageActions messageActions
             )
        {
            var body = message.Body;
            _logger.LogInformation($"Message: {body}");
            var blobContainerClient = _blobServiceClient.GetBlobContainerClient("orders");
            var blobClient = blobContainerClient.GetBlobClient($"{message.MessageId}.json");
            _logger.LogInformation($"Uploading to blob with Id: {message.MessageId}");
            await blobClient.UploadAsync(body);
            await messageActions.CompleteMessageAsync(message);
            _logger.LogInformation($"Uploaded successfully.");
        }
    }
}
