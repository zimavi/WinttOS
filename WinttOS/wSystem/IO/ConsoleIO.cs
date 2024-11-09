using System;

namespace WinttOS.wSystem.IO
{
    public class ConsoleIO : IIn, IOut
    {
        public string Get()
        {
            return Console.ReadLine();
        }

        public string Get(bool hide)
        {
            return Get();
        }

        public ConsoleKeyInfo GetChr()
        {
            return Console.ReadKey();
        }

        public ConsoleKeyInfo GetChr(bool hide)
        {
            return Console.ReadKey(hide);
        }

        public void Put(char chr)
        {
            Console.Write(chr);
            //WinttOS.CommandManager._commandOutput += chr;
        }

        public void Put(string str)
        {
            Console.Write(str);
            //WinttOS.CommandManager._commandOutput += str;
        }

        public void PutLine(string str)
        {
            Console.WriteLine(str);
            //WinttOS.CommandManager._commandOutput += str + '\n';
        }

        public void PutLine(char chr)
        {
            Console.WriteLine(chr);
            //WinttOS.CommandManager._commandOutput += chr + '\n';
        }
    }
}
