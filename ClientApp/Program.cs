using System;
using System.Net.Http;
using System.Threading;



namespace SistemskoProjekat1
{
    class Program
    {
        static void Main(string[] args)
        {
            // Lista fajlova
            List<string> fileNames = new List<string> { 
                "Lorem.txt",
                "Lorem.bin",
                "nijeFajl",
                "nePostoji.txt",
                "Lorem.txt",
                "Lorem.bin",
                "nePostoji.txt",
                "nesto.txt" 
            };

            Thread[] threads = new Thread[fileNames.Count];

            int i = 0;
            // Kreiranje i pokretanje niti za svaki grad
            foreach (string fileName in fileNames)
            {

                threads[i] = new Thread(() => SendRequest(fileName));
                threads[i].Start();
                i++;
            }

            // Čekanje da se sve niti završe

            foreach(Thread thread in threads)
            {
                thread.Join();
            }
           
        }

        static void SendRequest(string fileName)
        {
            string url = $"http://localhost:5050/{fileName}"; // Dodavanje imena grada u URL

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = client.GetAsync(url).Result;
                    response.EnsureSuccessStatusCode(); // Provera da li je odgovor uspešan

                    string responseBody = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine($"Odgovor servera za fajl {fileName}:");
                    Console.WriteLine(responseBody);
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine($"Došlo je do greške prilikom slanja zahteva za fajl {fileName}: {ex.Message}");
                }
            }
        }

    }
}