using Azure.Storage.Queues;
using System.Text.Json;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    public class QueueService:IQueueService
    {
        private readonly IConfiguration _configuration;
        private readonly string queueName = "emailqueue";

        public QueueService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendMessage(Email email)
        {
            var queueClient = new QueueClient(_configuration["AzureStorageConnectionString"],queueName,
                new QueueClientOptions { MessageEncoding = QueueMessageEncoding.Base64});

            await queueClient.CreateIfNotExistsAsync();

            var message = JsonSerializer.Serialize(email);

            await queueClient.SendMessageAsync(message);
        }
    }
}
