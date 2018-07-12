using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    class Server
    {
        public static Client client;
        TcpListener server;
        public Server()
        {
            server = new TcpListener(IPAddress.Parse("192.168.0.125"), 9999);
            server.Start();
        }
        public void Run() 
        {
            Task.Run(() => AcceptClient());
            
        }
        private void AcceptClient()
        {
            while (true)
            {
                TcpClient clientSocket = default(TcpClient);
                clientSocket = server.AcceptTcpClient();
                Console.WriteLine("Connected");
                NetworkStream stream = clientSocket.GetStream();
                client = new Client(stream, clientSocket);
                Task.Run(() => NewClientChat());
            }
        }
        private void Respond(string body)
        {
             client.Send(body);
        }
        private void NewClientChat()
        {
            Task<string> message = Task.Run(() => client.Recieve());
            message.Wait();
            Task<string>[] messages = new Task<string>[] { message };
            string Message = messages[0].Result;
            Respond(Message);
        }
    }
}
