using System.Text;
using System.Text.Json;
using EFCore.Entity;
using RabbitMQ.Client;

namespace EFCore.Helper;

public class NotificationService:IAsyncDisposable
{
    private IConnection? _connection;
    private IChannel? _channel;
    private const string Exchange = "MovieApiExchange";
    private const string RoutingKey = "movie-api-routing-key";
    private const string QueueName = "movie-api-queue";

    public static async Task<NotificationService> CreateAsync()
    {
        var service = new NotificationService();
        await service.InitializeAsync();
        return service;
    }

    private async Task InitializeAsync()
    {
        var factory = new ConnectionFactory
        {
            Uri = new Uri("amqp://guest:guest@localhost:5672"),
            ClientProvidedName = "Movie Service"
        };

        _connection = await factory.CreateConnectionAsync();
        _channel = await _connection.CreateChannelAsync();

        await _channel.ExchangeDeclareAsync(Exchange, ExchangeType.Direct);
        await _channel.QueueDeclareAsync(QueueName, false, false, false, null);
        await _channel.QueueBindAsync(QueueName, Exchange, RoutingKey, null);
        
    }

    public async Task PublishAsync(string messageType, object payload)
    {
        try
        {
            var message = new MovieMessage
            {
                MessageType = messageType,
                Payload = payload
            };

            var json =JsonSerializer.Serialize(message);
            
            var body = Encoding.UTF8.GetBytes(json);
            var properties = new BasicProperties
            {
                Persistent = true,
                Headers = new Dictionary<string, object>
                {
                    { "MessageType", messageType }
                }!
            };
            

            await _channel.BasicPublishAsync(Exchange, RoutingKey, true, properties, body, CancellationToken.None);
            

        }
        catch (Exception e)
        {
            Console.WriteLine($"Error Message Sending Failed : {e.Message}");
        }
        
        
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