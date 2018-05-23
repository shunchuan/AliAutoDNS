using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CaoSCAutoDNS.Methods
{
    public class Log
    {
        public static void ConsoleWrite(string message)
        {
            Console.WriteLine("【当前时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "】 " + message);
        }

        public static void ConsoleWriteNoDate(string message)
        {
            Console.WriteLine(message);
        }
    }
}
