using System;
using WinttOS.Core.Utils.Debugging;
using WinttOS.wSystem.GUI;
using WinttOS.wSystem.GUI.Components;

namespace WinttOS.wSystem.Shell.commands.Misc
{
    public class WinManStartCommand : Command
    {
        public WinManStartCommand(string[] commandValues) : base(commandValues)
        {}

        public override ReturnInfo Execute()
        {
            WindowService service = new();
            WinttOS.ServiceManager.AddService(service);
            WinttOS.ServiceManager.StartService(service.ProcessName);

            bool hasSpawed = false;

            Window another = new("Another window", 500, 500, 300, 100, 1);
            another.AddComponent(new Label(-100, -100, 100, 100, ":O"));

            Window window = new("Test window", 100, 100, 1000, 500, 1);
            Label label = new(0, 0, 100, 100, "Hello World!");

            string[] strs =
            {
                "Hello World!",
                "YAY!",
                "SpOwOky OwOky Pumpkin",
                "Marry chistmas",
                "This is working",
                "rm -fr /",
            };

            window.AddComponent(label);

            window.AddComponent(new Button(100, 100, 100, 35, "Press me!", () =>
            {
                Random rnd = new();

                label.Text = strs[rnd.Next(strs.Length)];

                if (!hasSpawed)
                {
                    service.WindowManager.AddWindow(another);
                    hasSpawed = true;
                }
            }));


            service.WindowManager.AddWindow(window);

            

            return new(this, ReturnCode.OK);
        }
    }
}
