using System;
using System.Collections.Generic;
using System.Configuration;
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
        public Level AccessLevel { get; private set; }

        public IRCMessage(string msg)
        {
            // Retrieve all the important parts of the message.
            string[] msgTokens = msg.Split(':');
            Message = msgTokens.Last();
            Regex userReg = new Regex(@"![\w]+@");
            Regex channelReg = new Regex(@"#[\w]+", RegexOptions.RightToLeft);
            Match m = userReg.Match(msgTokens[1]);
            Match mc = channelReg.Match(msgTokens[1]);
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


            // Set Message Access Level
            AccessLevel = Level.LEVEL_EVERYONE;

            if(Username != null) {
                if (ConfigurationManager.AppSettings["admin"].ToLower().Equals(Username.ToLower())) {
                    AccessLevel = Level.LEVEL_ADMIN;
                    return;
                }

                Regex badgeRegex = new Regex(@"badges=[\w/,-_]*;");
                string badgeSection = badgeRegex.Match(msgTokens[0]).ToString();

                if (badgeSection.Contains("broadcaster"))
                    AccessLevel = Level.LEVEL_BROADCASTER;
                else if (badgeSection.Contains("moderator"))
                    AccessLevel = Level.LEVEL_MODERATOR;
                else if (badgeSection.Contains("subscriber"))
                    AccessLevel = Level.LEVEL_SUBSCRIBER;

            }
        }

    }
}
