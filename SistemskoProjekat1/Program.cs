using SistemskoProjekat1.Services;
using System.Net;

namespace SistemskoProjekat1
{
    public class Program
    {
        private static void Main(string[] args)
        {
            ConversionService conversionService = new ConversionService("C:\\Documents\\Fax\\6 semestar\\Sistemsko programiranje\\root");

            var ws = new WebServer(conversionService.GetRequest, "http://localhost:5050/");
            //var ws = new WebServer("http://localhost:5050/", conversionService.GetRequest);
            ws.Run();
            Console.WriteLine("A simple webserver. Press a key to quit.");
            Console.ReadKey();
            ws.Stop();
        }
    }
}