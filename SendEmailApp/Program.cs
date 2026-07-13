using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using Microsoft.Extensions.Configuration;

var config = new ConfigurationBuilder()
            .AddUserSecrets<Program>()   // binds to UserSecretsId in .csproj
            .Build();

string azureConnectionString = config["AzureStorageConnectionString"];
var queueClient = new QueueClient(azureConnectionString, "emailqueue", new QueueClientOptions { MessageEncoding = QueueMessageEncoding.Base64 });

if(await queueClient.ExistsAsync())
{
    QueueProperties properties = await queueClient.GetPropertiesAsync();

    while (properties.ApproximateMessagesCount > 0)
    {
        QueueMessage[] messages = await queueClient.ReceiveMessagesAsync(maxMessages: 10);

        foreach (QueueMessage message in messages)
        {
            //instead of consoling the message, we can call the email service to send the email
            Console.WriteLine($"Message: {message.MessageText}");
            await queueClient.DeleteMessageAsync(message.MessageId, message.PopReceipt);
        }

        //get the fresh count of messages in the queue after processing the batch
        properties = await queueClient.GetPropertiesAsync();
    }
}
else
{
    Console.WriteLine("Queue does not exist.");
}