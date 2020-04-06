using System;
using System.Net;
using System.Net.Sockets;

namespace ClassTcpListener
{
    class Program
    {
        static int port = 5005;
        static IPAddress address = IPAddress.Loopback;

        static void Main(string[] args)
        {
            TcpListener listener = null;
            try
            {
                listener = new TcpListener(address, port);
                listener.Start();

                Console.WriteLine("Ожидание подключений... ");

                while (true)
                {
                    TcpClient client = listener.AcceptTcpClient();

                    string response = Console.ReadLine();
                    byte[] data = System.Text.Encoding.UTF8.GetBytes(response);

                    NetworkStream stream = client.GetStream();
                    stream.Write(data, 0, data.Length);
                    Console.WriteLine("Отправлено сообщение: {0}", response);

                    stream.Close();
                    client.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                if (listener != null)
                {
                    listener.Stop();
                    listener = null;
                }
            }
        }
    }
}
