using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace SendRequests
{
    class Program
    {
        static void Main(string[] args)
        {
            //Для загрузки файлов с определенных ресурсов предназначен класс WebClient,
            // который находится в пространстве имен System.Net.

            // Самый простой способ загрузки предоставляет метод DownloadFile().

            WebClient client = new WebClient();
            client.DownloadFile("http://minisat.se/downloads/escar05.pdf", "MiniSat.pdf");
            Console.WriteLine("Файл загружен...");
            Console.WriteLine();

            // Метод DownloadFile() принимает два параметра. Первый параметр указывает на файл в интернете,
            // который надо загрузить, а второй параметр определяет имя загруженного файла на локальном компьютере.
            // После загрузки в папке с приложением появится файл Совершенный_код.pdf.


            //******************************************************************************************************************************************//


            // WebClient также позволяет получить файл в качестве потока
            // и затем манипулировать этим потоком в процессе загрузки.

            using (Stream stream = client.OpenRead("https://www.w3.org/TR/PNG/iso_8859-1.txt"))
            {
                using (var sr = new StreamReader(stream))
                {
                    var line = "";
                    while ((line = sr.ReadLine()) != null)
                    {
                        Console.WriteLine(line);
                    }
                }
            }

            // Для открытия потока используется метод OpenRead(), в который передается адрес файла.
            // Для чтения потока применяется класс StreamReader. Затем прочитанные строки выводятся на консоль.


            //******************************************************************************************************************************************//


            // Загрузку файла можно осуществлять в асинхронном режиме
            DownloadFileAsync().GetAwaiter();


            //******************************************************************************************************************************************//


            WedRequestResponse();
            HttpRequestResponse();


            //SendDatainRequestPost();
            //SendDatainRequestGet();


            //******************************************************************************************************************************************//



            Console.ReadKey();
        }

        private static async Task DownloadFileAsync()
        {
            WebClient client = new WebClient();
            await client.DownloadFileTaskAsync(new Uri("https://www.w3.org/TR/PNG/iso_8859-1.txt"), "text.txt");
            Console.WriteLine("Файл txt загружен...");
            Console.WriteLine();
        }


        private static void WedRequestResponse()
        {
            WebRequest request = WebRequest.Create("https://www.w3.org/TR/PNG/iso_8859-1.txt");
            WebResponse response = request.GetResponse();

            using (Stream stream = response.GetResponseStream())
            {
                using (StreamReader sr = new StreamReader(stream))
                {
                    var line = "";
                    var n = 0;
                    while (n < 5)
                    {
                        Console.WriteLine(sr.ReadLine());
                        n++;
                    }
                }
            }

            response.Close();
            Console.WriteLine("Запрос выполнен...\n");
        }

        private static async Task HttpRequestResponse()
        {
            // Для получения информации, специфичной для протокола HTTP, используются два класса:
            // HttpWebRequest и HttpWebResponse, которые наследуются соответственно от WebRequest и WebResponse.

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://www.w3.org/TR/PNG/iso_8859-1.txt");
            HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync();

            using (Stream stream = response.GetResponseStream())
            {
                using (StreamReader sr = new StreamReader(stream))
                {
                    var line = "";
                    var n = 0;
                    while (n < 5)
                    {
                        Console.WriteLine(sr.ReadLine());
                        n++;
                    }
                }
            }

            Console.WriteLine("Запрос выполнен...\n");


            //==========================================================================//


            // Получим, к примеру, заголовки ответа.
            WebHeaderCollection headerCollection = response.Headers;
            for (int i = 0; i < headerCollection.Count; i++)
            {
                Console.WriteLine("{0}: {1}", headerCollection.GetKey(i), headerCollection[i]);
            }

            response.Close();

            Console.WriteLine();


            //==========================================================================//


            // Установка логина и пароля в запросе. В данном случае используется базовая HTTP-аутентификация.
            HttpWebRequest request1 = (HttpWebRequest)WebRequest.Create("http://somesite.com/auth");
            request1.Credentials = new NetworkCredential("login", "password");
            //HttpWebResponse response1 = (HttpWebResponse) await request1.GetResponseAsync();

            //response1.Close();
        }


        private static async Task SendDatainRequestPost()
        {
            WebRequest request = WebRequest.Create("http://localhost:5374/Home/PostData");
            request.Method = "POST"; // для отправки используется метод Post

            // данные для отправки
            string data = "sName=Иван Иванов&age=31";
            // преобразуем данные в массив байтов   
            byte[] dataBytes = System.Text.Encoding.Default.GetBytes(data);

            // устанавливаем тип содержимого - параметр ContentType
            request.ContentType = "application/x-www-form-urlencoded";
            // Устанавливаем заголовок Content-Length запроса - свойство ContentLength
            request.ContentLength = dataBytes.Length;

            //записываем данные в поток запроса 
            using (Stream stream = request.GetRequestStream())
            {
                stream.Write(dataBytes, 0, dataBytes.Length);
            }

            WebResponse response = await request.GetResponseAsync();

            using (Stream stream1 = response.GetResponseStream())
            {
                using (StreamReader sr = new StreamReader(stream1))
                {
                    Console.WriteLine(sr.ReadToEnd());
                }
            }
            response.Close();
            Console.WriteLine("Запрос выполнен...");
        }

        private static async Task SendDatainRequestGet()
        {
            WebRequest request = WebRequest.Create("http://localhost:5374/Home/PostData?sName=ИванИванов&age=31");
            WebResponse response = await request.GetResponseAsync();

            using (Stream stream = response.GetResponseStream())
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    Console.WriteLine(reader.ReadToEnd());
                }
            }
            response.Close();
        }
    }
}
