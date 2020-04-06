using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace TCPonSockets_Server
{
    class Program
    {
        static int port = 8005;
        static IPAddress address = IPAddress.Loopback;

        static void Main(string[] args)
        {
            IPEndPoint endPoint = new IPEndPoint(address, port);
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                socket.Bind(endPoint);
                socket.Listen(10);

                Console.WriteLine("Сервер запущен. Ожидание подключений...");

                while (true)
                {
                    Socket clientSocket = socket.Accept();

                    int bytes = 0;
                    byte[] data = new byte[256];
                    StringBuilder builder = new StringBuilder();

                    do
                    {
                        bytes = clientSocket.Receive(data);
                        builder.Append(Encoding.UTF8.GetString(data, 0, bytes));
                    }
                    while (clientSocket.Available > 0);
                    Console.WriteLine("Клиент: " + builder.ToString());

                    string response = "Сообщение получено сервером.";
                    data = new byte[64];
                    data = Encoding.UTF8.GetBytes(response);
                    clientSocket.Send(data);

                    clientSocket.Shutdown(SocketShutdown.Both);
                    clientSocket.Close();
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
