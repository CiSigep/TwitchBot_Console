using System;
using System.Collections.Generic;
using System.Text;
using TwitchBot_Console.Bot;

namespace TwitchBot_Console.Model
{
    class JoinChannelCommand : ICommand
    {
        private TwitchBot bot;
        public Level AccessLevel { get; set; }

        public JoinChannelCommand(TwitchBot bot, Level AccessLevel) {
            this.bot = bot;
            this.AccessLevel = AccessLevel;
        }
        public void execute(IRCMessage msg)
        {
            bot.joinChannel(msg.Username);
        }
    }
}
