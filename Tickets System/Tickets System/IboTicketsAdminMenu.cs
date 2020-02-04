using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;

namespace Tickets_System
{
    class IboTicketsAdminMenu
    {
        public static void Menu()
        {
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.Clear();
            DrawMenu();

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
                            if (record.MouseEvent.dwMousePosition.Y > 8 && record.MouseEvent.dwMousePosition.Y <= 11 &&
                                   record.MouseEvent.dwMousePosition.X > 50 && record.MouseEvent.dwMousePosition.X < 74)  // Create Tiket
                            {
                                CreateTicketMenu();
                                Console.BackgroundColor = ConsoleColor.DarkBlue;
                                Console.Clear();
                                DrawMenu();
                            }
                            if (record.MouseEvent.dwMousePosition.Y > 14 && record.MouseEvent.dwMousePosition.Y <= 17 &&
                                   record.MouseEvent.dwMousePosition.X > 50 && record.MouseEvent.dwMousePosition.X < 74)  // Delete Tiket
                            {
                                DeleteTicketMenu();
                                Console.BackgroundColor = ConsoleColor.DarkBlue;
                                Console.Clear();
                                DrawMenu();
                            }
                            if (record.MouseEvent.dwMousePosition.Y > 8 && record.MouseEvent.dwMousePosition.Y <= 11 &&
                                   record.MouseEvent.dwMousePosition.X > 12 && record.MouseEvent.dwMousePosition.X < 27) //Back
                            {
                                return;
                            }
                            
                            break;
                        }
                }
            }   
        }

        private static void DrawRect(int ConsoleX, int ConsoleY, int length, int height, string text, ConsoleColor color)
        {
            Console.ForegroundColor = ConsoleColor.Black;

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    Console.SetCursorPosition(j + ConsoleX, i + ConsoleY);
                    if (i == 0)
                        Console.Write("▄");
                    else if (i == height - 1)
                        Console.Write("▀");
                    else if (j == 0 || j == length - 1)
                        Console.Write("█");
                    else
                    {
                        Console.BackgroundColor = color;
                        Console.Write(" ");
                        Console.BackgroundColor = ConsoleColor.DarkBlue;
                    }
                }
            }
            Console.BackgroundColor = color;
            Console.SetCursorPosition(ConsoleX + (length - text.Length) / 2, ConsoleY + height / 2);
            Console.Write(text);
            Console.BackgroundColor = ConsoleColor.DarkBlue;
        }
        public static void DrawMenu()
        {
            DrawRect(50, 8, 25, 5, "Create Ticket", ConsoleColor.DarkCyan);
            DrawRect(50, 14, 25, 5, "Delete Ticket", ConsoleColor.DarkCyan);
            DrawRect(12, 8, 16, 5, "Back", ConsoleColor.Red);
        }

        private static void DeleteTicketMenu()
        {
            Console.BackgroundColor = ConsoleColor.DarkYellow;
            Console.Clear();
            DrawDeleteTicketMenu();

            List<TransportTicket> tickets = TransportTicket.LoadAllTickets();
            int index = 0;
            ShowTicket(tickets[0]);
            DrawNextBackButton(true, false);

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
                            if (record.MouseEvent.dwMousePosition.Y > 8 && record.MouseEvent.dwMousePosition.Y <= 11 &&
                                   record.MouseEvent.dwMousePosition.X > 50 && record.MouseEvent.dwMousePosition.X < 74)  // Delete
                            {
                                
                            }                            
                            if (record.MouseEvent.dwMousePosition.Y > 8 && record.MouseEvent.dwMousePosition.Y <= 11 &&
                                   record.MouseEvent.dwMousePosition.X > 12 && record.MouseEvent.dwMousePosition.X < 27) // Back to Menu
                            {
                                return;
                            }
                            if (record.MouseEvent.dwMousePosition.X >= 38 && record.MouseEvent.dwMousePosition.X <= 45 &&
                               record.MouseEvent.dwMousePosition.Y >= 19 && record.MouseEvent.dwMousePosition.Y <= 20)   // Back
                            {
                                if (index != 0)
                                    index--;
                                else continue;

                               
                            }
                            if (record.MouseEvent.dwMousePosition.X >= 107 && record.MouseEvent.dwMousePosition.X <= 116 &&
                           record.MouseEvent.dwMousePosition.Y >= 19 && record.MouseEvent.dwMousePosition.Y <= 20)   // Next
                            {
                                if (index != tickets.Count - 1)
                                    index++;
                                else continue;

                                
                            }

                            break;
                        }
                }
            }
        }
        private static void DrawNextBackButton(bool BackExist, bool NextExist)
        {
            Console.ForegroundColor = ConsoleColor.Black;
            if (BackExist)
            {
                DrawRect(46, 22, 10, 4, ConsoleColor.Green, ConsoleColor.Cyan);
                Console.BackgroundColor = ConsoleColor.Green;
                Console.SetCursorPosition(48, 23);
                Console.Write("««««««");
                Console.SetCursorPosition(48, 24);
                Console.Write("««««««");
            }
            else
            {
                DrawRect(46, 22, 10, 4, ConsoleColor.DarkGray, ConsoleColor.Cyan);
                Console.BackgroundColor = ConsoleColor.DarkGray;
                Console.SetCursorPosition(48, 23);
                Console.Write("««««««");
                Console.SetCursorPosition(48, 24);
                Console.Write("««««««");
            }
            if (NextExist)
            {
                DrawRect(83, 22, 10, 4, ConsoleColor.Green, ConsoleColor.Cyan);
                Console.BackgroundColor = ConsoleColor.Green;
                Console.SetCursorPosition(85, 23);
                Console.Write("»»»»»»");
                Console.SetCursorPosition(85, 24);
                Console.Write("»»»»»»");
            }
            else
            {
                DrawRect(83, 22, 10, 4, ConsoleColor.DarkGray, ConsoleColor.Cyan);
                Console.BackgroundColor = ConsoleColor.DarkGray;
                Console.SetCursorPosition(85, 23);
                Console.Write("»»»»»»");
                Console.SetCursorPosition(85, 24);
                Console.Write("»»»»»»");
            }

            DrawRect(62, 22, 15, 3, ConsoleColor.Red, ConsoleColor.Cyan);
            Console.BackgroundColor = ConsoleColor.Red;
            Console.SetCursorPosition(67, 23);
            Console.Write("Delete");

        }
        private static void ShowTicket(TransportTicket ticket)
        {
            ticket.ShowTicket(42, 14);
        }
        private static void EraseRect(int ConsoleX, int ConsoleY, int length, int height, ConsoleColor background)
        {
            Console.BackgroundColor = background;
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    Console.SetCursorPosition(j + ConsoleX, i + ConsoleY);
                    
                    Console.Write(" ");                    
                }
            }
        }
        private static void DrawRect(int ConsoleX, int ConsoleY, int length, int height, ConsoleColor color, ConsoleColor background)
        {
            Console.BackgroundColor = background;
            Console.ForegroundColor = ConsoleColor.Black;

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    Console.SetCursorPosition(j + ConsoleX, i + ConsoleY);
                    if (i == 0)
                        Console.Write("▄");
                    else if (i == height - 1)
                        Console.Write("▀");
                    else if (j == 0 || j == length - 1)
                        Console.Write("█");
                    else
                    {
                        Console.BackgroundColor = color;
                        Console.Write(" ");
                        Console.BackgroundColor = background;
                    }
                }
            }
        }
        private static void DrawButtonsForTicketMenu()
        {

        }
        private static void DrawDeleteTicketMenu()
        {
            DrawRect(35, 7, 66, 20, ConsoleColor.Cyan, ConsoleColor.DarkYellow);
            DrawRect(54, 9, 25, 3, ConsoleColor.Magenta, ConsoleColor.Cyan);
            Console.BackgroundColor = ConsoleColor.Magenta;
            Console.SetCursorPosition(54 + (25 - 13) / 2, 10);
            Console.Write("Delete Ticket");

            DrawRect(12, 8, 16, 5, ConsoleColor.Red, ConsoleColor.DarkYellow);
            Console.BackgroundColor = ConsoleColor.Red;
            Console.SetCursorPosition(18, 10);
            Console.Write("Back");
        }


        private static void CreateTicketMenu()
        {
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.Clear();
            TransportTicket ticket = new TransportTicket();
            bool[] checkForVariables = new bool[7] { false, false, false, false, false, false, false };
            DrawAddTicketMenu(ticket);

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
                            if (record.MouseEvent.dwMousePosition.Y == 5 &&
                                   record.MouseEvent.dwMousePosition.X > 51 && record.MouseEvent.dwMousePosition.X <= 67)  // Back
                            {
                                SetType(ticket);
                                checkForVariables[0] = true;
                            }
                            else if (record.MouseEvent.dwMousePosition.Y == 9 &&
                                   record.MouseEvent.dwMousePosition.X > 35 && record.MouseEvent.dwMousePosition.X <= 59)  // Back
                            {
                                SetLocation(ticket, true);
                                checkForVariables[1] = true;
                            }
                            else if (record.MouseEvent.dwMousePosition.Y == 9 &&
                                   record.MouseEvent.dwMousePosition.X > 60 && record.MouseEvent.dwMousePosition.X <= 84)  // Back
                            {
                                SetLocation(ticket, false);
                                checkForVariables[2] = true;
                            }
                            else if (record.MouseEvent.dwMousePosition.Y == 15 &&
                                   record.MouseEvent.dwMousePosition.X > 35 && record.MouseEvent.dwMousePosition.X <= 59)  // Back
                            {
                                SetTime(ticket, true);
                                checkForVariables[3] = true;
                            }
                            else if (record.MouseEvent.dwMousePosition.Y == 15 &&
                                   record.MouseEvent.dwMousePosition.X > 60 && record.MouseEvent.dwMousePosition.X <= 84)  // Back
                            {
                                SetTime(ticket, false);
                                checkForVariables[4] = true;
                            }
                            else if (record.MouseEvent.dwMousePosition.Y == 20 &&
                                   record.MouseEvent.dwMousePosition.X > 35 && record.MouseEvent.dwMousePosition.X <= 59)  // Back
                            {
                                SetStatus(ticket);
                                checkForVariables[5] = true;
                            }
                            else if (record.MouseEvent.dwMousePosition.Y == 20 &&
                                   record.MouseEvent.dwMousePosition.X > 60 && record.MouseEvent.dwMousePosition.X <= 84)  // Back
                            {
                                SetPrice(ticket);
                                checkForVariables[6] = true;
                            }
                            else if (record.MouseEvent.dwMousePosition.Y == 26 &&
                                   record.MouseEvent.dwMousePosition.X > 43 && record.MouseEvent.dwMousePosition.X <= 56)  // Back
                            {
                                Console.Clear();
                                return;
                            }
                            else if (record.MouseEvent.dwMousePosition.Y == 26 &&
                                   record.MouseEvent.dwMousePosition.X > 62 && record.MouseEvent.dwMousePosition.X <= 75)  // Back
                            {
                                bool falseExist = false;
                                foreach (var item in checkForVariables)
                                {
                                    if (item == false)
                                    {
                                        falseExist = true;
                                        break;
                                    }
                                }
                                if (falseExist == true)
                                    SaveTicket(ticket, false);
                                else
                                {
                                    SaveTicket(ticket, true);
                                    return;
                                }
                            }

                            break;
                        }
                }
            }
        }
        private static void DrawRectForChoice(int ConsoleX, int ConsoleY, int length, string text)
        {
            Console.ForegroundColor = ConsoleColor.White;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    Console.SetCursorPosition(ConsoleX + j, ConsoleY + i);
                    if (i == 0 && j == 0)
                        Console.Write("╔");
                    else if (i == 0 && j == length - 1)
                        Console.Write("╗");
                    else if (i == 2 && j == 0)
                        Console.Write("╚");
                    else if (i == 2 && j == length - 1)
                        Console.Write("╝");
                    else if (i == 0 || i == 2)
                        Console.Write("═");
                    else if (j == 0 || j == length - 1)
                        Console.Write("║");
                    else
                        Console.Write(" ");
                }
            }
            Console.SetCursorPosition(ConsoleX + 2, ConsoleY);
            Console.Write(text);
        }
        private static void DrawExtensionRect(int ConsoleX, int ConsoleY, int length, int height)
        {
            height += 2;
            for (int i = 1; i < height; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    Console.SetCursorPosition(ConsoleX + j, ConsoleY + i);
                    if (i == 0 && j == 0)
                        Console.Write("╔");
                    else if (i == 0 && j == length - 1)
                        Console.Write("╗");
                    else if (i == height - 1 && j == 0)
                        Console.Write("╚");
                    else if (i == height - 1 && j == length - 1)
                        Console.Write("╝");
                    else if (i == 0 || i == height - 1)
                        Console.Write("═");
                    else if (j == 0 || j == length - 1)
                        Console.Write("║");
                    else
                        Console.Write(" ");
                }
            }
        }
        private static void EraseExtensionRect(int ConsoleX, int ConsoleY, int length, int height)
        {
            height += 2;
            for (int i = 1; i < height; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    Console.SetCursorPosition(ConsoleX + j, ConsoleY + i);
                    Console.Write(" ");
                }
            }
        }
        private static void SetLocation(TransportTicket ticket, bool isFrom)
        {
            int cursorX;
            int cursorY;
            if (isFrom == true)
            {
                DrawRectForChoice(35, 8, 25, " From ");
                cursorX = 38;
                cursorY = 10;
            }
            else
            {
                DrawRectForChoice(60, 8, 25, " To ");
                cursorX = 63;
                cursorY = 10;
            }
            var str = File.ReadAllText("Locations.json");
            List<Location> locations = JsonConvert.DeserializeObject<List<Location>>(str);

            int check = 0;  // Can be => 0, 1, 2 
            int startIndex = 0;
            bool isEnterPressed = false;

            while (isEnterPressed == false)
            {
                Console.ForegroundColor = ConsoleColor.White;
                DrawExtensionRect(cursorX, cursorY, 16, 2);
                for (int i = 0; i < 2; i++)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    if (check == i)
                    {
                        Console.BackgroundColor = ConsoleColor.Yellow;
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.SetCursorPosition(cursorX + 3, cursorY + 1 + i);
                        Console.Write(locations[i + startIndex].Country);
                        Console.BackgroundColor = ConsoleColor.Magenta;
                    }
                    else
                    {
                        Console.SetCursorPosition(cursorX + 3, cursorY + 1 + i);
                        Console.Write(locations[i + startIndex].Country);
                    }
                }
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.UpArrow:
                        if (check != 0)
                            check--;
                        else if (startIndex > 0)
                            startIndex--;
                        break;
                    case ConsoleKey.DownArrow:
                        if (check != 1)
                            check++;
                        else if (startIndex + 2 < locations.Count)
                            startIndex++;
                        break;

                    case ConsoleKey.Enter:
                        isEnterPressed = true;
                        break;
                }
            }
            int countryIndex = startIndex + check;
            check = 0;
            startIndex = 0;
            isEnterPressed = false;
            while (isEnterPressed == false)
            {
                Console.ForegroundColor = ConsoleColor.White;
                DrawExtensionRect(cursorX, cursorY, 16, 2);
                for (int i = 0; i < 2; i++)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    if (check == i)
                    {
                        Console.BackgroundColor = ConsoleColor.Yellow;
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.SetCursorPosition(cursorX + 3, cursorY + 1 + i);
                        Console.Write(locations[countryIndex].Cities[i + startIndex]);
                        Console.BackgroundColor = ConsoleColor.Magenta;
                    }
                    else
                    {
                        Console.SetCursorPosition(cursorX + 3, cursorY + 1 + i);
                        Console.Write(locations[countryIndex].Cities[i + startIndex]);
                    }
                }
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.UpArrow:
                        if (check != 0)
                            check--;
                        else if (startIndex > 0)
                            startIndex--;
                        break;
                    case ConsoleKey.DownArrow:
                        if (check != 1)
                            check++;
                        else if (startIndex + 2 < locations[countryIndex].Cities.Count)
                            startIndex++;
                        break;

                    case ConsoleKey.Enter:
                        isEnterPressed = true;
                        break;
                }
            }
            int cityIndex = startIndex + check;

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.SetCursorPosition(cursorX, cursorY - 1);
            Console.Write(locations[countryIndex].Country + "," + locations[countryIndex].Cities[cityIndex]);
            EraseExtensionRect(cursorX, cursorY, 16, 2);

            if (isFrom == true)
            {
                ticket.FromCountry = locations[countryIndex].Country;
                ticket.FromCity = locations[countryIndex].Cities[cityIndex];
            }
            else
            {
                ticket.ToCountry = locations[countryIndex].Country;
                ticket.ToCity = locations[countryIndex].Cities[cityIndex];
            }
        }
        private static void _GetHourAndMinute(out int hour, out int minute, string input)
        {
            string hourString = "";
            string minuteString = "";
            int i = 0;
            for (; i < input.Length && input[i] != ':' && input[i] != '.' && input[i] != ' '; i++)
            {
                hourString += input[i];
            }
            for (++i; i < input.Length; i++)
            {
                minuteString += input[i];
            }
            try
            {
                hour = int.Parse(hourString);
                minute = int.Parse(minuteString);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        private static int _GetNumberOfElementsInInteger(int num)
        {
            int count = 0;
            if (num < 0)
                num *= -1;
            while (num > 0)
            {
                num /= 10;
                count++;
            }
            return count;
        }
        private static void SetType(TransportTicket ticket)
        {
            DrawRectForChoice(51, 4, 18, " Type ");
            string[] type = new string[3] { "Airplane", "Train", "Bus" };
            int index = 0;
            bool isEnterPressed = false;

            while (isEnterPressed == false)
            {
                Console.SetCursorPosition(55, 5);
                Console.Write("          <>");
                if (index == 0)
                {
                    Console.SetCursorPosition(55, 5);
                    Console.WriteLine(type[index]);
                }
                else if (index == 1)
                {
                    Console.SetCursorPosition(57, 5);
                    Console.WriteLine(type[index]);
                }
                else if (index == 2)
                {
                    Console.SetCursorPosition(58, 5);
                    Console.WriteLine(type[index]);
                }
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.UpArrow:
                        if (index > 0)
                            index--;
                        break;
                    case ConsoleKey.DownArrow:
                        if (index < 2)
                            index++;
                        break;
                    case ConsoleKey.Enter:
                        ticket.Status = type[index];
                        isEnterPressed = true;
                        break;
                }
            }
            ticket.Type = type[index];
            Console.SetCursorPosition(65, 5);
            Console.Write("  ");
        }
        private static void SetTime(TransportTicket ticket, bool isLeftTime)
        {
            int cursorX;
            int cursorY;

            if (isLeftTime == true)
            {
                cursorX = 35;
                cursorY = 14;
                DrawRectForChoice(cursorX, cursorY, 25, " Left Time ");
            }
            else
            {
                cursorX = 60;
                cursorY = 14;
                DrawRectForChoice(cursorX, cursorY, 25, " Arrive Time ");
            }
        SetDate:
            try
            {
                if (isLeftTime == true)
                {
                    Console.SetCursorPosition(cursorX + 2, cursorY + 1);
                    Console.Write("Enter Date: ");
                    ticket.LeftTime = DateTime.Parse(Console.ReadLine());
                    Console.SetCursorPosition(cursorX + 2, cursorY + 1);
                    Console.Write("                      ");

                    if (DateTime.Now > ticket.LeftTime)
                    {
                        Console.BackgroundColor = ConsoleColor.DarkRed;
                        Console.SetCursorPosition(cursorX + 5, cursorY + 3);
                        Console.Write("Time travel error");
                        Console.ReadKey();
                        Console.BackgroundColor = ConsoleColor.Magenta;
                        Console.SetCursorPosition(cursorX + 5, cursorY + 3);
                        Console.Write("                 ");
                        goto SetDate;
                    }
                }
                else
                {
                    Console.SetCursorPosition(cursorX + 2, cursorY + 1);
                    Console.Write("Enter Date: ");
                    ticket.ArriveTime = DateTime.Parse(Console.ReadLine());
                    Console.SetCursorPosition(cursorX + 2, cursorY + 1);
                    Console.Write("                      ");
                    if (DateTime.Now > ticket.ArriveTime)
                    {
                        Console.BackgroundColor = ConsoleColor.DarkRed;
                        Console.SetCursorPosition(cursorX + 5, cursorY + 3);
                        Console.Write("Time travel error");
                        Console.ReadKey();
                        Console.BackgroundColor = ConsoleColor.Magenta;
                        Console.SetCursorPosition(cursorX + 5, cursorY + 3);
                        Console.Write("                 ");
                        goto SetDate;
                    }
                }
            }
            catch (Exception)
            {
                Console.SetCursorPosition(cursorX + 2, cursorY + 1);
                Console.Write("                      ");
                Console.BackgroundColor = ConsoleColor.DarkRed;
                Console.SetCursorPosition(cursorX + 1, cursorY + 3);
                Console.Write("Input Date Properly");
                Console.ReadKey();
                Console.BackgroundColor = ConsoleColor.Magenta;
                Console.SetCursorPosition(cursorX + 1, cursorY + 3);
                Console.Write("                   ");
                goto SetDate;
            }
        SetHour:
            try
            {
                if (isLeftTime == true)
                {
                    Console.SetCursorPosition(cursorX + 2, cursorY + 1);
                    Console.Write("Enter Hour: ");
                    string input = Console.ReadLine();
                    int hour, minute;
                    _GetHourAndMinute(out hour, out minute, input);
                    TimeSpan ts = new TimeSpan(hour, minute, 0);
                    ticket.LeftTime = ticket.LeftTime.Date + ts;
                    Console.SetCursorPosition(cursorX + 2, cursorY + 1);
                    Console.Write("                      ");
                    if (DateTime.Now > ticket.LeftTime || (ticket.ArriveTime != DateTime.Parse("1/1/1") && ticket.LeftTime >= ticket.ArriveTime))
                    {
                        Console.BackgroundColor = ConsoleColor.DarkRed;
                        Console.SetCursorPosition(cursorX + 5, cursorY + 3);
                        Console.Write("Time travel error");
                        Console.ReadKey();
                        Console.BackgroundColor = ConsoleColor.Magenta;
                        Console.SetCursorPosition(cursorX + 5, cursorY + 3);
                        Console.Write("                 ");
                        goto SetHour;
                    }
                }
                else
                {
                    Console.SetCursorPosition(cursorX + 2, cursorY + 1);
                    Console.Write("Enter Hour: ");
                    string input = Console.ReadLine();
                    int hour, minute;
                    _GetHourAndMinute(out hour, out minute, input);
                    TimeSpan ts = new TimeSpan(hour, minute, 0);
                    ticket.ArriveTime = ticket.ArriveTime.Date + ts;
                    Console.SetCursorPosition(cursorX + 2, cursorY + 1);
                    Console.Write("                      ");
                    if (DateTime.Now > ticket.ArriveTime || (ticket.LeftTime != DateTime.Parse("1/1/1") && ticket.LeftTime >= ticket.ArriveTime))
                    {
                        Console.BackgroundColor = ConsoleColor.DarkRed;
                        Console.SetCursorPosition(cursorX + 5, cursorY + 3);
                        Console.Write("Time travel error");
                        Console.ReadKey();
                        Console.BackgroundColor = ConsoleColor.Magenta;
                        Console.SetCursorPosition(cursorX + 5, cursorY + 3);
                        Console.Write("                 ");
                        goto SetDate;
                    }
                }
            }
            catch (Exception)
            {
                Console.SetCursorPosition(cursorX + 2, cursorY + 1);
                Console.Write("                      ");
                Console.BackgroundColor = ConsoleColor.DarkRed;
                Console.SetCursorPosition(cursorX + 1, cursorY + 3);
                Console.Write("Input Hour Properly");
                Console.ReadKey();
                Console.BackgroundColor = ConsoleColor.Magenta;
                Console.SetCursorPosition(cursorX + 1, cursorY + 3);
                Console.Write("                   ");
                goto SetHour;
            }

            Console.SetCursorPosition(cursorX + 2, cursorY + 1);
            if (isLeftTime == true)
            {
                Console.Write(ticket.LeftTime.ToString());
            }
            else
            {
                Console.Write(ticket.ArriveTime.ToString());

            }
        }
        private static void SetPrice(TransportTicket ticket)
        {
        setPrice:
            DrawRectForChoice(60, 19, 25, " Price ");
            Console.SetCursorPosition(66, 20);
            try
            {
                ticket.Price = int.Parse(Console.ReadLine());

                if (ticket.Price <= 0)
                {
                    Console.BackgroundColor = ConsoleColor.DarkRed;
                    Console.SetCursorPosition(65, 22);
                    Console.Write("Invalid Price");
                    Console.ReadKey();
                    Console.BackgroundColor = ConsoleColor.Magenta;
                    Console.SetCursorPosition(65, 22);
                    Console.Write("             ");
                    goto setPrice;
                }
            }
            catch (Exception)
            {
                Console.BackgroundColor = ConsoleColor.DarkRed;
                Console.SetCursorPosition(65, 22);
                Console.Write("Invalid Price");
                Console.ReadKey();
                Console.BackgroundColor = ConsoleColor.Magenta;
                Console.SetCursorPosition(65, 22);
                Console.Write("             ");
                goto setPrice;
            }
            Console.SetCursorPosition(66 + _GetNumberOfElementsInInteger(ticket.Price) + 1, 20);
            Console.Write("AZN");
        }
        private static void SetStatus(TransportTicket ticket)
        {
            DrawRectForChoice(35, 19, 25, " Status ");
            string[] status = new string[2] { "Business", "Normal" };
            int index = 0;
            bool isEnterPressed = false;

            while (isEnterPressed == false)
            {
                Console.ForegroundColor = ConsoleColor.White;
                DrawExtensionRect(39, 21, 16, 2);
                for (int i = 0; i < 2; i++)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    if (index == i)
                    {
                        Console.BackgroundColor = ConsoleColor.Yellow;
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.SetCursorPosition(39 + 3, 22 + i);
                        Console.Write(status[i]);
                        Console.BackgroundColor = ConsoleColor.Magenta;
                    }
                    else
                    {
                        Console.SetCursorPosition(39 + 3, 22 + i);
                        Console.Write(status[i]);
                    }
                }
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.UpArrow:
                        if (index > 0)
                            index--;
                        break;
                    case ConsoleKey.DownArrow:
                        if (index < 1)
                            index++;
                        break;
                    case ConsoleKey.Enter:
                        ticket.Status = status[index];
                        isEnterPressed = true;
                        break;
                }
            }
            EraseExtensionRect(39, 21, 16, 2);
            Console.SetCursorPosition(43, 20);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(ticket.Status);
        }
        private static void SaveTicket(TransportTicket ticket, bool isSave)
        {
            if (isSave == true)
            {
                ticket.SaveTicket();
                Console.BackgroundColor = ConsoleColor.Blue;
                Console.SetCursorPosition(50, 28);
                Console.Write("Ticket Has Been Created");
                Console.ReadKey();
                Console.BackgroundColor = ConsoleColor.Magenta;
                Console.SetCursorPosition(50, 28);
                Console.Write("                       ");
            }
            else
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.SetCursorPosition(48, 28);
                Console.Write("Please fill all the gaps");
                Console.ReadKey();
                Console.BackgroundColor = ConsoleColor.Magenta;
                Console.SetCursorPosition(48, 28);
                Console.Write("                        ");
            }
        }
        private static void DrawSaveOrExitButton(int cursorX, int cursorY, bool SaveOrBack) // true:Save, false: Back
        {
            if (SaveOrBack == true)
                Console.ForegroundColor = ConsoleColor.Green;
            else
                Console.ForegroundColor = ConsoleColor.Red;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 15; j++)
                {
                    Console.SetCursorPosition(cursorX + j, cursorY + i);
                    if (i == 0 && j == 0)
                        Console.Write("╔");
                    else if (i == 0 && j == 14)
                        Console.Write("╗");
                    else if (i == 2 && j == 0)
                        Console.Write("╚");
                    else if (i == 2 && j == 14)
                        Console.Write("╝");
                    else if (i == 0 || i == 2)
                        Console.Write("═");
                    else if (j == 0 || j == 14)
                        Console.Write("║");
                    else
                    {
                        if (SaveOrBack == true)
                            Console.BackgroundColor = ConsoleColor.Green;
                        else
                            Console.BackgroundColor = ConsoleColor.Red;
                        Console.Write(" ");
                        Console.BackgroundColor = ConsoleColor.Magenta;
                    }
                }
            }
            Console.ForegroundColor = ConsoleColor.Black;
            Console.SetCursorPosition(cursorX + 4, cursorY + 1);
            if (SaveOrBack == true)
            {
                Console.BackgroundColor = ConsoleColor.Green;
                Console.Write("Create");
            }
            else
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.Write(" Back");
            }
            Console.BackgroundColor = ConsoleColor.Magenta;
        }
        private static void DrawBoarder()
        {
            Console.BackgroundColor = ConsoleColor.Magenta;
            Console.ForegroundColor = ConsoleColor.Black;
            for (int i = 2; i <= 30; i++)
            {
                for (int j = 30; j < 90; j++)
                {
                    Console.SetCursorPosition(j, i);
                    if (i == 2 || i == 30 || j == 30 || j == 89)
                        Console.Write("█");
                    else
                        Console.Write(" ");
                }
            }
        }
        private static void DrawAddTicketMenu(TransportTicket ticket)
        {
            DrawBoarder();

            DrawRectForChoice(51, 4, 18, " Type ");
            DrawRectForChoice(35, 8, 25, " From ");
            DrawRectForChoice(60, 8, 25, " To ");
            DrawRectForChoice(35, 14, 25, " Left Time ");
            Console.SetCursorPosition(35 + 5, 15);
            Console.Write(" month/day/year");
            DrawRectForChoice(60, 14, 25, " Arrive Time ");
            Console.SetCursorPosition(60 + 5, 15);
            Console.Write(" month/day/year");
            DrawRectForChoice(35, 19, 25, " Status ");
            DrawRectForChoice(60, 19, 25, " Price ");

            DrawSaveOrExitButton(43, 25, false);
            DrawSaveOrExitButton(62, 25, true);            
        }
    }
}