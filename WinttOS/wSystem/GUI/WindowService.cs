using Cosmos.Core.Memory;
using Cosmos.HAL;
using Cosmos.System;
using Cosmos.System.Graphics;
using System.Collections.Generic;
using System.Drawing;
using WinttOS.wSystem.GUI.Components;
using WinttOS.wSystem.Services;

namespace WinttOS.wSystem.GUI
{
    public class WindowService : Service
    {

        private Canvas _screen;
        private Window _draggedWindow = null;
        private int _dragOffsetX;
        private int _dragOffsetY;
        private List<Button> _clickedButtons;

        private int _frameCount;
        private int _fps;
        private uint _lastTick;

        private int _lastMouseX;
        private int _lastMouseY;
        private bool _dirty;

        private int _lastCollectFrame;

        public WindowManager WindowManager;

        public WindowService() : base("windowd")
        { }

        public override void OnServiceTick()
        {
            _frameCount++;
            _lastCollectFrame++;

            int mouseX = (int)MouseManager.X;
            int mouseY = (int)MouseManager.Y;
            bool mouseMoved = mouseX != _lastMouseX || mouseY != _lastMouseY;
            _lastMouseX = mouseX;
            _lastMouseY = mouseY;

            if (MouseManager.MouseState == MouseState.Left && _draggedWindow != null)
            {
                _draggedWindow.X = mouseX - _dragOffsetX;
                _draggedWindow.Y = mouseY - _dragOffsetY;
                _draggedWindow.IsDirty = true;
                _dirty = true;
            }

            if (MouseManager.MouseState == MouseState.Left && _draggedWindow == null)
            {
                foreach (var window in WindowManager.Windows)
                {
                    if (window.IsVisable && window.IsTitleBarClicked(mouseX, mouseY))
                    {
                        _draggedWindow = window;
                        _dragOffsetX = mouseX - window.X;
                        _dragOffsetY = mouseY - window.Y;
                        WindowManager.FocusWindow(window);
                        _dirty = true;
                        break;
                    }
                    else if (window.IsVisable && window.ContainsPoint(mouseX, mouseY))
                    {
                        WindowManager.FocusWindow(window);
                        _dirty = true;
                        break;
                    }
                }
            }

            if(MouseManager.MouseState == MouseState.None)
            {
                _draggedWindow = null;
                foreach(Button button in _clickedButtons)
                {
                    button.HasClicked = false;
                }
                _clickedButtons.Clear();
            }

            if(MouseManager.MouseState == MouseState.Left)
            {
                foreach(var window in WindowManager.Windows)
                {
                    foreach(var component in window.Components)
                    {
                        if(component is Button button && !button.HasClicked && button.IsClicked(mouseX, mouseY, window.X, window.Y + Window.TITLEBAR_HIGHT))
                        {
                            _clickedButtons.Add(button);
                            button.HasClicked = true;
                            button.OnClick?.Invoke();
                            button.IsDirty = true;
                            window.IsDirty = true;
                            _dirty = true;
                        }
                    }
                }
            }

            if(_dirty || mouseMoved)
            {
                _screen.DrawImage(Files.Bg0, 0, 0);

                WindowManager.ForceRenderWindows(_screen);

                _screen.DrawString("FPS: " + _fps, Core.Utils.Sys.Files.Fonts.Font18, Color.White, 0, 0);

                WindowManager.DrawImageAlpha(_screen, Files.CursorImage, mouseX, mouseY);

                _screen.Display();

                _dirty = false;
            }

            

            uint currentTick = RTC.Second;
            if(currentTick != _lastTick)
            {
                _fps = _frameCount;
                _frameCount = 0;
                _lastTick = currentTick;
            }

            if(_lastCollectFrame > 20)
            {
                Heap.Collect();
            }

            if (MouseManager.MouseState == MouseState.Middle)
                WinttOS.ServiceManager.FinishService(ProcessName);
        }

        public override void OnServiceStart()
        {
            if (WinttOS.IsTty)
            {
                FullScreenCanvas.Disable();
                WinttOS.IsTty = false;
            }

            MouseManager.ScreenWidth = 1920;
            MouseManager.ScreenHeight = 1080;
            _screen = FullScreenCanvas.GetFullScreenCanvas(new Mode(1920, 1080, ColorDepth.ColorDepth32));

            WinttOS.ServiceManager.FinishService(WinttOS.CommandManager.ProcessName);
            SystemD.isWinMonRunning = true;

            WindowManager = new();
            _clickedButtons = new List<Button>();
            _dirty = true;
        }

        public override void OnServiceStop()
        {
            _screen.Disable();
            WinttOS.IsTty = true;
            WinttOS.Tty = new Tty(1920, 1080);

            WinttOS.ServiceManager.StartService(WinttOS.CommandManager.ProcessName);
            SystemD.isWinMonRunning = false;

            WinttOS.ProcessManager.TryRemoveDeadProcess(ProcessID);
            WinttOS.ServiceManager.RunServiceGC();
        }
    }
}
