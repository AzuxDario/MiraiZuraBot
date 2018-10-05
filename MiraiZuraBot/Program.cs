using Microsoft.EntityFrameworkCore;
using MiraiZuraBot.Core;
using MiraiZuraBot.Database;
using System;

namespace MiraiZuraBot
{
    class Program
    {
        static void Main(string[] args)
        {
            new Bot().Run();
            while (Console.ReadLine() != "quit") ;
        }
    }
}
