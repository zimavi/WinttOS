using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinttOS.Base.commands
{
    public class loadUiCommand : Command
    {
        public loadUiCommand(string name) : base(name) { }

        public override string execute(string[] arguments)
        {
            /*
            GlobalData.ui = new Utils.UI();
            GlobalData.ui.InitializeUI();

            while(true)
            {
                GlobalData.ui._mouse.DrawCursor();
                GlobalData.ui._canvas.Display();
                GlobalData.ui._canvas.Clear(Color.Black);
            }
            */
            return "Not working :)";
        }
    }
}
