using System;
using System.Runtime.ConstrainedExecution;

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
                        if (buffer.Length > 1)
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
                WinttOS.CommandManager._commandOutput += chr;
            }
        }

        public void Put(string str)
        {
            if (WinttOS.IsTty)
            {
                WinttOS.Tty.Write(str);
                WinttOS.CommandManager._commandOutput += str;
            }
        }

        public void PutLine(string str)
        {
            if(WinttOS.IsTty)
            {
                WinttOS.Tty.WriteLine(str);
                WinttOS.CommandManager._commandOutput += str + '\n';
            }
        }

        public void PutLine(char chr)
        {
            if(WinttOS.IsTty)
            {
                WinttOS.Tty.WriteLine(chr);
                WinttOS.CommandManager._commandOutput += chr + '\n';
            }
        }
    }
}
