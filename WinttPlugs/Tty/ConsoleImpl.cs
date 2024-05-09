using IL2CPU.API.Attribs;
using System;
using System.Runtime.CompilerServices;
using UniLua;

namespace WinttPlugs.Tty
{
    using Sys = WinttOS.wSystem.WinttOS;

    [Plug(Target = typeof(Console))]
    public class ConsoleImpl
    {
        
        public static string? ReadLine()
        {
            if (Sys.IsTty)
                return null;
            else
                return Console.ReadLine();
        }
        public static void WriteLine()
        {
            if (Sys.IsTty)
                ;
            else
                Console.WriteLine();
        }

        public static void WriteLine(bool value)
        {
            if (Sys.IsTty)
                ;
            else
                Console.WriteLine(value);
        }

        public static void WriteLine(char value)
        {
            if (Sys.IsTty)
                ;
            else
                Console.WriteLine(value);
        }

        public static void WriteLine(decimal value)
        {
            if (Sys.IsTty)
                ;
            else
                Console.WriteLine(value);
        }

        public static void WriteLine(double value)
        {
            if (Sys.IsTty)
                ;
            else
                Console.WriteLine(value);
        }

        public static void WriteLine(float value)
        {
            if (Sys.IsTty)
                ;
            else
                Console.WriteLine(value);
        }

        public static void WriteLine(int value)
        {
            if (Sys.IsTty)
                ;
            else
                Console.WriteLine(value);
        }

        public static void WriteLine(uint value)
        {
            if (Sys.IsTty)
                ;
            else
                Console.WriteLine(value);
        }

        public static void WriteLine(long value)
        {
            if (Sys.IsTty)
                ;
            else
                Console.WriteLine(value);
        }

        public static void WriteLine(ulong value)
        {
            if (Sys.IsTty)
                ;
            else
                Console.WriteLine(value);
        }

        public static void WriteLine(object? value)
        {
            if (Sys.IsTty)
                ;
            else
                Console.WriteLine(value);
        }

        public static void WriteLine(string? value)
        {
            if (Sys.IsTty)
                ;
            else
                Console.WriteLine(value);
        }

        public static void WriteLine(string format, object? arg0)
        {
            if (Sys.IsTty)
                ;
            else
                Console.WriteLine(format, arg0);
        }

        public static void WriteLine(string format, object? arg0, object? arg1)
        {
            if (Sys.IsTty)
                ;
            else
                Console.WriteLine(format, arg0, arg1);
        }

        public static void WriteLine(string format, object? arg0, object? arg1, object? arg2)
        {
            if (Sys.IsTty)
                ;
            else
                Console.WriteLine(format, arg0, arg1, arg2);
        }

        public static void WriteLine(string format, params object?[]? arg)
        {
            if (Sys.IsTty)
                ;
            else
                Console.WriteLine(format, arg);
        }

        public static void Write(string format, object? arg0)
        {
            if (Sys.IsTty)
                ;
            else
                Console.Write(format, arg0);
        }

        public static void Write(string format, object? arg0, object? arg1)
        {
            if (Sys.IsTty)
                ;
            else
                Console.Write(format, arg0, arg1);
        }

        public static void Write(string format, object? arg0, object? arg1, object? arg2)
        {
            if (Sys.IsTty)
                ;
            else
                Console.Write(format, arg0, arg1, arg2);
        }

        [MethodImplAttribute(MethodImplOptions.NoInlining)]
        public static void Write(string format, params object?[]? arg)
        {
            if (Sys.IsTty)
                ;
            else
                Console.Write(format, arg);
        }

        public static void Write(bool value)
        {
            if (Sys.IsTty)
                ;
            else
                Console.Write(value);
        }

        public static void Write(char value)
        {
            if (Sys.IsTty)
                ;
            else
                Console.Write(value);
        }


        public static void Write(double value)
        {
            if (Sys.IsTty)
                ;
            else
                Console.Write(value);
        }

        public static void Write(decimal value)
        {
            if (Sys.IsTty)
                ;
            else
                Console.Write(value);
        }

        public static void Write(float value)
        {
            if (Sys.IsTty)
                ;
            else
                Console.Write(value);
        }

        public static void Write(int value)
        {
            if (Sys.IsTty)
                ;
            else
                Console.Write(value);
        }

        public static void Write(uint value)
        {
            if (Sys.IsTty)
                ;
            else
                Console.Write(value);
        }

        public static void Write(long value)
        {
            if (Sys.IsTty)
                ;
            else
                Console.Write(value);
        }

        public static void Write(ulong value)
        {
            if (Sys.IsTty)
                ;
            else
                Console.Write(value);
        }

        public static void Write(object? value)
        {
            if (Sys.IsTty)
                ;
            else
                Console.Write(value);
        }
        public static void Write(string? value)
        {
            if (Sys.IsTty)
                ;
            else
                Console.Write(value);
        }
    }
    
}
