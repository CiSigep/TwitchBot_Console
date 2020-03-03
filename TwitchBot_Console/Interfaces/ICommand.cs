using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchBot_Console.Util;

namespace TwitchBot_Console.Model
{
    enum Level { LEVEL_ADMIN, LEVEL_BROADCASTER, LEVEL_MODERATOR, LEVEL_SUBSCRIBER, LEVEL_EVERYONE }
    interface ICommand
    {

        // Shows who can run the command.
        Level AccessLevel { get; set; }

        void execute(IRCMessage msg);

    }
}
