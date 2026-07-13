using Microsoft.Extensions.Configuration;
using Azure.Messaging.ServiceBus;

string busConnectionString = new ConfigurationBuilder().AddUserSecrets<Program>().Build()["ServiceBusConnectionString"];
const string? seriveBusQueueName = "servicebus-queue";

//creating required objects
ServiceBusClient serviceBusClient;
ServiceBusReceiver serviceBusReceiver;

//client to access the service bus and send messages to the queue
var client = new ServiceBusClient(busConnectionString);
//the sender object is used to send messages to the queue
var receiver = client.CreateReceiver(seriveBusQueueName);

//try displaying the messages in the queue
try
{
    var messages = await receiver.ReceiveMessagesAsync(maxMessages: 10);

    foreach (var message in messages)
    {
        Console.WriteLine($"Message received: {message.Body}");
        await receiver.CompleteMessageAsync(message);
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Exception occured while receiving the message batch: {ex.Message}");
}
finally
{
    //dispose the objects
    await receiver.DisposeAsync();
    await client.DisposeAsync();
}