using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

namespace InputService
{
    public class Program
    {
        private const string PathToWatchConfigName = "FolderToWatch";
        private const string FileFilterConfigName = "FileFilter";
        private const string QueueNameConfigName = "QueueName";

        private static IConfiguration _configuration;

        static void Main(string[] args)
        {
            _configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", false, true)
                .Build();

            string pathToWatch = _configuration.GetSection(PathToWatchConfigName).Value;

            Console.WriteLine("Input service.");
            Console.WriteLine($"Watching directory: {pathToWatch}");

            Directory.CreateDirectory(pathToWatch);
            using var watcher = new FileSystemWatcher(pathToWatch);

            watcher.NotifyFilter = NotifyFilters.Attributes
                                   | NotifyFilters.CreationTime
                                   | NotifyFilters.DirectoryName
                                   | NotifyFilters.FileName
                                   | NotifyFilters.LastAccess
                                   | NotifyFilters.LastWrite
                                   | NotifyFilters.Size;

            watcher.Created += OnCreated;

            watcher.Filter = _configuration.GetSection(FileFilterConfigName).Value;
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
            const string fileNameHeader = "name";
            var queueName = _configuration.GetSection(QueueNameConfigName).Value;

            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queueName,
                    true,
                    false,
                    false,
                    null);

                var body = File.ReadAllBytes(path);

                var properties = channel.CreateBasicProperties();
                properties.Persistent = true;
                properties.Headers = new Dictionary<string, object>()
                {
                    {fileNameHeader, Path.GetFileName(path)}
                };

                channel.BasicPublish("",
                    queueName,
                    properties,
                    body);
                Console.WriteLine($"{Path.GetFileName(path)} sent.");
            }
        }
    }
}
