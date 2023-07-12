using System;
using System.Net.Sockets;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Runtime.Serialization.Json;

namespace WpfApp
{
    public class TCPClient
    {
        public string message;
        public bool changesFlag;
        public string Name { get; set; }
        public bool connectFlag;

        public TCPClient(string name)
        {
            Name = name;
        }

        public string ConnectTo(string message)
        {
            try
            {
                connectFlag = true;
                string ip = "127.0.0.1";
                int port = 666;

                TcpClient tcpClient = new TcpClient(ip, port); //данные не синхронизируются с сервером
                NetworkStream networkStream = tcpClient.GetStream();

                Console.WriteLine("connected");

                byte[] bytes = Encoding.UTF8.GetBytes(message);
                networkStream.WriteAsync(bytes, 0, bytes.Length);

                if (message == "get")
                {
                    Console.WriteLine("Starting to receive a file");

                    using (networkStream)
                    using (var output = File.Create("p.json"))
                    {
                        var buffer = new byte[1024];
                        int bytesRead;

                        while ((bytesRead = networkStream.Read(buffer, 0, buffer.Length)) > 0) output.Write(buffer, 0, bytesRead);

                        networkStream.Flush();
                        tcpClient.Close();

                        return output.Name;
                    }
                }
                else
                {
                    byte[] data = File.ReadAllBytes("DEMO_p.json");

                    int bufferSize = 1024;

                    byte[] dataLength = BitConverter.GetBytes(data.Length);

                    networkStream.Write(dataLength, 0, 4);

                    int bytesSent = 0;
                    int bytesLeft = data.Length;

                    while (bytesLeft > 0)
                    {
                        int curDataSize = Math.Min(bufferSize, bytesLeft);

                        networkStream.Write(data, bytesSent, curDataSize);

                        bytesSent += curDataSize;
                        bytesLeft -= curDataSize;
                    }

                    networkStream.Flush();

                    var buffer = new byte[512];
                    var bytes2 = networkStream.Read(buffer, 0, buffer.Length);
                    string result = Encoding.ASCII.GetString(buffer, 0, bytes2);

                    tcpClient.Close();

                    return result;
                }
            }
            catch (Exception)
            {
                connectFlag = false;
                Console.WriteLine("disconnected");

                if (message == "get")
                {
                    return "pCash.json";
                }
                else
                {
                    DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(List<UserInfo>));
                    MemoryStream memoryStream = new MemoryStream();

                    jsonSerializer.WriteObject(memoryStream, RegistrationPage.usersDemo);
                    memoryStream.Seek(0, SeekOrigin.Begin);

                    StreamReader reader = new StreamReader(memoryStream);

                    string json = reader.ReadToEnd();

                    memoryStream.Close();
                    reader.Close();

                    File.WriteAllText(@"pAutonome.json", json);
                    return "Все данные/изменения записаны в промежуточный файл и будут записаны после проверки в рабочем режиме.";
                }
            }
        }

        public void UpdatesCheck(List<TCPClient> clients)
        {
            //var u = clients[0].changesFlag;
            //var j = clients[0].Name;
            if (clients.Count > 1) 
            {
                var t = clients[1].changesFlag;
                var f = clients[1].Name;
            }
            
            if (clients.Any(c => c.changesFlag /*&& c.Name != Name*/) && connectFlag)
            {
                UpdateMessage();
            }
        }

        public void UpdateMessage()
        {
            MessageBox.Show("Данные были обновлены.");
        }
    }
}