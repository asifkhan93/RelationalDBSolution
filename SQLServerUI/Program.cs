using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System;
using System.IO;
using DataAccessLibrary;
internal class Program
{
    private static void Main(string[] args)
    {

        SqlCrud sql = new SqlCrud(GetConnectionString());

        //ReadAllContacts(sql);
        ReadContact(sql,1);

        Console.ReadLine();
    }

    private static void ReadAllContacts(SqlCrud sql)
    {
        var rows = sql.GetAllContacts();
        foreach (var row in rows)
        {
            Console.WriteLine($"{row.Id}: {row.FirstName} {row.LastName}");
        }
    }

    private static void ReadContact(SqlCrud sql, int ContactId)
    {
        var contact = sql.GetFullContactById(ContactId);

        Console.WriteLine($"{contact.BasicInfo.Id}: {contact.BasicInfo.FirstName} {contact.BasicInfo.LastName}");


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
