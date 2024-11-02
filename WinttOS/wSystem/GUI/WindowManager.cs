using Cosmos.System.Graphics;
using Graphics = Cosmos.System.Graphics;
using System.Collections.Generic;
using System.Drawing;
using WinttOS.wSystem.GUI.Components;
using static System.Net.Mime.MediaTypeNames;

namespace WinttOS.wSystem.GUI
{
    public class WindowManager
    {
        private List<Window> _windows;
        private Window _focusedWindow;

        public static void DrawString(Canvas canvas, string str, Graphics.Fonts.Font font, Color color, int x, int y)
        {
            int length = str.Length;
            byte width = font.Width;
            byte height = font.Height;

            if (x + (width * str.Length) <= 0 || y + height <= 0)
                return;

            for (int i = 0; i < length; i++)
            {
                DrawChar(canvas, str[i], font, color, x, y);
                x += width;
            }
        }

        public static void DrawChar(Canvas canvas, char c, Graphics.Fonts.Font font, Color color, int x, int y)
        {
            byte height = font.Height;
            byte width = font.Width;

            if (x + width <= 0 || y + height <= 0)
                return;

            byte[] data = font.Data;
            int num = height * (byte)c;
            for (int i = 0; i < height; i++)
            {
                for (byte b = 0; b < width; b++)
                {
                    if (x + b < 0 || y + i < 0)
                        continue;
                    if (x + b > canvas.Mode.Width || y + i > canvas.Mode.Height)
                        continue;

                    if (font.ConvertByteToBitAddress(data[num + i], b + 1))
                    {
                        canvas.DrawPoint(color, (ushort)(x + b), (ushort)(y + i));
                    }
                }
            }
        }

        public static void DrawImage(Canvas canvas, Graphics.Image image, int x, int y)
        {

            for (int i = 0; i < image.Width; i++)
            {
                for (int j = 0; j < image.Height; j++)
                {
                    if (x + i < 0 || y + j < 0)
                        continue;
                    if (x + i > canvas.Mode.Width || y + j > canvas.Mode.Height)
                        continue;
                    Color color = Color.FromArgb(image.RawData[i + j * image.Width]);
                    canvas.DrawPoint(color, x + i, y + j);
                }
            }
        }

        public static void DrawImageAlpha(Canvas canvas, Graphics.Image image, int x, int y)
        {
            for (int i = 0; i < image.Width; i++)
            {
                for (int j = 0; j < image.Height; j++)
                {
                    if (x + i < 0 || y + j < 0)
                        continue;
                    if (x + i > canvas.Mode.Width || y + j > canvas.Mode.Height)
                        continue;
                    Color color = Color.FromArgb(image.RawData[i + j * image.Width]);
                    canvas.DrawPoint(color, x + i, y + j);
                }
            }
        }

        public List<Window> Windows
        {
            get
            {
                return _windows;
            }
        }

        public WindowManager()
        {
            _windows = new List<Window>();
            _focusedWindow = null;
        }

        public void AddWindow(Window window)
        {
            _windows.Add(window);
            SortWindowsByZOrder();
        }

        public void RemoveWindow(Window window)
        {
            window.OnWindowClosed?.Invoke();
            _windows.Remove(window);
            if(_focusedWindow == window)
                _focusedWindow = null;
        }

        public void BringToFront(Window window)
        {
            window.ZOrder = GetMaxZOrder() + 1;
            window.IsDirty = true;
            SortWindowsByZOrder();
        }

        public void FocusWindow(Window window)
        {
            if (_focusedWindow == window)
                return;

            if(_focusedWindow != null)
            {
                _focusedWindow.IsFocused = false;
            }

            _focusedWindow = window;
            _focusedWindow.IsFocused = true;
            BringToFront(window);
        }

        public void HandleMouseClick(int x, int y)
        {
            for (int i = _windows.Count - 1; i >= 0; i--)
            {
                if (_windows[i].ContainsPoint(x, y))
                {
                    FocusWindow(_windows[i]);
                    break;
                }
            }
        }

        public void RenderWindows(Canvas canvas)
        {
            foreach (var window in _windows)
            {
                if (window.IsDirty)
                {
                    window.Render(canvas, 0, 0);
                }
            }
        }

        public void ForceRenderWindows(Canvas canvas)
        {
            foreach (var window in _windows)
            {
                window.Render(canvas, 0, 0);
            }
        }

        private void SortWindowsByZOrder()
        {
            QuickSortZOrder(_windows, 0, _windows.Count - 1);
        }

        private void QuickSortZOrder(List<Window> windows, int low, int high)
        {
            if (low < high)
            {
                int pi = PartitionZOrder(windows, low, high);

                QuickSortZOrder(windows, low, pi - 1);
                QuickSortZOrder(windows, pi + 1, high);
            }
        }

        private int PartitionZOrder(List<Window> windows, int low, int high)
        {
            Window pivot = windows[high];

            int i = low - 1;

            for(int j = low; j <= high - 1; j++)
            {
                if (windows[j].ZOrder < pivot.ZOrder)
                {
                    i++;
                    Swap(windows, i, j);
                }
            }

            Swap(windows, i + 1, high);
            return i + 1;
        }

        private void Swap(List<Window> windows, int i, int j)
        {
            Window tmp = windows[i];
            windows[i] = windows[j];
            windows[j] = tmp;
        }

        private int GetMaxZOrder()
        {
            int maxZOrder = 0;
            foreach(var window in _windows)
            {
                if(window.ZOrder > maxZOrder)
                    maxZOrder = window.ZOrder;
            }
            return maxZOrder;
        }
    }
}
