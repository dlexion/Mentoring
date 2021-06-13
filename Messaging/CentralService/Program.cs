using System;
using System.IO;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace CentralService
{
    public class Program
    {
        private static IConfiguration _configuration;
        private static ILogger _logger;
        private static string _pathToSave;

        static void Main(string[] args)
        {
            _configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", false, true)
                .Build();

            using var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder
                    .AddFilter("Default", LogLevel.Information)
                    .AddConsole();
            });
            _logger = loggerFactory.CreateLogger<Program>();

            _logger.LogInformation("Central service");

            _pathToSave = _configuration.GetSection("SaveFolder").Value;
            var queueName = _configuration.GetSection("QueueName").Value;

            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queueName, true, false, false, null);

                channel.BasicQos(0, 0, false);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += ReceiveFile;
                channel.BasicConsume(queueName, false, consumer);

                Console.WriteLine("Press Enter to exit.");
                Console.ReadLine();
            }
        }

        private static void ReceiveFile(object sender, BasicDeliverEventArgs ea)
        {
            var isChunk = (bool)ea.BasicProperties.Headers["isChunk"];
            var name = Encoding.UTF8.GetString((byte[])ea.BasicProperties.Headers["name"]);

            if (isChunk)
            {
                ReceiveFileInChunks(sender, ea, name);
            }
            else
            {
                ReceiveEntireFile(sender, ea, name);
            }
        }

        private static void ReceiveEntireFile(object sender, BasicDeliverEventArgs ea, string name)
        {
            _logger.LogInformation($"{name} received");
            var body = ea.Body.ToArray();

            Directory.CreateDirectory(_pathToSave);
            File.WriteAllBytes(Path.Combine(_pathToSave, name), body);
            _logger.LogInformation($"{name} saved to {_pathToSave}");

            ((EventingBasicConsumer)sender)?.Model.BasicAck(ea.DeliveryTag, false);
            _logger.LogInformation($"{name} saved to {Path.Combine(_pathToSave, name)}");
        }

        private static void ReceiveFileInChunks(object sender, BasicDeliverEventArgs ea, string name)
        {
            var chunkPosition = (int)ea.BasicProperties.Headers["chunkPosition"];
            var chunksCount = (long)ea.BasicProperties.Headers["chunksCount"];
            _logger.LogInformation($"{name} received chunk {chunkPosition + 1} out of {chunksCount}");

            var body = ea.Body.ToArray();
            Directory.CreateDirectory(_pathToSave);

            using (var stream = new FileStream(Path.Combine(_pathToSave, name), FileMode.Append))
            {
                stream.Write(body);
            }

            if (chunkPosition + 1 == chunksCount)
            {
                ((EventingBasicConsumer)sender)?.Model.BasicAck(ea.DeliveryTag, true);
                _logger.LogInformation($"{name} saved to {Path.Combine(_pathToSave, name)}");
            }
        }
    }
}
