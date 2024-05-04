using SistemskoProjekat1.Services;
using System.Net;

namespace SistemskoProjekat1
{
    public class Program
    {
        private static void Main(string[] args)
        {
            ConversionService conversionService = new ConversionService("C:\\Documents_Fax\\6 semestar\\Sistemsko programiranje\\Projekti\\root");

            WebServer ws = new WebServer(conversionService.GetRequest, "http://localhost:5050/");

            ws.Run();
            Console.WriteLine("A simple webserver. Press a key to quit.");
            Console.ReadKey();
            ws.Stop();
        }
    }
}