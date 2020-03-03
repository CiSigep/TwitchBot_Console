using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchBot_Console.Util;

namespace TwitchBot_Console.Model
{
    class ShutdownCommand : ICommand
    {
        public Level AccessLevel { get; set; }

        private Bot.TwitchBot bot;
        public void execute(IRCMessage msg)
        {
            // Make the doWork Thread end.
            bot.Live = false;
        }

        public ShutdownCommand(Bot.TwitchBot bot)
        {
            this.bot = bot;
            AccessLevel = Level.LEVEL_ADMIN;
        }
    }
}
