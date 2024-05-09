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
        }

        public void Put(string str)
        {
            Console.Write(str);
        }

        public void PutLine(string str)
        {
            Console.WriteLine(str);
        }

        public void PutLine(char chr)
        {
            Console.WriteLine(chr);
        }
    }
}
