using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asgdotnet_lib
{
    public class Log
    {
        static public void Info(string? message)
        {
            Console.WriteLine($"INFO: {message}");
        }
    }
}
