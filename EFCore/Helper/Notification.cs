using System.Text;
using RabbitMQ.Client;

namespace EFCore.Helper;

public class Notification
{
    private ConnectionFactory connectionFactory;

    public Notification()
    {
        connectionFactory = new ConnectionFactory();
    }
    
    
    public async Task AddMessageToQueueAsync(string message)
    {
        connectionFactory.Uri = new Uri("amqp://guest:guest@localhost:5672/");

        connectionFactory.ClientProvidedName = "Movie Service";


        var connection = await connectionFactory.CreateConnectionAsync();

        var channel = await connection.CreateChannelAsync();

        string exchange = "MovieApiExchange";
        string routingKey = "movie-api-routing-key";
        string queueName = "movie-api-queue";

        await channel.ExchangeDeclareAsync(exchange, ExchangeType.Direct);
        await channel.QueueDeclareAsync(queueName, false, false, false, null);
        await channel.QueueBindAsync(queueName, exchange, routingKey, null);

        byte[] messageBytes = Encoding.UTF8.GetBytes(message);

        var properties = new BasicProperties();

        await channel.BasicPublishAsync(exchange, routingKey, true, properties, messageBytes, CancellationToken.None);

        await channel.CloseAsync();
        await connection.CloseAsync();


    }
}