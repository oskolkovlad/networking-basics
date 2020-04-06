using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace TCPonSockets_Client
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
                socket.Connect(endPoint);
                Console.Write("Введите сообщение: ");
                string message = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(message))
                    throw new ArgumentNullException(nameof(message), "Сообщение не может быть пустым.");

                byte[] data = new byte[64];
                data = Encoding.UTF8.GetBytes(message);
                socket.Send(data);

                int bytes = 0;
                data = new byte[256];
                StringBuilder builder = new StringBuilder();

                do
                {
                    bytes = socket.Receive(data);
                    builder.Append(Encoding.UTF8.GetString(data, 0, bytes));
                }
                while (socket.Available > 0);

                Console.WriteLine("Сервер: {0}", builder.ToString());

                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
