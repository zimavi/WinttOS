using Cosmos.System;
using System.Collections.Generic;

namespace WinttOS.wSystem.IO
{
    internal class UKRScanMap : ScanMapBase
    {
        public UKRScanMap()
        {
        }

        protected override void InitKeys()
        {

            /*
             * ONLY VALID FOR zap-ext psf fonts!
             *
             */
            Keys = new List<KeyMapping>(100)
            {
                new KeyMapping(0, ConsoleKeyEx.NoName),
                new KeyMapping(1, ConsoleKeyEx.Escape),
                new KeyMapping(2, '1', '!', '1', '1', '!', '1', ConsoleKeyEx.D1),
                new KeyMapping(3, '2', '"', '2', '2', '"', '2', ConsoleKeyEx.D2),
                new KeyMapping(4, '3', (char)0x105, '3', '3', (char)0x105, '3', ConsoleKeyEx.D3),
                new KeyMapping(5, '4', ';', '4', '4', ';', '4', ConsoleKeyEx.D4),
                new KeyMapping(6, '5', '%', '5', '5', '%', '5', ConsoleKeyEx.D5),
                new KeyMapping(7, '6', ':', '6', '6', ':', '6', ConsoleKeyEx.D6),
                new KeyMapping(8, '7', '?', '7', '7', '?', '7', ConsoleKeyEx.D7),
                new KeyMapping(9, '8', '*', '8', '8', '*', '8', ConsoleKeyEx.D8),
                new KeyMapping(10, '9', '(', '9', '9', '(', '9', ConsoleKeyEx.D9),
                new KeyMapping(11, '0', ')', '0', '0', ')', '0', ConsoleKeyEx.D0),
                new KeyMapping(12, '-', '_', '-', '-', '_', '-', ConsoleKeyEx.Minus),
                new KeyMapping(13, '=', '+', '=', '=', '+', '=', ConsoleKeyEx.Equal),
                new KeyMapping(14, ConsoleKeyEx.Backspace),
                new KeyMapping(15, '\t', ConsoleKeyEx.Tab),
                new KeyMapping(16, (char)0x12f, (char)0x10e, (char)0x12f, (char)0x10e, (char)0x12f, (char)0x10e, ConsoleKeyEx.Q),
                new KeyMapping(17, (char)0x137, (char)0x113, (char)0x137, (char)0x113, (char)0x137, (char)0x113, ConsoleKeyEx.W),
                new KeyMapping(18, 'y', (char)0x111, 'y', (char)0x111, 'y', (char)0x111, ConsoleKeyEx.E),
                new KeyMapping(19, (char)0x130, 'K', (char)0x130, 'K', (char)0x130, 'K', ConsoleKeyEx.R),
                new KeyMapping(20, 'e', 'E', 'e', 'E', 'e', 'E', ConsoleKeyEx.T),
                new KeyMapping(21, (char)0x133, 'H', (char)0x133, 'H', (char)0x133, 'H', ConsoleKeyEx.Y),
                new KeyMapping(22, (char)0x12a, (char)0x109, (char)0x12a, (char)0x109, (char)0x12a, '\0', (char)0x14a, (char)0x126, ConsoleKeyEx.U),
                new KeyMapping(23, (char)0x139, (char)0x115, (char)0x139, (char)0x115, (char)0x139, (char)0x115, ConsoleKeyEx.I),
                new KeyMapping(24, (char)0x13a, (char)0x116, (char)0x13a, (char)0x116, (char)0x13a, (char)0x116, ConsoleKeyEx.O),
                new KeyMapping(25, (char)0x12d, (char)0x10c, (char)0x12d, (char)0x10c, (char)0x12d, (char)0x10c, ConsoleKeyEx.P),
                new KeyMapping(26, 'x', 'X', 'x', 'X', 'x', 'X', ConsoleKeyEx.LBracket),
                new KeyMapping(27, (char)0x0ef, (char)0x08f, (char)0x0ef, (char)0x08f, (char)0x0ef, (char)0x08f, ConsoleKeyEx.RBracket),
                new KeyMapping(28, ConsoleKeyEx.Enter),
                new KeyMapping(29, ConsoleKeyEx.LCtrl),
                new KeyMapping(30, (char)0x136, (char)0x112, (char)0x136, (char)0x112, (char)0x136, (char)0x112, ConsoleKeyEx.A),
                new KeyMapping(31, 'i', 'I', 'i', 'I', 'i', 'I', ConsoleKeyEx.S),
                new KeyMapping(32, (char)0x129, 'B', (char)0x129, 'B', (char)0x129, 'B', ConsoleKeyEx.D),
                new KeyMapping(33, 'a', 'A', 'a', 'A', 'a', 'A', ConsoleKeyEx.F),
                new KeyMapping(34, (char)0x134, (char)0x110, (char)0x134, (char)0x110, (char)0x134, (char)0x110, ConsoleKeyEx.G),
                new KeyMapping(35, 'p', 'P', 'p', 'P', 'p', 'P', ConsoleKeyEx.H),
                new KeyMapping(36, 'o', 'O', 'o', 'O', 'o', 'O', ConsoleKeyEx.J),
                new KeyMapping(37, (char)0x131, (char)0x10f, (char)0x131, (char)0x10f, (char)0x131, (char)0x10f, ConsoleKeyEx.K),
                new KeyMapping(38, (char)0x12b, (char)0x10a, (char)0x12b, (char)0x10a, (char)0x12b, (char)0x10a, ConsoleKeyEx.L),
                new KeyMapping(39, (char)0x12c, (char)0x10b, (char)0x12c, (char)0x10b, (char)0x12c, (char)0x10b, ConsoleKeyEx.Semicolon),
                new KeyMapping(40, (char)0x141, (char)0x11d, (char)0x141, (char)0x11d, (char)0x141, (char)0x11d, ConsoleKeyEx.Apostrophe),
                new KeyMapping(41, '\'', (char)0x106, '\'', (char)0x106, '\'', (char)0x106, ConsoleKeyEx.Backquote),
                new KeyMapping(42, ConsoleKeyEx.LShift),
                new KeyMapping(43, '/', '\\', '/', '/', '\\', '\\', ConsoleKeyEx.Backslash),
                new KeyMapping(44, (char)0x140, (char)0x11c, (char)0x140, (char)0x11c, (char)0x140, (char)0x11c, ConsoleKeyEx.Z),
                new KeyMapping(45, (char)0x138, (char)0x114, (char)0x138, (char)0x114, (char)0x138, (char)0x114, ConsoleKeyEx.X),
                new KeyMapping(46, 'c', 'C', 'c', 'C', 'c', 'C', ConsoleKeyEx.C),
                new KeyMapping(47, (char)0x132, 'M', (char)0x132, 'M', (char)0x132, 'M', ConsoleKeyEx.V),
                new KeyMapping(48, (char)0x12e, (char)0x10d, (char)0x12e, (char)0x10d, (char)0x12e, (char)0x10d, ConsoleKeyEx.B),
                new KeyMapping(49, (char)0x135, 'T', (char)0x135, 'T', (char)0x135, 'T', ConsoleKeyEx.N),
                new KeyMapping(50, (char)0x13d, (char)0x119, (char)0x13d, (char)0x119, (char)0x13d, (char)0x119, ConsoleKeyEx.M),
                new KeyMapping(51, (char)0x128, (char)0x108, (char)0x128, (char)0x108, (char)0x128, (char)0x108, ConsoleKeyEx.Comma),
                new KeyMapping(52, (char)0x13f, (char)0x11b, (char)0x13f, (char)0x11b, (char)0x13f, (char)0x11b, ConsoleKeyEx.Period),
                new KeyMapping(53, '.', ',', '.', ',', '.', ',', ConsoleKeyEx.Slash),
                new KeyMapping(54, ConsoleKeyEx.RShift),
                new KeyMapping(55, '*', '*', '*', '*', '*', '*', ConsoleKeyEx.NumMultiply),
                new KeyMapping(56, ConsoleKeyEx.LAlt),
                new KeyMapping(57, ' ', ConsoleKeyEx.Spacebar),
                new KeyMapping(58, ConsoleKeyEx.CapsLock),
                new KeyMapping(59, ConsoleKeyEx.F1),
                new KeyMapping(60, ConsoleKeyEx.F2),
                new KeyMapping(61, ConsoleKeyEx.F3),
                new KeyMapping(62, ConsoleKeyEx.F4),
                new KeyMapping(63, ConsoleKeyEx.F5),
                new KeyMapping(64, ConsoleKeyEx.F6),
                new KeyMapping(65, ConsoleKeyEx.F7),
                new KeyMapping(66, ConsoleKeyEx.F8),
                new KeyMapping(67, ConsoleKeyEx.F9),
                new KeyMapping(68, ConsoleKeyEx.F10),
                new KeyMapping(87, ConsoleKeyEx.F11),
                new KeyMapping(88, ConsoleKeyEx.F12),
                new KeyMapping(69, ConsoleKeyEx.NumLock),
                new KeyMapping(70, ConsoleKeyEx.ScrollLock),
                new KeyMapping(71, '\0', '\0', '7', '\0', '\0', '\0', ConsoleKeyEx.Home, ConsoleKeyEx.Num7),
                new KeyMapping(72, '\0', '\0', '8', '\0', '\0', '\0', ConsoleKeyEx.UpArrow, ConsoleKeyEx.Num8),
                new KeyMapping(73, '\0', '\0', '9', '\0', '\0', '\0', ConsoleKeyEx.PageUp, ConsoleKeyEx.Num9),
                new KeyMapping(74, '-', '-', '-', '-', '-', '-', ConsoleKeyEx.NumMinus),
                new KeyMapping(75, '\0', '\0', '4', '\0', '\0', '\0', ConsoleKeyEx.LeftArrow, ConsoleKeyEx.Num4),
                new KeyMapping(76, '\0', '\0', '5', '\0', '\0', '\0', ConsoleKeyEx.Num5),
                new KeyMapping(77, '\0', '\0', '6', '\0', '\0', '\0', ConsoleKeyEx.RightArrow, ConsoleKeyEx.Num6),
                new KeyMapping(78, '+', '+', '+', '+', '+', '+', ConsoleKeyEx.NumPlus),
                new KeyMapping(79, '\0', '\0', '1', '\0', '\0', '\0', ConsoleKeyEx.End, ConsoleKeyEx.Num1),
                new KeyMapping(80, '\0', '\0', '2', '\0', '\0', '\0', ConsoleKeyEx.DownArrow, ConsoleKeyEx.Num2),
                new KeyMapping(81, '\0', '\0', '3', '\0', '\0', '\0', ConsoleKeyEx.PageDown, ConsoleKeyEx.Num3),
                new KeyMapping(82, '\0', '\0', '0', '\0', '\0', '\0', ConsoleKeyEx.Insert, ConsoleKeyEx.Num0),
                new KeyMapping(83, '\0', '\0', '.', '\0', '\0', '\0', ConsoleKeyEx.Delete, ConsoleKeyEx.NumPeriod),
                new KeyMapping(91, ConsoleKeyEx.LWin),
                new KeyMapping(92, ConsoleKeyEx.RWin)
            };
        }
    }
}
