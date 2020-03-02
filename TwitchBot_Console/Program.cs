using System;
using System.Configuration;
using TwitchBot_Console.Bot;

namespace TwitchBot_Console
{
    class Program
    {
        static void Main(string[] args)
        {
            string username = ConfigurationManager.AppSettings["username"];
            string token = ConfigurationManager.AppSettings["oauth"];

            TwitchBot t = new TwitchBot(username, token);

            t.doWork();
        }
    }
}
