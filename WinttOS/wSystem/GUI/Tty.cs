using Cosmos.System.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Security.AccessControl;
using WinttOS.Core;
using WinttOS.Core.Utils.Debugging;
using WinttOS.wSystem.Benchmark;

namespace WinttOS.wSystem.GUI
{
    public struct Cell
    {
        public char Char;
        public uint Foreground;
        public uint Background;
    }
    public class Tty
    {
        #region Constants

        private const char _newLine = '\n';
        private const char _carriage = '\r';
        private const char _tab = '\t';
        private const char _space = ' ';

        #endregion

        #region Private Members

        private uint[] _pallete = new uint[16];
        private Cell[] _text;
        private List<Cell[]> _ttyHistory;
        private int _ttyHistoryIdx = 0;

        private uint _foreground = (byte)ConsoleColor.White;
        private uint _background = (byte)ConsoleColor.Black;

        private Canvas _canvas;

        #endregion

        #region Public Members & Fields

        public bool ScrollMode = false;
        public bool CursorVisible;

        public int X = 0;
        public int Y = 0;

        public int Cols = 0;
        public int Rows = 0;

        public Color ForegroundColor = Color.White;
        public ConsoleColor Foreground
        {
            get { return (ConsoleColor)_foreground; }
            set
            {
                _foreground = (uint)value;

                uint color = _pallete[_foreground];
                byte r = (byte)(color >> 16 & 0xFF);
                byte g = (byte)(color >> 8 & 0xFF);
                byte b = (byte)(color & 0xFF);

                ForegroundColor = Color.FromArgb(0xFF, r, g, b);
            }
        }

        public Color BackgroundColor = Color.Black;
        public ConsoleColor Background
        {
            get { return (ConsoleColor)_background; }
            set
            {
                _background = (uint)value;

                uint color = _pallete[_background];
                byte r = (byte)(color >> 16 & 0xFF);
                byte g = (byte)(color >> 8 & 0xFF);
                byte b = (byte)(color & 0xFF);

                BackgroundColor = Color.FromArgb(0xFF, r, g, b);
            }
        }

        #endregion

        #region Constructor & Methods


        public void Update()
        {
            _canvas.Clear(Color.Black);

            for(int i = 0; i < Rows; i++)
            {
                for(int j = 0; j < Cols; j++)
                {
                    int idx = GetIndex(i, j);
                    if (_text[idx].Char == 0)
                        continue;

                    if (_text[idx].Char == '\n')
                        break;

                    _canvas.DrawChar(_text[idx].Char, GlobalData.FallbackFont,
                        Color.FromArgb((int)_text[idx].Foreground), 0 + j * GlobalData.FallbackFont.Width,
                        0 + i * GlobalData.FallbackFont.Height);
                }
            }


            _canvas.Display();
        }

        public Tty(uint x, uint y)
        {
            _canvas = FullScreenCanvas.GetFullScreenCanvas(new Mode(x, y, ColorDepth.ColorDepth32));

            _pallete[0] = 0xFF000000; // Black
            _pallete[1] = 0xFF0000AB; // Darkblue
            _pallete[2] = 0xFF008000; // DarkGreen
            _pallete[3] = 0xFF008080; // DarkCyan
            _pallete[4] = 0xFF800000; // DarkRed
            _pallete[5] = 0xFF800080; // DarkMagenta
            _pallete[6] = 0xFF808000; // DarkYellow
            _pallete[7] = 0xFFC0C0C0; // Gray
            _pallete[8] = 0xFF808080; // DarkGray
            _pallete[9] = 0xFF5353FF; // Blue
            _pallete[10] = 0xFF55FF55; // Green
            _pallete[11] = 0xFF00FFFF; // Cyan
            _pallete[12] = 0xFFAA0000; // Red
            _pallete[13] = 0xFFFF00FF; // Magenta
            _pallete[14] = 0xFFFFFF55; // Yellow
            _pallete[15] = 0xFFFFFFFF; //White

            // Init

            Cols = (int)x / GlobalData.FallbackFont.Width - 1;
            Rows = (int)y / GlobalData.FallbackFont.Height - 1;

            _text = new Cell[Cols * Rows];

            ClearText();

            CursorVisible = true;
            _ttyHistory = new List<Cell[]>();

            X = 0;
            Y = 0;
        }

