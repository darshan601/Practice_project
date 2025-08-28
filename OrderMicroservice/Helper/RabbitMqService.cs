using System.Text;
using Microsoft.AspNetCore.Identity.UI.Services;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace OrderMicroservice.Helper;

public class RabbitMqService:IAsyncDisposable
{
    private IConnection? _connection;
    private IChannel? _channel;
    private const string Exchange = "MovieApiExchange";
    private const string RoutingKey = "movie-api-routing-key";
    private const string QueueName = "movie-api-queue";

    public static async Task<RabbitMqService> CreateAsync()
    {
        var service = new RabbitMqService();
        await service.InitializeAsync();
        return service;
    }

    private async Task InitializeAsync()
    {
        var factory = new ConnectionFactory
        {
            Uri = new Uri("amqp://guest:guest@localhost:5672"),
            ClientProvidedName = "Order Service"
        };
        
        _connection = await factory.CreateConnectionAsync();
        _channel = await _connection.CreateChannelAsync();
        
        // Declare Exchange and Queue Once
        await _channel.ExchangeDeclareAsync(Exchange, ExchangeType.Direct);
        await _channel.QueueDeclareAsync(QueueName, false, false, false, null);
        await _channel.QueueBindAsync(QueueName, Exchange, RoutingKey, null);
        await _channel.BasicQosAsync(0, 1, false);
    }

    public async Task StartListeningAsync(Func<string, Task> messageHandler)
    {
        if (_channel is null)
            throw new InvalidOperationException("Channel not initialized");

        var consumer = new AsyncEventingBasicConsumer(_channel);

        consumer.ReceivedAsync += async (sender, args) =>
        {
            try
            {
                var body = args.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($"[x] Message received: {message}");

                // process the message 
                await messageHandler(message);

                // acknowledge after successful processing
                await _channel.BasicAckAsync(args.DeliveryTag, false);
            }
            catch (Exception e)
            {
                Console.WriteLine($"[ERROR] Message processing failed: {e.Message}");
                await _channel.BasicNackAsync(args.DeliveryTag, false, true);

            }
        };

        var str = await _channel.BasicConsumeAsync(QueueName, false, consumer);
    }

    public async ValueTask DisposeAsync()
    {
        if (_channel is not null)
        {
            await _channel.CloseAsync();
            _channel.Dispose();
        }

        if (_connection is not null)
        {
            await _connection.CloseAsync();
            _connection.Dispose();
        }
        
        GC.SuppressFinalize(this);
    }
}