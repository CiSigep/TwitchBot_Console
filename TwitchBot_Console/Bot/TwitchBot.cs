using NLog;
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
        private Logger _logger;
        public bool Live { get; set; }

        public TwitchBot(string username, string oAuth)
        {
            irc = new IRCUtil("irc.twitch.tv", 6667, username, oAuth);
            this.botUsername = username;
            channels = new Dictionary<string, CommandDictionary>();
            Live = true;
            globalCommands = new CommandDictionary("#GLOBAL#", irc);
            globalCommands["shutdown"] = new ShutdownCommand(this);
            globalCommands["leaveChannel"] = new LeaveChannelCommand(this, Level.LEVEL_MODERATOR);
            botUsername = username;

            this._logger = LogManager.GetCurrentClassLogger();
        }

        public bool joinChannel(string channelName)
        {
            // Twitch IRC channels are all lowercase
            channelName = channelName.ToLower();
            try
            {
                _logger.Info("Joining channel - " + channelName);
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
                _logger.Info("Leaving channel - " + channelName);
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
            
            // Grant users way to have bot join their channel.
            channels[ConfigurationManager.AppSettings["admin"]]["joinChannel"] = new JoinChannelCommand(this, Level.LEVEL_EVERYONE);

            while (Live)
            {
                IRCMessage msg = irc.receive();
                if (msg.Username != null && !msg.Username.Equals(botUsername))
                {
                    // Temporary if for testing purposes. Name is removed in GitHub Syncs
                    //TODO: remove all username references for git commits
                    if (msg.Message.StartsWith("!"))
                        handleCommand(msg);

                }
            }

            LogManager.Shutdown();
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
            {
                _logger.Info("Executing command " + command + " in channel " + msg.Channel);
                cmd.execute(msg);
            }
             
        }

    }
}
