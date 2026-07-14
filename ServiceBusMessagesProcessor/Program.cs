using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;

var serviceBusConnectionString = new ConfigurationBuilder().AddUserSecrets<Program>().Build()["ServiceBusConnectionString"];

//get the objects
var client  = new ServiceBusClient(serviceBusConnectionString);
var processor = client.CreateProcessor("servicebus-queue", new ServiceBusProcessorOptions());

//successfull message processing event handler
async Task ProcessMesaages(ProcessMessageEventArgs args)
{
    //collect message
    string message = args.Message.Body.ToString();

    //act with message
    Console.WriteLine($"Message received: {message}");

    //set the message as completed/processed
    await args.CompleteMessageAsync(args.Message);
}

//error message processing event handler
async Task ProcessError(ProcessErrorEventArgs args)
{
    Console.WriteLine($"Exception occured while receiving the message batch: {args.Exception.Message}");
}

try
{
    processor.ProcessMessageAsync += ProcessMesaages;
    processor.ProcessErrorAsync += ProcessError;

    //wait for messages
    await processor.StartProcessingAsync();
    Console.WriteLine("Press any key to stop the processing");
    Console.ReadKey();

    //stopping the processing
    Console.WriteLine("Stopping the processing");
    await processor.StopProcessingAsync();
    Console.WriteLine("Processing stopped");
}
catch (Exception ex)
{
    Console.WriteLine($"Exception occured while receiving the message batch: {ex.Message}");
}
finally
{
    //dispose the objects
    await processor.DisposeAsync();
    await client.DisposeAsync();
}