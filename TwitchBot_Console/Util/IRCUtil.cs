using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Sockets;
using TwitchBot_Console.Model;

namespace TwitchBot_Console.Util
{
    
    class IRCUtil : IDisposable
    {
        private TcpClient client;
        private StreamReader reader;
        private StreamWriter writer;

        private string username;
        //private string channel;

        public IRCUtil(string ip, int port, string username, string oAuth)
        {
            this.username = username;
            client = new TcpClient(ip, port);
            reader = new StreamReader(client.GetStream());
            writer = new StreamWriter(client.GetStream());

            writer.WriteLine("PASS oauth:" + oAuth);
            writer.WriteLine("NICK " + username);
            writer.WriteLine("USER " + username + " 8 * :" + username);
            writer.Flush();
            writer.WriteLine("CAP REQ :twitch.tv/tags");
            writer.Flush();
            reader.ReadLine();
            reader.ReadLine();
        }

        public void join(string name)
        {
            //channel = name;
            writer.WriteLine("JOIN #" + name);
            writer.Flush();
        }

        public void leave(string name)
        {
            writer.WriteLine("PART #" + name);
            writer.Flush();
            //channel = null;
        }

        public IRCMessage receive() {
            string msg = reader.ReadLine();
            if (msg.Contains(@"PING: tmi.twitch.tv"))
                pong();
            return new IRCMessage(msg);
        }

        private void pong() {
            writer.WriteLine("PONG :tmi.twitch.tv");
            writer.Flush();
        }

        //public void send(string msg)
        public void send(string channel, string msg)
        {
            sendIRC(":" + username + "!" + username + "@" + username + "tmi.twitch.tv PRIVMSG #" + channel + " :" + msg);
        }

        private void sendIRC(string formattedMsg)
        {
            writer.WriteLine(formattedMsg);
            writer.Flush();
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    reader.Close();
                    writer.Close();
                    client.Close();
                }
                


                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion


    }
}
