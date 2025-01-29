using Microsoft.EntityFrameworkCore.Metadata;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using IModel = RabbitMQ.Client.IModel;
namespace prescriptionSystemApi.source.svc
{
    public class RabbitmqService : IDisposable
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public RabbitmqService(IConfiguration configuration)
        {
            var factory = new ConnectionFactory
            {
                HostName = configuration["RabbitMQ:HostName"],
                UserName = configuration["RabbitMQ:UserName"],
                Password = configuration["RabbitMQ:Password"],
                VirtualHost = configuration["RabbitMQ:VirtualHost"],
                DispatchConsumersAsync = true, // Enables async consumers for better performance
                AutomaticRecoveryEnabled = true, // Enables auto-reconnect in case of failure
                NetworkRecoveryInterval = TimeSpan.FromSeconds(10), // Retry every 10 sec if disconnected
                Ssl = new SslOption
                {
                    Enabled = true, // Ensure SSL is enabled
                    ServerName = configuration["RabbitMQ:HostName"] // Set the server name
                }
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
        }
        public void PublishMessage(string queueName, string message)
        {
            _channel.QueueDeclare(queue: queueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );
            var body = Encoding.UTF8.GetBytes(message);
            var properties = _channel.CreateBasicProperties();
            properties.Persistent = true; // Ensures messages are saved to disk

            _channel.BasicPublish(exchange: "",
                routingKey: queueName,
                basicProperties: null,
                body: body
            );
            Console.WriteLine($"Message published to {queueName}: {message}");
        }

        public void ConsumeMessage(string queueName, Action<string> processMessage)
        {
            _channel.QueueDeclare(queue: queueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.Received += async (model, eventArgs) =>
            {
                var body = eventArgs.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                try
                {
                    Console.WriteLine($"Received message from {queueName}: {message}");
                    processMessage(message);
                    _channel.BasicAck(eventArgs.DeliveryTag, false);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error processing message: {ex.Message}");
                    // Optionally, reject the message and requeue it for later processing
                    _channel.BasicNack(eventArgs.DeliveryTag, false, true);
                }
            };

            _channel.BasicConsume(queue: queueName,
                autoAck: false,
                consumer: consumer
            );
        }

        public void Dispose()
        {
            if (_channel != null && _channel.IsOpen)
            {
                _channel.Close();
                _channel.Dispose();
            }

            if (_connection != null && _connection.IsOpen)
            {
                _connection.Close();
                _connection.Dispose();
            }

            Console.WriteLine("RabbitMQ connection closed.");
        }
    }
}
