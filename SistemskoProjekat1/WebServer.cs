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
        private readonly Func<HttpListenerRequest, string> _responderMethod;


        public WebServer(Func<HttpListenerRequest, string> responderMethod, params string[] prefixes)
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
                            HttpListenerContext context = _listener.GetContext();
                            HttpListenerRequest request = context.Request;
                            HttpListenerResponse response = context.Response;
                            try
                            {
                                if (context == null)
                                {
                                    return;
                                }
                                //Console.WriteLine($"Thread ID: {Thread.CurrentThread.ManagedThreadId}");
                                //Thread.Sleep(5000);

                                if (request.RawUrl == "/favicon.ico")
                                {
                                    return;
                                }

                                string rstr = _responderMethod(request);
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
                                    response.OutputStream.Close();
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
