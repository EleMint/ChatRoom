using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class Client
    {
        TcpClient clientSocket;
        NetworkStream stream;
        public Client(string IP, int port)
        {
            clientSocket = new TcpClient();
            clientSocket.Connect(IPAddress.Parse(IP), port);
            stream = clientSocket.GetStream();
        }
        public void Send()
        {
            while(true)
            {
                Task<string> messageString = Task.Run(() => UI.GetInput());
                messageString.Wait();
                Task<string>[] currentMessage = new Task<string>[] { messageString };
                string currentStringMessage = currentMessage[0].Result;
                byte[] message = Encoding.ASCII.GetBytes(currentStringMessage);
                stream.Write(message, 0, message.Count());
            }
        }
        public void Recieve()
        {
            while(true)
            {
                byte[] recievedMessage = new byte[256];
                Task incoming = Task.Run(() => stream.Read(recievedMessage, 0, recievedMessage.Length));
                incoming.Wait();
                UI.DisplayMessage(Encoding.ASCII.GetString(recievedMessage));
            }
        }
    }
}
