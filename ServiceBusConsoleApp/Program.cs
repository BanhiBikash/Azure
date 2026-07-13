using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;

string busConnectionString = new ConfigurationBuilder().AddUserSecrets<Program>().Build()["ServiceBusConnectionString"];
const string? seriveBusQueueName = "servicebus-queue";

//creating required objects
ServiceBusClient serviceBusClient;
ServiceBusSender serviceBusSender;

//client to access the service bus and send messages to the queue
var client  = new ServiceBusClient(busConnectionString);
//the sender object is used to send messages to the queue
var sender = client.CreateSender(seriveBusQueueName);

//creating a message batch
using ServiceBusMessageBatch messageBatch = await sender.CreateMessageBatchAsync();

for (int i=1; i<= 10; i++)
{
    if (!messageBatch.TryAddMessage(new ServiceBusMessage($"This is message - {i}")))
    {
        Console.WriteLine($"Message {i} is too large to fit in the batch.");
    }
}

//now sending the message batch
try
{
    await sender.SendMessagesAsync(messageBatch);
    Console.WriteLine("Messages sent.");
}
catch (Exception ex)
{
    Console.WriteLine($"Exception occured while sending the message batch: {ex.Message}");
}
finally
{
    //dispose the objects
    await sender.DisposeAsync();
    await client.DisposeAsync();
}