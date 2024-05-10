using System;

namespace WinttOS.wSystem.IO
{
    public interface IOut
    {
        public void Put(char chr);
        public void Put(string str);
        public void PutLine(string str);
        public void PutLine(char chr);
    }

    public interface IIn
    {
        public string Get();
        public string Get(bool hide);

        public ConsoleKeyInfo GetChr();
        public ConsoleKeyInfo GetChr(bool hide);
    }
}
