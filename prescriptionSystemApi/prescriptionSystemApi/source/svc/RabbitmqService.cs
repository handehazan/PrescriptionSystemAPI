using Microsoft.EntityFrameworkCore.Metadata;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using IModel = RabbitMQ.Client.IModel;
namespace prescriptionSystemApi.source.svc
{
    public class RabbitmqService
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public RabbitmqService(IConfiguration configuration)
        {
            var factory = new ConnectionFactory
            {
                HostName = configuration["RabbitMQ:HostName"],
                UserName = configuration["RabbitMQ:UserName"],
                Password = configuration["RabbitMQ:Password"]
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

            _channel.BasicPublish(exchange: "",
                routingKey: queueName,
                basicProperties: null,
                body: body
            );
        }

        public void ConsumeMessage(string queueName, Action<string> processMessage)
        {
            _channel.QueueDeclare(queue: queueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, eventArgs) =>
            {
                var body = eventArgs.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                processMessage(message);
                _channel.BasicAck(eventArgs.DeliveryTag, false);
            };

            _channel.BasicConsume(queue: queueName,
                autoAck: false,
                consumer: consumer
            );
        }

        public void Dispose()
        {
            _channel?.Close();
            _connection?.Close();
        }


    }
}
