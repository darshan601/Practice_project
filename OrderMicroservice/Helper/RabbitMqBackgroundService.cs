using System.Text.Json;
using OrderMicroservice.Entities;

namespace OrderMicroservice.Helper;

public class RabbitMqBackgroundService: BackgroundService
{
    private readonly RabbitMqService rabbitMqService;
    private readonly ILogger<RabbitMqBackgroundService> logger;

    public RabbitMqBackgroundService(RabbitMqService rabbitMqService, ILogger<RabbitMqBackgroundService> logger)
    {
        this.rabbitMqService = rabbitMqService;
        this.logger = logger;
    }
    
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        rabbitMqService.StartListeningAsync(async messageJson =>
        {
            try
            {
                logger.LogInformation("Processing Message: {Message}", messageJson);

                Console.WriteLine($"[x] Message received => {messageJson}");

                var message = JsonSerializer.Deserialize<MovieMessage>(messageJson);

                switch (message!.MessageType)
                {
                    case "add-movie":
                        Console.WriteLine("[x] Adding Movie => "+message.Payload.ToString());
                        break;
                    case "update-movie":
                        Console.WriteLine("[x] Updating Movie => "+message.Payload.ToString());
                        break;
                    case "delete-movie":
                        Console.WriteLine("[x] Deleting Movie => "+message.Payload.ToString());
                        break;
                    default:
                        Console.WriteLine("[x] Unknown Message type => "+message.Payload.ToString());
                        break;
                }
                
                
                await Task.Delay(100);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error processing message");
            }
        });

        return Task.CompletedTask;

    }


    public override async Task StopAsync(CancellationToken token)
    {
        await rabbitMqService.DisposeAsync();
        await base.StopAsync(token);

    }
}

