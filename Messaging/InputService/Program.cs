using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

namespace InputService
{
    public class Program
    {
        private static IConfiguration _configuration;

        static void Main(string[] args)
        {
            _configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", false, true)
                .Build();

            string pathToWatch = _configuration.GetSection("FolderToWatch").Value;

            Console.WriteLine("Input service.");
            Console.WriteLine($"Watching directory: {pathToWatch}");

            Directory.CreateDirectory(pathToWatch);
            using var watcher = new FileSystemWatcher(pathToWatch);

            watcher.Created += OnCreated;

            watcher.Filter = _configuration.GetSection("FileFilter").Value;
            watcher.EnableRaisingEvents = true;

            Console.WriteLine("Press enter to exit.");
            Console.ReadLine();
        }

        private static void OnCreated(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine($"{e.Name} created.");
            SendFile(e.FullPath);
        }

        private static void SendFile(string path)
        {
            var queueName = _configuration.GetSection("QueueName").Value;
            var maxMessageSize = int.Parse(_configuration.GetSection("MaxMessageSize").Value);

            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queueName, true, false, false, null);

                if (new FileInfo(path).Length <= maxMessageSize)
                {
                    SendEntireFile(path, channel, queueName);
                }
                else
                {
                    SendFileInChunks(path, channel, maxMessageSize, queueName);
                }
                Console.WriteLine($"{Path.GetFileName(path)} sent.");

                File.Delete(path);
            }
        }

        private static IBasicProperties GetBasicProperties(string path, IModel channel)
        {
            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;
            properties.Headers = new Dictionary<string, object>()
            {
                {"name", Path.GetFileName(path)}
            };
            return properties;
        }

        private static void SendFileInChunks(string path, IModel channel, int maxMessageSize, string queueName)
        {
            byte[] buffer = new byte[maxMessageSize];
            int index = 0;

            using (Stream fs = File.OpenRead(path))
            {
                var chunksCount = (fs.Length + maxMessageSize - 1) / maxMessageSize;
                int read;
                while ((read = fs.Read(buffer, 0, buffer.Length)) > 0)
                {
                    var chunkProperties = GetBasicProperties(path, channel);
                    chunkProperties.Headers.Add("isChunk", true);
                    chunkProperties.Headers.Add("chunkPosition", index);
                    chunkProperties.Headers.Add("chunksCount", chunksCount);

                    channel.BasicPublish("", queueName, true, chunkProperties, new ReadOnlyMemory<byte>(buffer, 0, read));
                    index++;
                }
            }
        }

        private static void SendEntireFile(string path, IModel channel, string queueName)
        {
            Console.WriteLine("Sending entire file.");
            var basicProperties = GetBasicProperties(path, channel);
            basicProperties.Headers.Add("isChunk", false);
            var body = File.ReadAllBytes(path);
            channel.BasicPublish("", queueName, basicProperties, body);
        }
    }
}
