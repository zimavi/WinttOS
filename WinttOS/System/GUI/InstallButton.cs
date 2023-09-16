using WinttOS.Core.Utils.GUI;
using Bitmap = Cosmos.System.Graphics.Bitmap;
using System;

namespace WinttOS.System.GUI
{
    public class InstallNextButton : OSButton
    {

        public InstallNextButton(uint x, uint y, Bitmap image) :
            base(x, y, image, false)
        { }

        public override void onButtonPressed()
        {
            throw new NotImplementedException("Used not implemented button!");
        }
    }
}
