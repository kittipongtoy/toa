using System;
using System.IO;
using System.Net;
using System.Text;

namespace TOAMediaPlayer
{
    public class TOAHttpListener
    {
        public void HttpListenerCallback(IAsyncResult result)
        {
            HttpListener listener = (HttpListener)result.AsyncState;
            HttpListenerContext context = listener.EndGetContext(result);
            HttpListenerResponse Response = context.Response;

            String dateAsString = DateTime.Now.ToString(@"MM\/dd\/yyyy h\:mm tt");

            byte[] bOutput = System.Text.Encoding.UTF8.GetBytes(dateAsString);
            Response.ContentType = "application/json";
            Response.ContentLength64 = bOutput.Length;

            Stream OutputStream = Response.OutputStream;
            OutputStream.Write(bOutput, 0, bOutput.Length);
            OutputStream.Close();
        }

        public void GetPlayer()
        {
            HttpListener listener = new HttpListener();
            listener.Prefixes.Add("http://127.0.0.1:8081/");
            listener.Start();
            while (true)
            {
                HttpListenerContext context = listener.GetContext();
                HttpListenerRequest req = context.Request;

                Console.WriteLine($"Received request for {req.Url}");

                HttpListenerResponse resp = context.Response;
                resp.Headers.Set("Content-Type", "text/plain");

                string data = "Hello there!";
                byte[] buffer = Encoding.UTF8.GetBytes(data);
                resp.ContentLength64 = buffer.Length;

                Stream ros = resp.OutputStream;
                ros.Write(buffer, 0, buffer.Length);
            }
        }
    }
}
