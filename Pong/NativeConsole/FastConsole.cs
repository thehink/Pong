using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Pong.NativeConsole
{

    class FastConsole : IDisposable
    {

        private SMALL_RECT rect;
        private COORD bufCoord = new COORD() { X = 0, Y = 0 };
        private COORD bufSize;
        private CharInfo[] buf;
        private IntPtr hStdout;
        private const int STD_OUTPUT_HANDLE = -11;
        private bool disposed = false;

        public FastConsole(short width, short height)
        {
            hStdout = NativeMethods.GetStdHandle(STD_OUTPUT_HANDLE);
            buf = new CharInfo[width * height];
            bufSize = new COORD() { X = width, Y = height };
            rect = new SMALL_RECT() { Left = 0, Top = 0, Right = width, Bottom = height };
            Console.SetWindowSize(width, height);
            Console.SetBufferSize(width, height);
        }

        public void Dispose()
        {
            this.Dispose(true);
            // This object will be cleaned up by the Dispose method.
            // Therefore, you should call GC.SupressFinalize to
            // take this object off the finalization queue
            // and prevent finalization code for this object
            // from executing a second time.
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called.
            if (!this.disposed)
            {
                // If disposing equals true, dispose all managed
                // and unmanaged resources.
                if (disposing)
                {
                    // Dispose managed resources.
                }

                // Call the appropriate methods to clean up
                // unmanaged resources here.
                // If disposing is false,
                // only the following code is executed.
                NativeMethods.CloseHandle(hStdout);
                hStdout = IntPtr.Zero;

                // Note disposing has been done.
                disposed = true;

            }
        }

        public void SetCursorPosition()
        {

        }

        public void WriteString(string str, int x, int y, ConsoleColor color = ConsoleColor.White)
        {
            for (int i = 0; i < str.Length; ++i)
            {
                WriteChar(str[i], (short)(x + i), y, color);
            }
        }

        public void WriteChar(char chr, int x, int y, ConsoleColor color = ConsoleColor.White)
        {
            int pos = y * bufSize.X + x;
            buf[pos].Attributes = (short)color;
            buf[pos].Char.UnicodeChar = chr;
        }

        public char ReadChar(int x, int y)
        {
            int pos = y * bufSize.X + x;
            return buf[pos].Char.UnicodeChar;
        }

        public void Clear()
        {
            Array.Clear(buf, 0, buf.Length);
        }

        public void Draw()
        {
            NativeMethods.WriteConsoleOutput(hStdout, buf, bufSize, bufCoord, ref rect);
        }

        public static int ReadInt(int min = 0, int max = 0)
        {
            int num;

            Console.Write(min != 0 && max != min ? $"{min} - {max}:" : ":");

            while (!int.TryParse(Console.ReadLine(), out num) || num < min || num > max)
            {
                Console.Write($"Invalid number! Try again {(min != 0 && max != min ? $"({min} - {max}):" : ":")}");
            }
            return num;
        }

        public static bool IsKeyDown(VirtualKeys key)
        {

            return (NativeMethods.GetAsyncKeyState((ushort)key) & 0x8000 ) > 0;
        }

        ~FastConsole()
        {
            // Do not re-create Dispose clean-up code here.
            // Calling Dispose(false) is optimal in terms of
            // readability and maintainability.
            this.Dispose(false);
        }
    }
}
