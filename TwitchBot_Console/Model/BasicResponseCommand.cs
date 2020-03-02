using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchBot_Console.Util;

namespace TwitchBot_Console.Model
{
    class BasicResponseCommand : ICommand
    {
        public string Response { get; set; }
        public Level AccessLevel { get; set; }

        private IRCUtil util;
        public BasicResponseCommand(IRCUtil util, string response, Level accessLevel)
        {
            Response = response;
            AccessLevel = accessLevel;
            this.util = util;
        }
        public virtual void execute(string channel)
        {
            util.send(channel, Response);
        }

       
    }
}
