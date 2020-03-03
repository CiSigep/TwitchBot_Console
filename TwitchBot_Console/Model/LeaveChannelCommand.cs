using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using TwitchBot_Console.Bot;

namespace TwitchBot_Console.Model
{
    class LeaveChannelCommand : ICommand
    {
        private TwitchBot bot;
        public Level AccessLevel { get; set; }
        public LeaveChannelCommand(TwitchBot bot, Level AccessLevel)
        {
            this.bot = bot;
            this.AccessLevel = AccessLevel;
        }

        public void execute(IRCMessage msg)
        {
            if(msg.Username.Equals(msg.Channel) && !msg.Channel.ToLower().Equals(ConfigurationManager.AppSettings["admin"].ToLower()))
                bot.leaveChannel(msg.Channel);
        }
    }
}
