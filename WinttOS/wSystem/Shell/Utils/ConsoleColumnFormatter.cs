using System;
using WinttOS.wSystem.IO;

namespace WinttOS.wSystem.Shell.Utils
{
    public sealed class ConsoleColumnFormatter
    {
        private byte columnWidth;
        private byte columnsInRow;

        private byte currentColumn = 0;

        public ConsoleColumnFormatter() : this(10, 3)
        { }

        public ConsoleColumnFormatter(byte columnWidth, byte columnsInRow)
        {
            this.columnWidth = columnWidth;
            this.columnsInRow = columnsInRow;
        }

        public void Write(string text)
        {
            if (text.Length > columnWidth)
                text = text[..(columnWidth - 1)];

            SystemIO.STDOUT.Put(text.PadRight(columnWidth, ' '));
            currentColumn++;
            checkForNewLine();
        }

        private void checkForNewLine()
        {
            if(currentColumn >= columnsInRow)
            {
                SystemIO.STDOUT.PutLine("");
                currentColumn = 0;
            }
        }
    }
}
