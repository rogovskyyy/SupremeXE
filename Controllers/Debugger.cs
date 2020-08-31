using System;
using System.Drawing;
using Console = Colorful.Console;

namespace SupremeXE_UI
{
    public class Debugger
    {
        public static void Log(string message)
        {
            Console.Write($"[{DateTime.Now.ToString("HH:mm:ss")}] | ", Color.LightBlue);
            Console.Write($"{message} \n\n", Color.LightGreen);
        }   
    }
}