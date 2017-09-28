using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace Listen.Api
{
    public class Program
    {
        public const string DatabaseName = "Listen";

        public static void Main(string[] args)
        {
            Console.WriteLine("AUTH_AUDIENCE: " + Environment.GetEnvironmentVariable("AUTH_AUDIENCE"));
            Console.WriteLine("AUTH_SCOPE:" + Environment.GetEnvironmentVariable("AUTH_SCOPE"));
            Console.WriteLine("AUTH_CLIENT_ID: " + Environment.GetEnvironmentVariable("AUTH_CLIENT_ID"));
            Console.WriteLine("AUTH_DOMAIN: " + Environment.GetEnvironmentVariable("AUTH_DOMAIN"));
            Console.WriteLine("DATABASE: " + Environment.GetEnvironmentVariable("DATABASE"));


            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .UseApplicationInsights()
                .UseUrls("http://*:7000")
                .Build();

            host.Run();
        }
    }
}
