using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ClassTcpClient
{
    class Program
    {
        static int port = 5005;
        static IPAddress address = IPAddress.Loopback;

        static void Main(string[] args)
        {
            TcpClient client = new TcpClient();

            try
            {
                client.Connect(address, port);
                NetworkStream stream = client.GetStream();

                byte[] data = new byte[256];
                StringBuilder response = new StringBuilder();

                do
                {
                    int bytes = stream.Read(data);
                    response.Append(Encoding.UTF8.GetString(data, 0, bytes));
                }
                while (stream.DataAvailable);

                Console.WriteLine(response.ToString());

                stream.Close();
                client.Close();
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: {0}", e.Message);
            }

            Console.WriteLine("Запрос завершен...");
            Console.Read();
        }
    }
}
