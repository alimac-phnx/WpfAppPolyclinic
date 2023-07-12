using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Windows;

namespace Wpf_server
{
    class Program
    {
        public static bool IsValid(UserInfo userInfo, List<ValidationResult> validationResults)
        {
            var context = new ValidationContext(userInfo);

            return Validator.TryValidateObject(userInfo, context, validationResults, true);
        }

        public static bool IsPatientExist(UserInfo userInfo)
        {
            var users = DataFile.DataFileContent("p.json");
            if (users.Any(u => u.Surname.ToLower() == userInfo.Surname.ToLower() && u.Name.ToLower() == userInfo.Name.ToLower() && 
            u.Secname.ToLower() == userInfo.Secname.ToLower() && u.Birthday == userInfo.Birthday)) return true;
            else return false;
        }

        private static MessageBoxResult VerificationMessage()
        {
            string disp = "This patient is already registered.\n\nВcё равно зарегистрировать?";
            string caption = "Registration";
            MessageBoxImage icon = MessageBoxImage.Question;
            MessageBoxButton button = MessageBoxButton.YesNo;
            return MessageBox.Show(disp, caption, button, icon);
        }

        private static string Validation(UserInfo userInfo, List<ValidationResult> results)
        {
            if (IsValid(userInfo, results))
            {
                Register(userInfo);
                return null;
            }
            else
            {
                string disp = string.Join("\n", results);
                return disp;
            }
        }

        public static string TryToRegister(UserInfo userInfo)
        {
            var results = new List<ValidationResult>();

            if (IsPatientExist(userInfo))
            {
                MessageBoxResult buttonresult = VerificationMessage();

                return buttonresult == MessageBoxResult.Yes ? Validation(userInfo, results) : "rejected";
            }
            else return Validation(userInfo, results);
        }

        public static string TryToSave(UserInfo editUser)
        {
            var results = new List<ValidationResult>();

            if (IsValid(editUser, results))
            {
                Save(editUser);
                return null;
            }
            else
            {
                string disp = string.Join("\n", results);
                return disp;
            }
        }

        public static void DeleteData(List<UserInfo> users, string surname, string phonenumber)
        {
            users.RemoveAll(b => b.Surname == surname && b.Phonenumber == phonenumber);
        }

        private static void Register(UserInfo userInfo) 
        {
            var users = DataFile.DataFileContent("p.json");
            users.Add(userInfo);
            Serialization(users);
        }

        public static void Save(UserInfo editUser)
        {
            var users = DataFile.DataFileContent("p.json");
            DeleteData(users, editUser.Surname, editUser.Phonenumber);
            users.Add(editUser);
            Serialization(users);
        }

        public static void Serialization(List<UserInfo> users)
        {
            DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(List<UserInfo>));
            MemoryStream memoryStream = new MemoryStream();

            jsonSerializer.WriteObject(memoryStream, users);
            memoryStream.Seek(0, SeekOrigin.Begin);

            StreamReader reader = new StreamReader(memoryStream);

            string json = reader.ReadToEnd();

            memoryStream.Close();
            reader.Close();

            File.WriteAllText(@"p.json", json);
        }

        public static void FileReceiving(NetworkStream stream, TcpClient tcpClient)
        {
            byte[] fileSizeBytes = new byte[4];
            stream.Read(fileSizeBytes, 0, 4);
            int dataLength = BitConverter.ToInt32(fileSizeBytes, 0);

            int bytesLeft = dataLength;
            byte[] data2 = new byte[dataLength];

            int bufferSize = 1024;
            int bytesRead = 0;

            while (bytesLeft > 0)
            {
                int curDataSize = Math.Min(bufferSize, bytesLeft);
                if (tcpClient.Available < curDataSize)
                    curDataSize = tcpClient.Available;

                stream.Read(data2, bytesRead, curDataSize);

                bytesRead += curDataSize;
                bytesLeft -= curDataSize;
            }

            File.WriteAllBytes("DEMO_p.json", data2);

            stream.Flush();
        }

        public static void AnalizeOperationResult(string result, NetworkStream stream, string operationDone)
        {
            if (result != null)
            {
                Console.WriteLine("Sending errors");

                var buffer3 = Encoding.UTF8.GetBytes(result);
                stream.WriteAsync(buffer3, 0, buffer3.Length);
            }
            else Console.WriteLine(operationDone);
        }

        static void Main(string[] args)
        {
            IPAddress localAdd = IPAddress.Parse("127.0.0.1");
            TcpListener tcpListener = new TcpListener(localAdd, 666);

            tcpListener.Start();

            while (true)
            {
                using TcpClient tcpClient = tcpListener.AcceptTcpClient();

                NetworkStream stream = tcpClient.GetStream();

                var buffer = new byte[512];
                int bytes = stream.Read(buffer, 0, buffer.Length);
                string command = Encoding.ASCII.GetString(buffer, 0, bytes);

                Console.WriteLine(command);

                UserInfo userInfo = null;
                var result = string.Empty;

                switch (command)
                {
                    case "get":
                        Console.WriteLine("SENDING FILE");

                        string fileName = "p.json";

                        StreamWriter sWriter = new StreamWriter(stream);

                        byte[] bytes1 = File.ReadAllBytes(fileName);

                        tcpClient.Client.SendFile(fileName);
                        Console.WriteLine("FIle sent");

                        tcpClient.Close();
                        stream.Flush();

                        break;
                    case "send to register":
                        FileReceiving(stream, tcpClient);
                        userInfo = DataFile.SingleDataFileContent("DEMO_p.json");
                        result = TryToRegister(userInfo);

                        AnalizeOperationResult(result, stream, "Patient registered");

                        tcpClient.Close();
                        stream.Flush();

                        break;
                    case "send to save":
                        FileReceiving(stream, tcpClient);
                        userInfo = DataFile.SingleDataFileContent("DEMO_p.json");
                        result = TryToSave(userInfo);

                        AnalizeOperationResult(result, stream, "File saved");

                        tcpClient.Close();
                        stream.Flush();

                        break;
                    default:
                        Console.WriteLine("DELETING DATA");

                        FileReceiving(stream, tcpClient);

                        var userInfoToDelete = DataFile.SingleDataFileContent("DEMO_p.json");
                        var users = DataFile.DataFileContent("p.json");

                        DeleteData(users, userInfoToDelete.Surname, userInfoToDelete.Phonenumber);
                        Serialization(users);

                        Console.WriteLine("Data was deleted");

                        tcpClient.Close();
                        stream.Flush();

                        break;
                }
            }
        }
    }
}
