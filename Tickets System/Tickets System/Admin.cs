using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;

namespace Tickets_System
{
    class Admin : IAutorised
    {
        public string Login { get; set; }
        public string Password { get; set; }

        public bool SetLogin()
        {
            Login = Console.ReadLine();
            if (Login.Length < 6)
                return false;
            else return true;
        }
        public bool SetPassword()
        {
            #region Password Masking
            Password = "";
            do
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                // Backspace Should Not Work
                if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                {
                    Password += key.KeyChar;
                    Console.Write("*");
                }
                else
                {
                    if (key.Key == ConsoleKey.Backspace && Password.Length > 0)
                    {
                        Password = Password.Substring(0, (Password.Length - 1));
                        Console.Write("\b \b");
                    }
                    else if (key.Key == ConsoleKey.Enter)
                    {
                        break;
                    }
                }
            } while (true);
            #endregion

            if (Password.Length < 6)
                return false;
            else
            {
                Password = Encryptor.EncryptOrDecrypt(Password, "parol");
                return true;
            }
        }

        public void SignIn()
        { 
            List<Admin> admins = null;
            var str = File.ReadAllText("Admin.json");
            admins = JsonConvert.DeserializeObject<List<Admin>>(str);
            if (admins != null)
            {
                admins.Add(this);
            }
            else
            {
                admins = new List<Admin>();
                admins.Add(this);
            }

            var ser = JsonConvert.SerializeObject(admins, Formatting.Indented);
            File.WriteAllText("Admin.json", ser);
        }
        public bool LogIn()
        {
            var ser = File.ReadAllText("Admin.json");
            dynamic users = JsonConvert.DeserializeObject(ser);

            bool isLogged = false;
            if (users != null)
            {
                foreach (var item in users)
                {
                    if (Login == item["Login"].ToString() && Password == item["Password"].ToString())
                    {
                        isLogged = true;
                        break;
                    }
                }
                if (isLogged == true)  //  Logged In
                {
                    return true;
                }
                else  // Wrong Login or Password
                {
                    return false;
                }
            }
            else   // DataBase is empty...
            {
                return false;
            }
        }
        public void LogOut()
        {
            Login = null;
            Password = null;
        }
    }
    class LoginForAdmin
    {
        public static void Menu()
        {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.Clear();
            DrawPage();
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
                            if (record.MouseEvent.dwMousePosition.Y >= 8 && record.MouseEvent.dwMousePosition.Y <= 10 &&
                                   record.MouseEvent.dwMousePosition.X > 13 && record.MouseEvent.dwMousePosition.X <= 30)  // Back
                            {
                                Console.BackgroundColor = ConsoleColor.Cyan;
                                Console.Clear();
                                IboTicketsMenu.SetMenu();
                                return;
                            }

                            break;
                        }
                    case NativeMethods.KEY_EVENT:
                        {
                            if (record.KeyEvent.bKeyDown == true && record.KeyEvent.wVirtualKeyCode == 13) // Enter pressed
                            {
                                LoginPage();
                                Console.BackgroundColor = ConsoleColor.Red;
                                Console.Clear();
                                DrawPage();
                            }
                            break;
                        }
                }
            }
        }
        private static void DrawPage()
        {
            #region Draw LoginWindow
            for (int i = 6; i < 25; i++)
            {
                for (int j = 45; j < 80; j++)
                {
                    Console.SetCursorPosition(j, i);
                    if (i == 6 || i == 25 - 1 || j == 45 || j == 80 - 1 || i == 10)
                        Console.ForegroundColor = ConsoleColor.Black;
                    else if (i < 11)
                        Console.ForegroundColor = ConsoleColor.DarkBlue;
                    else
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.Write("█");
                }
            }
            Console.SetCursorPosition(54, 8);
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Admin Login Page");

            Console.SetCursorPosition(48, 16);
            Console.BackgroundColor = ConsoleColor.DarkGray;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write(" Login:   ");  // 10 symbol
            Console.BackgroundColor = ConsoleColor.White;
            Console.Write("               ");

            Console.SetCursorPosition(48, 18);
            Console.BackgroundColor = ConsoleColor.DarkGray;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write("Password: ");  // 10 symbol
            Console.BackgroundColor = ConsoleColor.White;
            Console.Write("               ");
            #endregion

            Console.BackgroundColor = ConsoleColor.Yellow;
            Console.ForegroundColor = ConsoleColor.Black;
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    Console.SetCursorPosition(12 + j, 7 + i);
                    if (i == 0 || i == 4 || j == 0 || j == 19)
                        Console.Write("█");
                    else
                        Console.Write(" ");
                }
                Console.SetCursorPosition(20, 9);
                Console.Write("Back");
            }

        }
        public static void LoginPage()
        {
            var admin = new Admin();
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;

            do
            {
                Console.SetCursorPosition(58, 16);
                Console.Write("               ");
                Console.SetCursorPosition(60, 16);
            } while (admin.SetLogin() == false);

            do
            {
                Console.SetCursorPosition(58, 18);
                Console.Write("               ");
                Console.SetCursorPosition(60, 18);
            } while (admin.SetPassword() == false);

            if (admin.LogIn() == true)
            {
                Console.BackgroundColor = ConsoleColor.DarkGreen;
                Console.SetCursorPosition(55, 21);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("~You Logged In~");
                Console.ReadKey();

                IboTicketsAdminMenu.Menu();
            }
            else
            {
                Console.BackgroundColor = ConsoleColor.DarkRed;
                Console.SetCursorPosition(50, 21);
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("~Wrong Login or Password~");
                Console.ReadKey();
                
            }
        }
    }
        
}
