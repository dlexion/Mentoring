using System;
using System.IO;
using System.Text;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace CentralService
{
    public class Program
    {
        private const string PathToSaveConfigName = "SaveFolder";
        private const string QueueNameConfigName = "QueueName";

        private static IConfiguration _configuration;

        static void Main(string[] args)
        {
            Console.WriteLine("Central service");

            _configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", false, true)
                .Build();

            string pathToSave = _configuration.GetSection(PathToSaveConfigName).Value;
            string queueName = _configuration.GetSection(QueueNameConfigName).Value;

            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queueName,
                    true,
                    false,
                    false,
                    null);

                channel.BasicQos(0, 1, false);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (sender, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var name = Encoding.UTF8.GetString(ea.BasicProperties.Headers["name"] as byte[]);
                    Console.WriteLine($"{name} received");

                    Directory.CreateDirectory(pathToSave);
                    File.WriteAllBytes(Path.Combine(pathToSave, name), body);

                    ((EventingBasicConsumer)sender)?.Model.BasicAck(ea.DeliveryTag, false);
                    Console.WriteLine($"{name} saved to {Path.Combine(pathToSave, name)}");
                };
                channel.BasicConsume(queueName,
                    false,
                    consumer);

                Console.WriteLine("Press Enter to exit.");
                Console.ReadLine();
            }
        }
    }
}
