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

    class FastConsole
    {

        private SMALL_RECT rect;
        private COORD bufCoord = new COORD() { X = 0, Y = 0 };
        private COORD bufSize;
        private CharInfo[] buf;
        private IntPtr hStdout;
        private const int STD_OUTPUT_HANDLE = -11;

        public FastConsole(short width, short height)
        {
            hStdout = NativeMethods.GetStdHandle(STD_OUTPUT_HANDLE);
            buf = new CharInfo[width * height];
            bufSize = new COORD() { X = width, Y = height };
            rect = new SMALL_RECT() { Left = 0, Top = 0, Right = width, Bottom = height };
            Console.SetWindowSize(width, height);
            Console.SetBufferSize(width, height);
        }

        public void SetCursorPosition()
        {

        }

        public void WriteString(string str, short x, short y, ConsoleColor color = ConsoleColor.White)
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

        public static bool IsKeyDown(VirtualKeys key)
        {

            return (NativeMethods.GetAsyncKeyState((ushort)key) & 0x8000 ) > 0;
        }
    }
}