        private int GetIndex(int row, int column) => row * Cols + column;

        public void SetCursorPos(int x, int y)
        {
            if (CursorVisible)
            {
                _canvas.DrawFilledRectangle(ForegroundColor, 0 + x * GlobalData.FallbackFont.Width,
                    0 + y * GlobalData.FallbackFont.Height + GlobalData.FallbackFont.Height, 8, 4);
            }
        }

        public void ClearText()
        {
            _canvas.Clear(Color.Black);

            X = 0;
            Y = 0;

            for(int i = 0; i < _text.Length; i++)
            {
                _text[i].Char = (char)0;
                _text[i].Foreground = (uint)ForegroundColor.ToArgb();
                _text[i].Background = (uint)BackgroundColor.ToArgb();
            }
            Update();
        }

        public void DrawCursor() => SetCursorPos(X, Y);

        private void NextLine()
        {
            Y++;
            X = 0;
            if (Y == Rows)
            {
                Scroll();
                Y--;
            }
        }

        private void Scroll()
        {
            _canvas.Clear(Color.Black);

            Cell[] lineToHistory = new Cell[Cols];
            Array.Copy(_text, 0, lineToHistory, 0, Cols);

            _ttyHistory.Add(lineToHistory);

            Array.Copy(_text, Cols, _text, 0, (Rows - 1) * Cols);

            int startIdx = (Rows - 1) * Cols;
            for(int i = startIdx; i < startIdx + Cols; i++)
            {
                _text[i].Char = (char)0;
                _text[i].Foreground = (uint)ForegroundColor.ToArgb();
                _text[i].Background = (uint)BackgroundColor.ToArgb();
            }

            _ttyHistoryIdx = _ttyHistory.Count;

            Update();
        }

        public void ScrollUp()
        {
            if (_ttyHistoryIdx > 0)
            {
                ScrollMode = true;

                _ttyHistoryIdx--;

                Array.Copy(_text, 0, _text, Cols, (Rows - 1) * Cols);

                Cell[] lineFromHistory = _ttyHistory[_ttyHistoryIdx];
                Array.Copy(lineFromHistory, 0, _text, 0, Cols);
                Update();
            }
        }

        public void ScrollDown()
        {
            _ttyHistoryIdx = 0;

            _ttyHistory.Clear();

            ScrollMode = false;

            ClearText();
            X = 0;
            Y = 0;
            Update();
        }

        private void DoCarriage() => X = 0;

        private void DoTab()
        {
            Write(_space);
            Write(_space);
            Write(_space);
            Write(_space);
        }

        public void Write(char @char)
        {
            int idx = GetIndex(Y, X);
            _text[idx] = new Cell()
            {
                Char = @char,
                Foreground = (uint)ForegroundColor.ToArgb(),
                Background = (uint)BackgroundColor.ToArgb(),
            };

            X++;
            if (X == Cols)
                NextLine();

            Update();
        }

        private void WriteNoUpdate(char @char)
        {
            int idx = GetIndex(Y, X);
            _text[idx] = new Cell()
            {
                Char = @char,
                Foreground = (uint)ForegroundColor.ToArgb(),
                Background = (uint)BackgroundColor.ToArgb(),
            };

            X++;
            if (X == Cols)
                NextLine();
        }

        public void Write(string text)
        {
            for(int i = 0; i < text.Length; i++)
            {
                switch(text[i])
                {
                    case _newLine:
                        NextLine();
                        break;
                    case _carriage:
                        DoCarriage();
                        break;
                    case _tab:
                        DoTab();
                        break;

                    default:
                        WriteNoUpdate(text[i]); 
                        break;
                }
            }

            Update();
        }

        public void Write(uint value) => Write(value.ToString());
        public void Write(ulong value) => Write(value.ToString());
        public void WriteLine(char chr) => Write(new string(new char[] { chr, _newLine }));
        public void WriteLine(string text) => Write(text + _newLine);

        #endregion
    }
}
