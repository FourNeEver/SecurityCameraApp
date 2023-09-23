using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Microsoft.Maui.Controls;
using static System.Net.Mime.MediaTypeNames;
using System.Text.Json;

namespace SecurityCameraApp.Sevices
{
    public static class WebHandlingService
    {
        static Socket socket;
        static HttpClient client;
        static IPHostEntry hostEntry;
        static IPEndPoint Ip;
        public static bool Connected = false;
        static byte[] buffer;

        public static async Task<string> StartListeningAsync(string Adress, int Port)
        {
            string con = "Connected";
            var timeout = new CancellationTokenSource();
            timeout.CancelAfter(4000);

            try {
                IPEndPoint ipe = new IPEndPoint(IPAddress.Parse(Adress), Port);
                socket = new Socket(ipe.AddressFamily,SocketType.Stream, ProtocolType.Tcp);
                await socket.ConnectAsync(ipe,timeout.Token);
                if (socket.Connected) { Connected = true; }
            }
            catch (SocketException ex) {
                con = $"Connection Interrpt: {ex.Message}";
            }
            catch (OperationCanceledException ex) {
                con = "Connection Timeout";
            }
            catch (FormatException ex)
            {
                con = "Invalid Adress";
            }

            return con;
            
        }

        public static async Task<string> FetchData()
        {

            var buffer = new byte[1024*1000];
            string test = "";
            try { 
                await socket.ReceiveAsync(buffer);
                Console.WriteLine(buffer.ToString());
                test = Encoding.UTF8.GetString(buffer);

            }
            catch(SocketException ex)
            {
                Connected = false;
                buffer = null;
                test = $"Connection interrupt: {ex.Message}";
            }


            return test;
        }

        public static async Task<Tuple<ImageSource,string>> FetchImage(byte[] buffer)
        {
            string test = "";
            ImageSource img = null;
            try
            {
                await socket.ReceiveAsync(buffer);
                if(buffer.Length > 0) {
                    MemoryStream ms = new MemoryStream(buffer);
                    img = ImageSource.FromStream(() => { return ms; });
                    ms.Flush();
                    test = "Live Feed";
                }
                
                
            }
            catch (SocketException ex)
            {
                Connected = false;
                test = $"Connection interrupt: {ex.Message}";
                img = "nofeed.png";
            }
            ;
            return Tuple.Create(img,test);
        }


        public static async Task PostControlData(string data)
        {
            var json = JsonSerializer.Serialize(data);
            try
            {
                await client.PostAsync(Ip + "/controlData", new StringContent(json, Encoding.UTF8, "application/json"));

            }
            catch (TimeoutException)
            {
                Connected = false;
            }
            catch (TaskCanceledException)
            {
                Connected = false;
            }

        }
        public static void checkConnectrion()
        {
            Connected = socket.Connected;
        }

    }
}
