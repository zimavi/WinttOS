using System;

namespace WinttOS.wSystem.IO
{
    public class TtyIO : IIn, IOut
    {
        public string Get()
        {
            return Get(false);
        }

        public string Get(bool hide)
        {
            string output = "";
            string buffer = "";
            bool isReturning = false;
            while (true)
            {
                var key = Console.ReadKey();
                switch (key.Key)
                {
                    case ConsoleKey.Backspace:
                        buffer = buffer[..^1]; 
                        break;
                    case ConsoleKey.Enter:
                        isReturning = true;
                        break;
                    default:
                        buffer += key.KeyChar;
                        break;
                }

                if(!hide)
                {
                    WinttOS.Tty.X -= output.Length;
                    WinttOS.Tty.Write(buffer);
                }

                output = buffer;

                if (isReturning)
                    break;
            }

            return output;
        }

        public ConsoleKeyInfo GetChr()
        {
            return GetChr(false);
        }

        public ConsoleKeyInfo GetChr(bool hide)
        {
            var key = Console.ReadKey();

            if (!hide)
            {
                WinttOS.Tty.Write(key.KeyChar);
            }

            return key;
        }

        public void Put(char chr)
        {
            if(WinttOS.IsTty)
            {
                WinttOS.Tty.Write(chr);
            }
        }

        public void Put(string str)
        {
            for(int i = 0; i < str.Length; i++)
            {
                Put(str[i]);
            }
        }

        public void PutLine(string str)
        {
            if(WinttOS.IsTty)
            {
                WinttOS.Tty.WriteLine(str);
            }
        }

        public void PutLine(char chr)
        {
            if(WinttOS.IsTty)
            {
                WinttOS.Tty.WriteLine(chr);
            }
        }
    }
}
