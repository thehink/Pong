using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Pong
{

    class FastConsole
    {
        private SMALL_RECT rect;
        private COORD bufCoord = new COORD() { X = 0, Y = 0 };
        private COORD bufSize;
        private CharInfo[] buf;
        private IntPtr hStdout;
        private const int STD_OUTPUT_HANDLE = -11;

        [DllImport("User32.dll")]
        private static extern short GetAsyncKeyState(System.Int32 vKey);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr GetStdHandle(int nStdHandle);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool WriteConsoleOutput(
          IntPtr hConsoleOutput,
          CharInfo[] lpBuffer,
          COORD dwBufferSize,
          COORD dwBufferCoord,
          ref SMALL_RECT lpWriteRegion);

        [StructLayout(LayoutKind.Sequential)]
        private struct COORD
        {
            public short X;
            public short Y;

            public COORD(short X, short Y)
            {
                this.X = X;
                this.Y = Y;
            }
        };

        [StructLayout(LayoutKind.Explicit)]
        private struct CharUnion
        {
            [FieldOffset(0)]
            public char UnicodeChar;
            [FieldOffset(0)]
            public byte AsciiChar;
        }

        [StructLayout(LayoutKind.Explicit)]
        private struct CharInfo
        {
            [FieldOffset(0)]
            public CharUnion Char;
            [FieldOffset(2)]
            public short Attributes;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct SMALL_RECT
        {
            public short Left;
            public short Top;
            public short Right;
            public short Bottom;
        }

        public FastConsole(short width, short height)
        {
            hStdout = GetStdHandle(STD_OUTPUT_HANDLE);
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
            WriteConsoleOutput(hStdout, buf, bufSize, bufCoord, ref rect);
        }

        public static bool IsKeyDown(VirtualKeys key)
        {

            return (GetAsyncKeyState((ushort)key) & 0x8000 ) > 0;
        }
    }
}
