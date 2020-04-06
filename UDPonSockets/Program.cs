using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace UDPonSockets
{
    class Program
    {
        static int localPort;  // порт приема сообщений
        static int remotePort; // порт для отправки сообщений
        static Socket listeningSocket;

        static void Main(string[] args)
        {
            Console.WriteLine("Введите порт приема:");
            localPort = int.Parse(Console.ReadLine());
            Console.WriteLine("Введите порт для отправки сообщений:");
            remotePort = int.Parse(Console.ReadLine());

            Console.WriteLine("Введите сообщение...");

            try
            {
                listeningSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                Task listenTask = new Task(Listen);
                listenTask.Start();           
                                
                while (true)
                {
                    EndPoint remoteIp = new IPEndPoint(IPAddress.Parse("127.0.0.1"), remotePort);

                    byte[] data = new byte[256];
                    data = Encoding.UTF8.GetBytes(Console.ReadLine());

                    listeningSocket.SendTo(data, remoteIp);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Close();
            }
        }

        static void Listen()
        {
            try
            {
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), localPort);
                listeningSocket.Bind(endPoint);

                while (true)
                {
                    EndPoint remoteIp = new IPEndPoint(IPAddress.Any, 0);

                    StringBuilder builder = new StringBuilder();
                    int bytes = 0;
                    byte[] data = new byte[256];

                    do
                    {
                        bytes = listeningSocket.ReceiveFrom(data, ref remoteIp);
                        builder.Append(Encoding.UTF8.GetString(data, 0, bytes));
                    }
                    while (listeningSocket.Available > 0);

                    IPEndPoint iPEndPoint = remoteIp as IPEndPoint;
                    Console.WriteLine("{0}:{1} - {2}", iPEndPoint.Address.ToString(), iPEndPoint.Port.ToString(), builder.ToString());
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Close();
            }
        }

        static void Close()
        {
            if (listeningSocket != null)
            {
                listeningSocket.Shutdown(SocketShutdown.Both);
                listeningSocket.Close();
                listeningSocket = null;
            }
        }
    }
}
