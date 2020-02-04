using System;
using System.Runtime.InteropServices;
using System.Diagnostics;
using Microsoft.Win32.SafeHandles;
using System.ComponentModel;

namespace Tickets_System
{
    interface IAutorised
    {
        void SignIn();
        bool LogIn();
        void LogOut();
    }
    static class IboTicketsMenu
    {
        private static void DrawLogo()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            for (int i = 1; i < 9; i++)
            {
                for (int j = 15; j < 116; j++)
                {
                    if (i == 1)
                    {
                        Console.SetCursorPosition(j, i);
                        Console.Write("▄");
                    }
                    else if (i == 8)
                    {
                        Console.SetCursorPosition(j, i);
                        Console.Write("▀");
                    }
                    else if (j == 15 || j == 115)
                    {
                        Console.SetCursorPosition(j, i);
                        Console.Write("█");
                    }
                }
            }

            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.SetCursorPosition(16, 2);
            Console.Write(@"  __     ______     ______     ______   __     ______     __  __     ______     ______   ______    ");
            Console.SetCursorPosition(16, 3);
            Console.Write(@" /\ \   /\  == \   /\  __ \   /\__  _\ /\ \   /\  ___\   /\ \/ /    /\  ___\   /\__  _\ /\  ___\   ");
            Console.SetCursorPosition(16, 4);
            Console.Write(@" \ \ \  \ \  __<   \ \ \/\ \  \/_/\ \/ \ \ \  \ \ \____  \ \  _'-.  \ \  __\   \/_/\ \/ \ \___  \  ");
            Console.SetCursorPosition(16, 5);
            Console.Write(@"  \ \_\  \ \_____\  \ \_____\    \ \_\  \ \_\  \ \_____\  \ \_\ \_\  \ \_____\    \ \_\  \/\_____\ ");
            Console.SetCursorPosition(16, 6);
            Console.Write(@"   \/_/   \/_____/   \/_____/     \/_/   \/_/   \/_____/   \/_/\/_/   \/_____/     \/_/   \/_____/ ");
            Console.SetCursorPosition(16, 7);
            Console.Write(@"                                                                                                   ");
        }
        private static void DrawRect(int x, int y, string text)
        {
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.BackgroundColor = ConsoleColor.DarkYellow;
            for (int i = 0; i < 15; i++)
            {
                for (int j = 0; j < 30; j++)
                {
                    Console.SetCursorPosition(j + x, i + y);
                    if (i == 0 || i == 14 || j == 0 || j == 29)
                    {
                        Console.Write("█");
                    }
                    else Console.Write(" ");
                }
            }
            Console.SetCursorPosition(x + 12, y + 7);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.BackgroundColor = ConsoleColor.Red;
            Console.Write(text);
        }
        public static void SetMenu()
        {
            DrawLogo();
            DrawRect(25, 12, " USER ");
            DrawRect(75, 12, " ADMIN ");
        }
        public static void Menu()
        {
            SetMenu();
            #region Prepare Variables For Mouse
            var handle = NativeMethods.GetStdHandle(NativeMethods.STD_INPUT_HANDLE);

            int mode = 0;
            if (!(NativeMethods.GetConsoleMode(handle, ref mode))) { throw new Win32Exception(); }

            mode |= NativeMethods.ENABLE_MOUSE_INPUT;
            mode &= ~NativeMethods.ENABLE_QUICK_EDIT_MODE;
            mode |= NativeMethods.ENABLE_EXTENDED_FLAGS;

            if (!(NativeMethods.SetConsoleMode(handle, mode))) { throw new Win32Exception(); }

            var record = new NativeMethods.INPUT_RECORD();
            uint recordLen = 0;
            #endregion

            while (true)
            {
                if (!(NativeMethods.ReadConsoleInput(handle, ref record, 1, ref recordLen))) { throw new Win32Exception(); }
                Console.SetCursorPosition(0, 0);  // siline biler
                switch (record.EventType)
                {
                    case NativeMethods.MOUSE_EVENT:
                        {
                            Console.WriteLine("x: " + record.MouseEvent.dwMousePosition.X);
                            Console.WriteLine("y: " + record.MouseEvent.dwMousePosition.Y);
                            Console.WriteLine("y: " + record.MouseEvent.dwEventFlags);

                            if (record.MouseEvent.dwButtonState != 1 || record.MouseEvent.dwEventFlags != 0)
                                continue;
                            if (record.MouseEvent.dwMousePosition.X >= 26 && record.MouseEvent.dwMousePosition.X <= 53 &&
                                record.MouseEvent.dwMousePosition.Y >= 13 && record.MouseEvent.dwMousePosition.Y <= 25)
                            {
                                Autorisation.Menu(record, handle, recordLen);
                                Console.BackgroundColor = ConsoleColor.Cyan;
                                Console.Clear();
                                SetMenu();
                            }
                            if (record.MouseEvent.dwMousePosition.X >= 76 && record.MouseEvent.dwMousePosition.X <= 103 &&
                                record.MouseEvent.dwMousePosition.Y >= 13 && record.MouseEvent.dwMousePosition.Y <= 25)
                            {
                                LoginForAdmin.Menu();
                            }
                        }
                        break;
                }
            }
        }
    }
    public class NativeMethods
    {
        public const Int32 STD_INPUT_HANDLE = -10;

        public const Int32 ENABLE_MOUSE_INPUT = 0x0010;
        public const Int32 ENABLE_QUICK_EDIT_MODE = 0x0040;
        public const Int32 ENABLE_EXTENDED_FLAGS = 0x0080;

        public const Int32 KEY_EVENT = 1;
        public const Int32 MOUSE_EVENT = 2;


        [DebuggerDisplay("EventType: {EventType}")]
        [StructLayout(LayoutKind.Explicit)]
        public struct INPUT_RECORD
        {
            [FieldOffset(0)]
            public Int16 EventType;
            [FieldOffset(4)]
            public KEY_EVENT_RECORD KeyEvent;
            [FieldOffset(4)]
            public MOUSE_EVENT_RECORD MouseEvent;
        }

        [DebuggerDisplay("{dwMousePosition.X}, {dwMousePosition.Y}")]
        public struct MOUSE_EVENT_RECORD
        {
            public COORD dwMousePosition;
            public Int32 dwButtonState;
            public Int32 dwControlKeyState;
            public Int32 dwEventFlags;
        }

        [DebuggerDisplay("{X}, {Y}")]
        public struct COORD
        {
            public UInt16 X;
            public UInt16 Y;
        }

        [DebuggerDisplay("KeyCode: {wVirtualKeyCode}")]
        [StructLayout(LayoutKind.Explicit)]
        public struct KEY_EVENT_RECORD
        {
            [FieldOffset(0)]
            [MarshalAsAttribute(UnmanagedType.Bool)]
            public Boolean bKeyDown;
            [FieldOffset(4)]
            public UInt16 wRepeatCount;
            [FieldOffset(6)]
            public UInt16 wVirtualKeyCode;
            [FieldOffset(8)]
            public UInt16 wVirtualScanCode;
            [FieldOffset(10)]
            public Char UnicodeChar;
            [FieldOffset(10)]
            public Byte AsciiChar;
            [FieldOffset(12)]
            public Int32 dwControlKeyState;
        };


        public class ConsoleHandle : SafeHandleMinusOneIsInvalid
        {
            public ConsoleHandle() : base(false) { }

            protected override bool ReleaseHandle()
            {
                return true; //releasing console handle is not our business
            }
        }


        [DllImportAttribute("kernel32.dll", SetLastError = true)]
        [return: MarshalAsAttribute(UnmanagedType.Bool)]
        public static extern Boolean GetConsoleMode(ConsoleHandle hConsoleHandle, ref Int32 lpMode);

        [DllImportAttribute("kernel32.dll", SetLastError = true)]
        public static extern ConsoleHandle GetStdHandle(Int32 nStdHandle);

        [DllImportAttribute("kernel32.dll", SetLastError = true)]
        [return: MarshalAsAttribute(UnmanagedType.Bool)]
        public static extern Boolean ReadConsoleInput(ConsoleHandle hConsoleInput, ref INPUT_RECORD lpBuffer, UInt32 nLength, ref UInt32 lpNumberOfEventsRead);

        [DllImportAttribute("kernel32.dll", SetLastError = true)]
        [return: MarshalAsAttribute(UnmanagedType.Bool)]
        public static extern Boolean SetConsoleMode(ConsoleHandle hConsoleHandle, Int32 dwMode);

    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.BackgroundColor = ConsoleColor.Cyan;
            Console.SetWindowSize(Console.LargestWindowWidth, Console.LargestWindowHeight);
            Console.SetBufferSize(Console.LargestWindowWidth, Console.LargestWindowHeight);

            IboTicketsMenu.Menu();      
            //int a, b;
            //IboTicketsAdminMenu._GetHourAndMinute(out a, out b, "13:30");

            IboTicketsAdminMenu.DrawMenu();

            Console.ReadKey();
        }
    }
}