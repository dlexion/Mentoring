using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using RabbitMQ.Client;

namespace InputService
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Input service.");
            using var watcher = new FileSystemWatcher(@"D:\Mentoring\test");

            watcher.NotifyFilter = NotifyFilters.Attributes
                                   | NotifyFilters.CreationTime
                                   | NotifyFilters.DirectoryName
                                   | NotifyFilters.FileName
                                   | NotifyFilters.LastAccess
                                   | NotifyFilters.LastWrite
                                   | NotifyFilters.Size;

            watcher.Created += OnCreated;

            watcher.Filter = "*.txt";
            watcher.IncludeSubdirectories = true;
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
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare("messaging",
                    true,
                    false,
                    false,
                    null);

                var body = File.ReadAllBytes(path);

                var properties = channel.CreateBasicProperties();
                properties.Persistent = true;
                properties.Headers = new Dictionary<string, object>()
                {
                    {"name", Path.GetFileName(path)}
                };

                channel.BasicPublish("",
                    "messaging",
                    properties,
                    body);
                Console.WriteLine($"{Path.GetFileName(path)} sent.");
            }
        }
    }
}
