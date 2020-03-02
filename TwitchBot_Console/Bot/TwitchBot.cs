using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchBot_Console.Model;
using TwitchBot_Console.Util;

namespace TwitchBot_Console.Bot
{ 
    public class TwitchBot
    {
        private Dictionary<string, CommandDictionary> channels;
        private CommandDictionary globalCommands;
        private IRCUtil irc;
        private string botUsername;
        public bool Live { get; set; }

        public TwitchBot(string username, string oAuth)
        {
            irc = new IRCUtil("irc.twitch.tv", 6667, username, oAuth);
            this.botUsername = username;
            channels = new Dictionary<string, CommandDictionary>();
            Live = true;
            globalCommands = new CommandDictionary("#GLOBAL#", irc);
            globalCommands["shutdown"] = new ShutdownCommand(this);
            botUsername = username;
        }

        public bool joinChannel(string channelName)
        {
            // Twitch IRC channels are all lowercase
            channelName = channelName.ToLower();
            try
            {
                channels.Add(channelName, new CommandDictionary(channelName, irc));
                irc.join(channelName);
            }
            catch(Exception e)
            {
                return false;
            }

            return true;
        }

        public bool leaveChannel(string channelName)
        {
            try
            {
                channels.Remove(channelName);
                irc.leave(channelName);
            }
            catch(Exception e)
            {
                return false;
            }

            return true;
        }

        public void doWork()
        {
            joinChannel(ConfigurationManager.AppSettings["admin"]);

            while (Live)
            {
                IRCMessage msg = irc.receive();
                if (msg.Username != null && !msg.Username.Equals(botUsername))
                {
                    // Temporary if for testing purposes. Name is removed in GitHub Syncs
                    //TODO: remove all username references for git commits
                    if (msg.Username.ToLower().Equals(ConfigurationManager.AppSettings["admin"].ToLower()) && msg.Message.StartsWith("!"))
                        handleCommand(msg);

                }
            }
        }

        private void handleCommand(IRCMessage msg)
        {
            string command = msg.Message.Split(null)[0].Remove(0, 1);

            // Check the global commands before going for channel commands
            ICommand cmd = globalCommands[command];
            if(cmd == null)
            {
                cmd = channels[msg.Channel][command];
            }

            if (cmd != null)
                cmd.execute(msg.Channel);
             
        }

    }
}
