using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SistemskoProjekat1.Services
{
    internal class ConversionService
    {
        private string rootFolder;
        public ConversionService(string rootFolder)
        {
            this.rootFolder = rootFolder;
        }

        public string GetRequest(HttpListenerRequest request, Cache.Cache cache )
        {

            string url = request.RawUrl ?? "";

            Console.WriteLine("Primljen je zahtev: " + url);

            if(url == "/" || url=="")
            {
                Console.WriteLine("Nije zahtevan fajl");
                return "<HTML><BODY>Niste zahtevali fajl<br></BODY></HTML>";
            }
            string fileName = url.Split('/')[1];
            string path = Path.Combine(rootFolder, fileName);
            //Console.WriteLine(path);

            string? cacheData = cache.ReadCache(fileName);
            string data;

            if (cacheData != null){
                Console.WriteLine($"Citanje podataka iz kesa za fajl {fileName}");
                return cacheData;
            }
            else
            {
                if (File.Exists(path))
                {
                    if (path.EndsWith(".txt"))
                    {
                        Console.WriteLine("Konverzija txt u bin");

                        data = this.TransformTxtToBin(path);
                        cache.WriteToCache(fileName, data);
                        return data;
                    }
                    else if (path.EndsWith(".bin"))
                    {
                        Console.WriteLine("Konverzija bin u txt");
                        data = this.TransformBinToTxt(path);
                        cache.WriteToCache(fileName, data);
                        return data;
                    }
                    else
                    {
                        Console.WriteLine("Nije unet format fajla");
                        data = "<HTML><BODY>Nije unet format fajla<br></BODY></HTML>";
                        cache.WriteToCache(fileName, data);
                        return data;
                    }
                }
                else
                {
                    Console.WriteLine("Fajl se ne nalazi u folderu");
                    data = "<HTML><BODY>Ne postoji fajl u root folderu<br></BODY></HTML>";
                    cache.WriteToCache(fileName, data);
                    return data;
                }
            }

            

        }

        private string TransformTxtToBin(string path)
        {
            byte[] bytes = File.ReadAllBytes(path);
            //using (FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read))
            //{
            //    bytes = new byte[file.Length];
            //    file.Read(bytes, 0, (int)file.Length);
            //}
            //using (FileStream fs = new FileStream("C:\\Documents\\Fax\\6 semestar\\Sistemsko programiranje\\root\\lorem.bin", FileMode.Create))
            //{
            //    fs.Write(bytes, 0, bytes.Length);
            //}
            string data = BitConverter.ToString(bytes).Replace("-", " ");
            return $"<HTML><BODY>{data}<br></BODY></HTML>";
        }
        private string TransformBinToTxt(string path)
        {
            byte[] bytes = File.ReadAllBytes(path);
            string data = System.Text.Encoding.Default.GetString(bytes);
            return $"<HTML><BODY>{data}<br></BODY></HTML>";
        }

    }
}
