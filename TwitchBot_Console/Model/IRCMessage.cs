using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TwitchBot_Console.Model
{
    
    public class IRCMessage
    {
        public string Username { get; private set; }
        public string Message { get; private set; }
        public string Channel { get; private set; }
        public string OriginalMessage { get; private set; }

        public IRCMessage(string msg)
        {
            // Retrieve all the important parts of the message.
            Message = msg.Split(':').Last();
            Regex userReg = new Regex(@"![\w]+@");
            Regex channelReg = new Regex(@"#[\w]+");
            Match m = userReg.Match(msg);
            Match mc = channelReg.Match(msg);
            if (m.Success)
            {
                StringBuilder sb = new StringBuilder(m.ToString()).Remove(0, 1);
                sb.Remove(sb.Length - 1, 1);
                Username = sb.ToString();
            }
            if (mc.Success)
            {
                Channel = mc.ToString().Remove(0, 1);
            }

            OriginalMessage = msg;

        }

        public IRCMessage(string username, string channel, string message)
        {
            Username = username;
            Channel = channel;
            Message = message;

            OriginalMessage = ":" + username + "!" + username + "@" + username + "tmi.twitch.tv PRIVMSG #" + channel + " :" + message;
        }
    }
}
