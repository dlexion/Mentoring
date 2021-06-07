using System;
using System.IO;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace CentralService
{
    class Program
    {
        static void Main(string[] args)
        {
            const string pathToSave = @"D:\Mentoring\messaging-central";

            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare("messaging",
                    true,
                    false,
                    false,
                    null);

                channel.BasicQos(0, 1, false);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (sender, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var name = ea.BasicProperties.Headers["name"].ToString();
                    Console.WriteLine($"{name} received");

                    Directory.CreateDirectory(pathToSave);
                    File.WriteAllBytes(Path.Combine(pathToSave, name), body);

                    ((EventingBasicConsumer)sender)?.Model.BasicAck(ea.DeliveryTag, false);
                    Console.WriteLine($"{name} saved");
                };
                channel.BasicConsume("messaging",
                    false,
                    consumer);

                Console.WriteLine("Press Enter to exit.");
                Console.ReadLine();
            }
        }
    }
}
