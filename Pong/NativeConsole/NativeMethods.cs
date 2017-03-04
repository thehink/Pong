using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Pong.NativeConsole
{

    [StructLayout(LayoutKind.Sequential)]
    public struct COORD
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
    public struct CharUnion
    {
        [FieldOffset(0)]
        public char UnicodeChar;
        [FieldOffset(0)]
        public byte AsciiChar;
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct CharInfo
    {
        [FieldOffset(0)]
        public CharUnion Char;
        [FieldOffset(2)]
        public short Attributes;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct SMALL_RECT
    {
        public short Left;
        public short Top;
        public short Right;
        public short Bottom;
    }

    class NativeMethods
    {

        [DllImport("User32.dll")]
        public static extern short GetAsyncKeyState(System.Int32 vKey);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr GetStdHandle(int nStdHandle);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool WriteConsoleOutput(
          IntPtr hConsoleOutput,
          CharInfo[] lpBuffer,
          COORD dwBufferSize,
          COORD dwBufferCoord,
          ref SMALL_RECT lpWriteRegion);

        [DllImport("Kernel32")]
        public extern static Boolean CloseHandle(IntPtr handle);


    }
}
