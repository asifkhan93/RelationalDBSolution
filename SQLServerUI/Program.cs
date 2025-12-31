using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System;
using System.IO;
internal class Program
{
    private static void Main(string[] args)
    {
        Console.ReadLine();
    }

    private static string GetConnectionString(string connectionStringName = "Default")
    {
        string output = string.Empty;

        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json");

        var config = builder.Build();
        output = config.GetConnectionString(connectionStringName);

        return output;
    }
}
