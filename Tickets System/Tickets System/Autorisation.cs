using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.IO;

namespace Tickets_System
{
    class Autorisation
    {
        static int yMin;
        static int yMax;
        static int xMin;
        static int xMax;
        static int xMiddle;

        static int boardX_Min;
        static int boardY_Min;

        static Autorisation()
        {
            yMin = 4;
            yMax = 30;
            xMin = 40;
            xMax = 95;
            xMiddle = xMin + (xMax - xMin) / 2;

            boardX_Min = xMin + 1;
            boardY_Min = yMin + 7;
        }
        public static void Menu(NativeMethods.INPUT_RECORD record, NativeMethods.ConsoleHandle handle, uint recordLen)
        {
            bool login_sign = true;
            Console.BackgroundColor = ConsoleColor.Red;
            Console.Clear();
            DrawMenu(login_sign);

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
                            if (record.MouseEvent.dwMousePosition.Y >= 5 && record.MouseEvent.dwMousePosition.Y <= 9 &&
                                record.MouseEvent.dwMousePosition.X >= 41 && record.MouseEvent.dwMousePosition.X <= 66) // Login 
                            {
                                if (login_sign == false)
                                {
                                    login_sign = true;
                                    DrawMenu(login_sign);
                                }
                            }
                            else if (record.MouseEvent.dwMousePosition.Y >= 5 && record.MouseEvent.dwMousePosition.Y <= 9 &&
                                   record.MouseEvent.dwMousePosition.X > 68 && record.MouseEvent.dwMousePosition.X <= 93) // Sign in
                            {
                                if (login_sign == true)
                                {
                                    login_sign = false;
                                    DrawMenu(login_sign);
                                }
                            }
                            else if (record.MouseEvent.dwMousePosition.Y >= 8 && record.MouseEvent.dwMousePosition.Y <= 10 &&
                                   record.MouseEvent.dwMousePosition.X > 13 && record.MouseEvent.dwMousePosition.X <= 30)  // Back
                            {                                
                                return;
                            }

                            break;
                        }
                    case NativeMethods.KEY_EVENT:
                        {
                            if (record.KeyEvent.bKeyDown == true && record.KeyEvent.wVirtualKeyCode == 13) // Enter pressed
                            {
                                if (login_sign == true) // Login
                                {
                                    var user = new User();
                                    if (LoginMenu(user) == true)
                                    {
                                        IboTicketsUserMenu.Menu(user);
                                        Console.BackgroundColor = ConsoleColor.Red;
                                        Console.Clear();
                                        DrawMenu(true);
                                    }
                                    else
                                        DrawMenu(true);
                                }
                                else     // Sign in
                                {
                                    SignMenu();
                                }
                            }
                            break;
                        }
                }
            }
        }
        private static bool LoginMenu(User user)
        {            
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;

            do
            {
                Console.SetCursorPosition(boardX_Min + 22, boardY_Min + 7);
                Console.Write("               ");
                Console.SetCursorPosition(boardX_Min + 22 + 2, boardY_Min + 7);
            } while (user.SetLogin() == false);

            do
            {
                Console.SetCursorPosition(boardX_Min + 22, boardY_Min + 9);
                Console.Write("               ");
                Console.SetCursorPosition(boardX_Min + 22 + 2, boardY_Min + 9);
            } while (user.SetPassword() == false);

            user.PictureNum = -1; 
            
            if (user.LogIn() == true)
            {
                Console.BackgroundColor = ConsoleColor.DarkGreen;
                Console.SetCursorPosition(boardX_Min + 19, boardY_Min + 13);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("~You Logged In~");
                Console.ReadKey();

                return true;
            }
            else
            {
                Console.BackgroundColor = ConsoleColor.DarkRed;
                Console.SetCursorPosition(boardX_Min + 15, boardY_Min + 13);
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("~Wrong Login or Password~");
                Console.ReadKey();
            }            
            return false;
        }
        private static void SignMenu()
        {
            var user = new User();
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;

            #region Entering Information
            do
            {
                Console.SetCursorPosition(boardX_Min + 22, boardY_Min + 2);
                Console.Write("               ");
                Console.SetCursorPosition(boardX_Min + 22 + 2, boardY_Min + 2);
            } while (user.SetName() == false);

            do
            {
                Console.SetCursorPosition(boardX_Min + 22, boardY_Min + 4);
                Console.Write("               ");
                Console.SetCursorPosition(boardX_Min + 22 + 2, boardY_Min + 4);
            } while (user.SetSurname() == false);

            do
            {
                Console.SetCursorPosition(boardX_Min + 22, boardY_Min + 6);
                Console.Write("               ");
                Console.SetCursorPosition(boardX_Min + 22 + 2, boardY_Min + 6);
            } while (user.SetAge() == false);

            do
            {
                Console.SetCursorPosition(boardX_Min + 22, boardY_Min + 8);
                Console.Write("               ");
                Console.SetCursorPosition(boardX_Min + 22 + 2, boardY_Min + 8);
            } while (user.SetLogin() == false);

            do
            {
                Console.SetCursorPosition(boardX_Min + 22, boardY_Min + 10);
                Console.Write("               ");
                Console.SetCursorPosition(boardX_Min + 22 + 2, boardY_Min + 10);
            } while (user.SetPassword() == false);
            string firstPassword = user.Password;

            do
            {
                Console.SetCursorPosition(boardX_Min + 22, boardY_Min + 12);
                Console.Write("               ");
                Console.SetCursorPosition(boardX_Min + 22 + 2, boardY_Min + 12);
            } while (user.SetPassword() == false);
            #endregion

            var ser = File.ReadAllText("Users.json");
            dynamic users = JsonConvert.DeserializeObject(ser);

            bool isExist = false;
            if (users != null)
            {
                foreach (var item in users)
                {
                    if (user.Login == item["Login"].ToString())
                    {
                        isExist = true;
                        break;
                    }
                }
            }

            if (firstPassword != user.Password)
            {
                Console.BackgroundColor = ConsoleColor.DarkRed;
                Console.SetCursorPosition(boardX_Min + 13, boardY_Min + 15);
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("~Enter Password Carefully~");
                Console.ReadKey();
            }
            else if(isExist == true)
            {
                Console.BackgroundColor = ConsoleColor.DarkRed;
                Console.SetCursorPosition(boardX_Min + 15, boardY_Min + 15);
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("~Login is already exist~");
                Console.ReadKey();
            }
            else   // Signed in
            {
                user.SignIn();
                Console.BackgroundColor = ConsoleColor.DarkGreen;
                Console.SetCursorPosition(boardX_Min + 19, boardY_Min + 15);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("~You Signed In~");
                Console.ReadKey();
            }

            DrawMenu(false);
        }
        public static void DrawMenu(bool login_sign = true)   // true => Login page,  false => Sign Page
        {
            #region DrawPage
            for (int i = yMin; i < yMax; i++)
            {
                for (int j = xMin; j < xMax; j++)
                {
                    Console.SetCursorPosition(j, i);
                    if (i == yMin || i == yMax - 1 || j == xMin || j == xMax - 1 || i == yMin + 6)
                        Console.ForegroundColor = ConsoleColor.Black;
                    else if (i < yMin + 6)
                        if (j == xMiddle)
                            Console.ForegroundColor = ConsoleColor.Black;
                        else
                        if (j < xMiddle == login_sign)
                            Console.ForegroundColor = ConsoleColor.Cyan;
                        else
                            Console.ForegroundColor = ConsoleColor.DarkBlue;
                    else
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.Write("█");
                }
            }
            if (login_sign == true)
            {
                Console.SetCursorPosition(xMin + 12, yMin + 3);
                Console.BackgroundColor = ConsoleColor.Cyan;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.Write("Login");
                Console.SetCursorPosition(xMiddle + 10, yMin + 3);
                Console.BackgroundColor = ConsoleColor.DarkBlue;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.Write("Sign in");
            }
            else
            {
                Console.SetCursorPosition(xMin + 12, yMin + 3);
                Console.BackgroundColor = ConsoleColor.DarkBlue;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.Write("Login");
                Console.SetCursorPosition(xMiddle + 10, yMin + 3);
                Console.BackgroundColor = ConsoleColor.Cyan;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.Write("Sign in");
            }

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
            #endregion

            if (login_sign == true)
                DrawLoginMenu();
            else
                DrawSignMenu();
        }
        private static void DrawLoginMenu()
        {
            Console.SetCursorPosition(boardX_Min + 12, boardY_Min + 7);
            Console.BackgroundColor = ConsoleColor.DarkGray;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write(" Login:   ");  // 10 symbol
            Console.BackgroundColor = ConsoleColor.White;
            Console.Write("               ");

            Console.SetCursorPosition(boardX_Min + 12, boardY_Min + 9);
            Console.BackgroundColor = ConsoleColor.DarkGray;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write("Password: ");  // 10 symbol
            Console.BackgroundColor = ConsoleColor.White;
            Console.Write("               ");
        }
        private static void DrawSignMenu()
        {
            Console.SetCursorPosition(boardX_Min + 12, boardY_Min + 2);
            Console.BackgroundColor = ConsoleColor.DarkGray;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write("  Name:   ");  // 10 symbol
            Console.BackgroundColor = ConsoleColor.White;
            Console.Write("               ");

            Console.SetCursorPosition(boardX_Min + 12, boardY_Min + 4);
            Console.BackgroundColor = ConsoleColor.DarkGray;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write(" Surname: ");  // 10 symbol
            Console.BackgroundColor = ConsoleColor.White;
            Console.Write("               ");

            Console.SetCursorPosition(boardX_Min + 12, boardY_Min + 6);
            Console.BackgroundColor = ConsoleColor.DarkGray;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write("  Age:    ");  // 10 symbol
            Console.BackgroundColor = ConsoleColor.White;
            Console.Write("               ");

            Console.SetCursorPosition(boardX_Min + 12, boardY_Min + 8);
            Console.BackgroundColor = ConsoleColor.DarkGray;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write(" Login:   ");  // 10 symbol
            Console.BackgroundColor = ConsoleColor.White;
            Console.Write("               ");

            Console.SetCursorPosition(boardX_Min + 12, boardY_Min + 10);
            Console.BackgroundColor = ConsoleColor.DarkGray;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write("Password: ");  // 10 symbol
            Console.BackgroundColor = ConsoleColor.White;
            Console.Write("               ");

            Console.SetCursorPosition(boardX_Min + 9, boardY_Min + 12);
            Console.BackgroundColor = ConsoleColor.DarkGray;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write("Re-Password: ");
            Console.BackgroundColor = ConsoleColor.White;
            Console.Write("               ");
        }
    }
}
