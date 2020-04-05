using System;
using System.Net;

namespace Addresses
{
    class Program
    {
        static void Main(string[] args)
        {
            // Все ip-адреса представляют 32 - битное(протокол IPv4) или
            // 128 - битное значение(протокол IPv6), например, 31.170.165.181.
            // В системе классов.NET ip-адрес представлен классом IPAddress.

            IPAddress ip1 = IPAddress.Parse("127.0.0.1");
            IPAddress ip2 = IPAddress.Loopback;  // 127.0.0.1
            IPAddress ip3 = IPAddress.Any;       // 0.0.0.0
            IPAddress ip4 = IPAddress.Broadcast; // 255.255.255.255

            Console.WriteLine($"{ip1}\n{ip2}\n{ip3}\n{ip4}\n");



            // Также для получения адреса в сети используется класс IPHostEntry.
            // Он содержит информацию об определенном компьютере-хосте.

            // С помощью свойства HostName этот класс возвращает имя хоста, а с помощью свойства
            // AddressList - все ip - адреса хоста, так как один компьютер может иметь в сети несколько ip-адресов.

            // Для взаимодействия с dns-сервером и получения ip - адреса применяется класс Dns.Для получения
            // информации о хосте компьютера и его адресах у него имеется метод GetHostEntry()

            IPHostEntry hostEntry = Dns.GetHostEntry("www.google.com");

            Console.WriteLine(hostEntry.HostName);
            foreach(var ip in hostEntry.AddressList)
            {
                Console.WriteLine(ip);
            }



            Console.ReadKey();
        }
    }
}
