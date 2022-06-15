using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FarmasiCase.Service.RabbitMQ
{
    public static class QueueFactory
    {
        public static void SendMessageToExchange(string exchangeName,   // Name of the Producer message
                                                 string exchangeType,   // Method of moving data from producer to Queues
                                                 string queueName,      // Send the queue name incase exchangeName doesn't exist
                                                 object obj)
        {
            var channel = CreateBasicConsumer()                                     // First, create an Exchange
                                        .EnsureExchange(exchangeName, exchangeType) // If Exchange doesn't exist, create and bring one
                                        .EnsureQueue(queueName, exchangeName)       // If Queue doesn't exist, create and bring one
                                        .Model;                                     // Bring the Channel so we can send the obj

            // It asks for a byte array, We have an object. We Serialize with Json first and then turn it into byte[].
            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(obj));

            channel.BasicPublish(exchangeName, queueName, null, body);
        }

        public static EventingBasicConsumer CreateBasicConsumer()
        {
            var factory = new ConnectionFactory() { HostName = "localhost", };
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            return new EventingBasicConsumer(channel);
        }

        public static EventingBasicConsumer EnsureExchange(this EventingBasicConsumer consumer,
                                                           string exchangeName,
                                                           string exchangeType = "direct")
        {
            consumer.Model.ExchangeDeclare(exchangeName, exchangeType, false, false);
            return consumer;
        }

        public static EventingBasicConsumer EnsureQueue(this EventingBasicConsumer consumer,
                                                   string queueName,
                                                   string exchangeName)
        {
            consumer.Model.QueueDeclare(queueName, false, false, false, null);
            consumer.Model.QueueBind(queueName, exchangeName, queueName);
            return consumer;
        }
    }
}
