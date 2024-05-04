using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SistemskoProjekat1
{
    public class WebServer
    {
        private readonly HttpListener _listener = new HttpListener();
        private readonly Func<HttpListenerRequest, Cache.Cache, string> _responderMethod;
        private Cache.Cache cache;


        public WebServer(Func<HttpListenerRequest, Cache.Cache, string> responderMethod, int CacheCap, params string[] prefixes)
        {
            if (prefixes == null || prefixes.Length == 0)
            {
                throw new ArgumentException("URI ne sadrzi adekvatan broj parametara");
            }

            if (responderMethod == null)
            {
                throw new ArgumentException("Potreban je odgovarajuci responderMethod");
            }

            foreach (string prefix in prefixes)
            {
                _listener.Prefixes.Add(prefix);
            }

            cache = new Cache.Cache(CacheCap);

            _responderMethod = responderMethod;

            _listener.Start();
        }

        public void Run()
        {
            ThreadPool.QueueUserWorkItem(o =>
            {
                Console.WriteLine("Webserver running...");
                try
                {
                    while (_listener.IsListening)
                    {
                        ThreadPool.QueueUserWorkItem(c =>
                        {
                            //Console.WriteLine("pokrenuta nit");

                            //HttpListenerContext context = _listener.GetContext();
                            HttpListenerContext? context = c as HttpListenerContext;

                            try
                            {
                                if (context == null)
                                {
                                    return;
                                }

                                HttpListenerRequest request = context.Request;
                                HttpListenerResponse response = context.Response;
                                //Console.WriteLine($"Thread ID: {Thread.CurrentThread.ManagedThreadId}");
                                //Thread.Sleep(5000);

                                if (request.RawUrl == "/favicon.ico")
                                {
                                    return;
                                }

                                //Console.WriteLine(request.RawUrl);

                                string rstr = _responderMethod(request, this.cache);
                                byte[] buf = Encoding.UTF8.GetBytes(rstr);
                                response.ContentLength64 = buf.Length;
                                response.OutputStream.Write(buf, 0, buf.Length);


                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Error: {ex}");
                            }
                            finally
                            {
                                if (context != null)
                                {
                                    context.Response.OutputStream.Close();
                                }
                            }
                        }, _listener.GetContext());
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex}");
                }
            });
        }

        public void Stop()
        {
            _listener.Stop();
            _listener.Close();
        }


    }
}
