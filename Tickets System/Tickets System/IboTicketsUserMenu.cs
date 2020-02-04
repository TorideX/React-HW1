using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;

namespace Tickets_System
{
    class IboTicketsUserMenu
    {
        class Picture
        {
            public string Line1 { get; set; }
            public string Line2 { get; set; }
            public string Line3 { get; set; }
            public string Line4 { get; set; }
            public string Line5 { get; set; }

            public void LoadPicture(int num = -1)
            {
                var ser = File.ReadAllText("Pictures.json");
                Picture[] pictures = JsonConvert.DeserializeObject<Picture[]>(ser);
                if (num >= 0 && num < pictures.Length)
                {
                    Line1 = pictures[num].Line1;
                    Line2 = pictures[num].Line2;
                    Line3 = pictures[num].Line3;
                    Line4 = pictures[num].Line4;
                    Line5 = pictures[num].Line5;
                }
                else
                {
                    Line1 = "            ";
                    Line2 = "            ";
                    Line3 = " No Picture ";
                    Line4 = "            ";
                    Line5 = "            ";
                }
            }
            private void SavePicture()
            {
                List<Picture> pics = null;
                var str = File.ReadAllText("Pictures.json");
                pics = JsonConvert.DeserializeObject<List<Picture>>(str);
                if (pics != null)
                {
                    pics.Add(this);
                }
                else
                {
                    pics = new List<Picture>();
                    pics.Add(this);
                }

                var ser = JsonConvert.SerializeObject(pics, Formatting.Indented);
                File.WriteAllText("Pictures.json", ser);
            }
            public void AddPicture(string line1, string line2, string line3, string line4, string line5)
            {
                Line1 = line1;
                Line2 = line2;
                Line3 = line3;
                Line4 = line4;
                Line5 = line5;

                SavePicture();
            }
        }
        public static void Menu(User user)
        {
            int MenuNumber = 1;
            Console.Clear();
            DrawMenu(user, MenuNumber);

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
                            if (record.MouseEvent.dwMousePosition.X > 10 && record.MouseEvent.dwMousePosition.X < 30 &&
                                record.MouseEvent.dwMousePosition.Y > 2 && record.MouseEvent.dwMousePosition.Y < 11)
                            {
                                if (MenuNumber != 1)
                                {
                                    MenuNumber = 1;
                                    DrawMenu(user, 1);
                                }
                            }
                            else if (record.MouseEvent.dwMousePosition.X > 10 && record.MouseEvent.dwMousePosition.X < 30 &&
                                record.MouseEvent.dwMousePosition.Y > 11 && record.MouseEvent.dwMousePosition.Y < 20)
                            {
                                if (MenuNumber != 2)
                                {
                                    MenuNumber = 2;
                                    DrawMenu(user, 2);
                                }
                            }
                            else if (record.MouseEvent.dwMousePosition.X > 10 && record.MouseEvent.dwMousePosition.X < 30 &&
                                record.MouseEvent.dwMousePosition.Y > 20 && record.MouseEvent.dwMousePosition.Y < 29)
                            {
                                if (MenuNumber != 3)
                                {
                                    MenuNumber = 3;
                                    DrawMenu(user, 3);
                                }
                            }

                            break;
                        }
                    case NativeMethods.KEY_EVENT:
                        {
                            if (record.KeyEvent.bKeyDown == true && record.KeyEvent.wVirtualKeyCode == 13) // Enter pressed
                            {
                                if (MenuNumber == 1)
                                    TicketsMenu(user);
                                if (MenuNumber == 2)
                                    MyCartMenu(user);
                                if (MenuNumber == 3)
                                    if (MyProfileMenu(user) == false)
                                        return;
                            }
                            break;
                        }
                }
            }
        }
                
        private static void TicketsMenu(User user)
        {
            DrawSelectedPage();
            DrawTicketsMenuChoosen();

            int showOptionNum = 0;
            int sortOptionNum = 0;
            List<TransportTicket> tickets = null;
            int index = 0;
            bool isSearchPressed = false;

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
                            if (record.MouseEvent.dwMousePosition.X >= 53 && record.MouseEvent.dwMousePosition.X <= 70 &&
                                record.MouseEvent.dwMousePosition.Y >= 12 && record.MouseEvent.dwMousePosition.Y <= 14)  // Show Option
                            {
                                ChangeShowOption(ref showOptionNum);
                            }
                            if (record.MouseEvent.dwMousePosition.X >= 79 && record.MouseEvent.dwMousePosition.X <= 96 &&
                                record.MouseEvent.dwMousePosition.Y >= 12 && record.MouseEvent.dwMousePosition.Y <= 14)  // Sort Option
                            {
                                ChangeSortOption(ref sortOptionNum);
                            }
                            if (record.MouseEvent.dwMousePosition.X >= 104 && record.MouseEvent.dwMousePosition.X <= 111 &&
                               record.MouseEvent.dwMousePosition.Y == 13)  // Search
                            {
                                LoadTickets(showOptionNum, sortOptionNum, ref tickets);
                                isSearchPressed = true;

                                index = 0;
                                ShowTicket(index, tickets);
                            }
                            if(isSearchPressed == true)
                            {
                                if (record.MouseEvent.dwMousePosition.X >= 38 && record.MouseEvent.dwMousePosition.X <= 45 &&
                               record.MouseEvent.dwMousePosition.Y >= 19 && record.MouseEvent.dwMousePosition.Y <= 20)   // Back
                                {
                                    if (index != 0)
                                        index--;
                                    else continue;

                                    ShowTicket(index, tickets);
                                }
                                if (record.MouseEvent.dwMousePosition.X >= 107 && record.MouseEvent.dwMousePosition.X <= 116 &&
                               record.MouseEvent.dwMousePosition.Y >= 19 && record.MouseEvent.dwMousePosition.Y <= 20)   // Next
                                {
                                    if (index != tickets.Count - 1)
                                        index++;
                                    else continue;

                                    ShowTicket(index, tickets);
                                }
                                if (record.MouseEvent.dwMousePosition.X >= 69 && record.MouseEvent.dwMousePosition.X <= 81 &&
                               record.MouseEvent.dwMousePosition.Y == 25) // Add to Cart
                                {
                                    AddToCart(user, tickets[index]);
                                    Console.SetCursorPosition(60, 27);
                                    Console.Write("Ticket has been added to your Cart");
                                    Console.ReadKey();
                                    Console.SetCursorPosition(60, 27);
                                    Console.BackgroundColor = ConsoleColor.Cyan;
                                    Console.Write("                                  ");
                                }
                            }

                            break;
                        }
                    case NativeMethods.KEY_EVENT:
                        {
                            if (record.KeyEvent.bKeyDown == true && record.KeyEvent.wVirtualKeyCode == 27) // ESC pressed
                            {
                                DrawMenu(user, 1);
                                return;
                            }
                            break;
                        }
                }
            }
        }
        private static void AddToCart(User user, TransportTicket ticket)
        {
            var cart = new Cart();
            cart.UserLogin = user.Login;
            cart.ticket = ticket;
            cart.isBought = false;

            cart.SaveCart();
        }
        private static void DrawNextBackButton(bool BackExist, bool NextExist)
        {
            Console.ForegroundColor = ConsoleColor.Black;
            if (BackExist)
            {
                DrawRect(37, 18, 10, 4, ConsoleColor.Blue);
                Console.BackgroundColor = ConsoleColor.Blue;
                Console.SetCursorPosition(39,19);
                Console.Write("««««««");
                Console.SetCursorPosition(39,20);
                Console.Write("««««««");
            }
            else
            {
                DrawRect(37, 18, 10, 4, ConsoleColor.DarkGray);
                Console.BackgroundColor = ConsoleColor.DarkGray;
                Console.SetCursorPosition(39, 19);
                Console.Write("««««««");
                Console.SetCursorPosition(39, 20);
                Console.Write("««««««");
            }
            if (NextExist)
            {
                DrawRect(106, 18, 10, 4, ConsoleColor.Blue);
                Console.BackgroundColor = ConsoleColor.Blue;
                Console.SetCursorPosition(108, 19);
                Console.Write("»»»»»»");
                Console.SetCursorPosition(108, 20);
                Console.Write("»»»»»»");
            }
            else
            {
                DrawRect(106, 18, 10, 4, ConsoleColor.DarkGray);
                Console.BackgroundColor = ConsoleColor.DarkGray;
                Console.SetCursorPosition(108, 19);
                Console.Write("»»»»»»");
                Console.SetCursorPosition(108, 20);
                Console.Write("»»»»»»");
            }

            DrawRect(68, 24, 15, 3, ConsoleColor.Green);
            Console.BackgroundColor = ConsoleColor.Green;
            Console.SetCursorPosition(70, 25);
            Console.Write("Add to Cart");

        }
        private static void EraseNextBackButton()
        {
            EraseRect(37, 18, 10, 4);
            EraseRect(106, 18, 10, 4);
            EraseRect(68, 24, 15, 3);
        }
        private static void EraseRect(int ConsoleX, int ConsoleY, int length, int height)
        {
            Console.BackgroundColor = ConsoleColor.Cyan;
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    Console.SetCursorPosition(j + ConsoleX, i + ConsoleY);
                    
                    Console.Write(" ");                    
                }
            }
        }
        private static void DrawRect(int ConsoleX, int ConsoleY, int length, int height, ConsoleColor color)
        {
            Console.BackgroundColor = ConsoleColor.Cyan;
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
                        Console.BackgroundColor = ConsoleColor.Cyan;
                    }
                }
            }
        }
        private static void EraseTicketPicture(int consoleX, int consoleY)
        {
            Console.BackgroundColor = ConsoleColor.Cyan;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 53; j++)
                {
                    Console.SetCursorPosition(consoleX + j, consoleY + i);
                    Console.Write(" ");
                }
            }
        }
        private static void ShowTicket(int index, List<TransportTicket> tickets)
        {
            if (tickets.Count != 0)
            {
                DrawNextBackButton(index > 0, index < tickets.Count - 1);
                tickets[index].ShowTicket(50, 16);
            }
            else
            {
                EraseNextBackButton();
                EraseTicketPicture(50, 16);
                Console.SetCursorPosition(60, 20);
                Console.Write("No Tickets were found...");
            }
        }
        private static void LoadTickets(int showOption, int sortOption, ref List<TransportTicket> tickets)
        {
            tickets = new List<TransportTicket>();
            var allTickets = TransportTicket.LoadAllTickets();
            foreach (var item in allTickets)
            {                
                if (showOption == 1)
                {
                    if (item.Type == "Airplane")
                        tickets.Add(item);
                }
                else if (showOption == 2)
                {
                    if (item.Type == "Train")
                        tickets.Add(item);
                }
                else if (showOption == 3)
                {
                    if (item.Type == "Bus")
                        tickets.Add(item);
                }
                else if (showOption == 0)
                    tickets.Add(item);
            }

            if(sortOption == 1)
                tickets = tickets.OrderBy(ticket => ticket.LeftTime).ToList<TransportTicket>();
            else if (sortOption == 2)
                tickets = tickets.OrderBy(ticket => ticket.Price).ToList<TransportTicket>();
        }
        private static void ChangeShowOption(ref int optionNum)
        {
            string[] options = new string[4] { "All", "Airplane", "Train", "Bus" };
            if (optionNum == 3)
                optionNum = 0;
            else optionNum++;

            DrawRect(52, 11, 20, 5, ConsoleColor.Red);
            Console.SetCursorPosition(55, 13);
            Console.BackgroundColor = ConsoleColor.Red;
            Console.Write("Show: " + options[optionNum]);
        }
        private static void ChangeSortOption(ref int optionNum)
        {
            string[] options = new string[3] { "Non", "by Date", "by Price" };
            if (optionNum == 2)
                optionNum = 0;
            else optionNum++;

            DrawRect(78, 11, 20, 5, ConsoleColor.Red);
            Console.SetCursorPosition(81, 13);
            Console.BackgroundColor = ConsoleColor.Red;
            Console.Write("Sort: " + options[optionNum]);
        }
        private static void DrawTicketsMenuChoosen()
        {
            Console.ForegroundColor = ConsoleColor.Black;
            Console.BackgroundColor = ConsoleColor.Red;
            Console.SetCursorPosition(55, 13);
            Console.Write("Show: All");
            Console.SetCursorPosition(81, 13);
            Console.Write("Sort: Non");
        }
        private static void DrawTicketsMenu()
        {
            DrawMenuHeader("Tickets", ConsoleColor.Magenta);
            Console.ForegroundColor = ConsoleColor.Black;
            Console.BackgroundColor = ConsoleColor.Cyan;
            
            DrawRect(52, 11, 20, 5, ConsoleColor.Red);
            Console.SetCursorPosition(55, 13);
            Console.BackgroundColor = ConsoleColor.Red;
            Console.Write("Show: ");

            DrawRect(78, 11, 20, 5, ConsoleColor.Red);
            Console.SetCursorPosition(81, 13);
            Console.BackgroundColor = ConsoleColor.Red;
            Console.Write("Sort: ");

            DrawRect(103, 12, 10, 3, ConsoleColor.Green);
            Console.SetCursorPosition(105, 13);
            Console.BackgroundColor = ConsoleColor.Green;
            Console.Write("Search");
        }

        private static void MyCartMenu(User user)
        {
            DrawSelectedPage();
            List<Cart> myCart = LoadMyCart(user);
            bool isChoosingMenu = true;
            bool isSessionFull = false;
            if (myCart.Count == 0)
            {
                DrawEmptyCartMenu();
                DrawMenu(user, 2);
                return;
            }
            int index = 0;
            DrawCartMenuChoosen(myCart[index].ticket);
            DrawCartMenuName(37, 9, "Choose", "Ticket");

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
                            if (isChoosingMenu == true)
                            {
                                if (record.MouseEvent.dwMousePosition.X >= 37 && record.MouseEvent.dwMousePosition.X <= 42 &&
                                    record.MouseEvent.dwMousePosition.Y == 22) // Prev Button
                                {
                                    if (index != 0)
                                        index--;
                                    else continue;
                                    EraseRect(33, 15, 23, 7);
                                    DrawChooseTicket(myCart[index].ticket);
                                }
                                if (record.MouseEvent.dwMousePosition.X >= 46 && record.MouseEvent.dwMousePosition.X <= 51 &&
                                    record.MouseEvent.dwMousePosition.Y == 22) // Next Button
                                {
                                    if (index != myCart.Count - 1)
                                        index++;
                                    else continue;
                                    EraseRect(33, 15, 23, 7);
                                    DrawChooseTicket(myCart[index].ticket);
                                }
                                if (record.MouseEvent.dwMousePosition.X >= 41 && record.MouseEvent.dwMousePosition.X <= 46 &&
                                    record.MouseEvent.dwMousePosition.Y == 14) // Show Button
                                {
                                    ShowTicketBigger(myCart[index].ticket);
                                }
                                if (record.MouseEvent.dwMousePosition.X >= 39 && record.MouseEvent.dwMousePosition.X <= 48 &&
                                    record.MouseEvent.dwMousePosition.Y == 24) // Choose
                                {
                                    isChoosingMenu = false;
                                    EraseButtonsChooseTicket();
                                    DrawCartMenuName(72, 9, "Payment", "Session");
                                    DrawBuyMenu();
                                    DrawButtonsForPaymentSession();
                                }
                            }
                            else
                            {
                                if (record.MouseEvent.dwMousePosition.X >= 72 && record.MouseEvent.dwMousePosition.X <= 89 &&
                                    record.MouseEvent.dwMousePosition.Y == 15) // Enter Information
                                {
                                    isSessionFull = true;
                                    DrawBuyMenu();
                                    BuyTicket();
                                }
                                if (record.MouseEvent.dwMousePosition.X >= 68 && record.MouseEvent.dwMousePosition.X <= 77 &&
                                    record.MouseEvent.dwMousePosition.Y == 24) // Back
                                {
                                    ErasePaymentSession();
                                    DrawButtonsForChooseTicket();
                                    DrawCartMenuName(37, 9, "Choose", "Ticket");
                                    isChoosingMenu = true;
                                }
                                if (record.MouseEvent.dwMousePosition.X >= 81 && record.MouseEvent.dwMousePosition.X <= 90 &&
                                    record.MouseEvent.dwMousePosition.Y == 24) // Buy
                                {
                                    if (isSessionFull == true)
                                    {
                                        Console.SetCursorPosition(66, 27);
                                        Console.BackgroundColor = ConsoleColor.Green;
                                        Console.Write("Payment Succesfully finished!");
                                        Console.ReadKey();
                                        Console.BackgroundColor = ConsoleColor.Cyan;
                                        Console.SetCursorPosition(66, 27);
                                        Console.Write("                             ");

                                        DeleteBoughtTicketFromCart(myCart[index]);
                                        myCart = LoadMyCart(user);
                                        DrawMenu(user, 2);
                                        return;
                                    }
                                    else
                                    {
                                        Console.SetCursorPosition(68, 27);
                                        Console.BackgroundColor = ConsoleColor.Red;
                                        Console.Write("Payment Session is Empty");
                                        Console.ReadKey();
                                        Console.BackgroundColor = ConsoleColor.Cyan;
                                        Console.SetCursorPosition(68, 27);
                                        Console.Write("                             ");
                                    }
                                }
                            }

                            break;
                        }
                    case NativeMethods.KEY_EVENT:
                        {
                            if (record.KeyEvent.bKeyDown == true && record.KeyEvent.wVirtualKeyCode == 27) // ESC pressed
                            {
                                DrawMenu(user, 2);
                                return;
                            }
                            break;
                        }
                }
            }
        }
        private static void DeleteBoughtTicketFromCart(Cart cart)
        {
            var str = File.ReadAllText("Carts.json");
            List<Cart> carts = JsonConvert.DeserializeObject<List<Cart>>(str);
            List<Cart> newCarts = new List<Cart>();
            foreach (var item in carts)
            {
                if (item.ticket == cart.ticket && item.UserLogin == cart.UserLogin)
                    continue;
                newCarts.Add(item);
            }
            var ser = JsonConvert.SerializeObject(newCarts, Formatting.Indented);
            File.WriteAllText("Carts.json", ser);
        }
        private static void ShowTicketBigger(TransportTicket ticket)
        {
            ticket.ShowTicket(62, 14);
            Console.SetCursorPosition(0, 0);
            Console.ReadKey();
            EraseRect(62, 14, 53, 8);            
        }
        private static void DrawEmptyCartMenu()
        {
            DrawRect(51, 15, 50, 6, ConsoleColor.Red);
            Console.BackgroundColor = ConsoleColor.Red;
            Console.SetCursorPosition(62, 17);
            Console.Write("    Your Cart is Empty");
            Console.SetCursorPosition(62, 18);
            Console.Write("Add some tickets to your Cart");

            Console.SetCursorPosition(0, 0);
            Console.ReadKey();
        }
        private static void DrawIboBankCard(int consoleX, int consoleY, IboBankCard card)
        {
            DrawRect(consoleX, consoleY, 25, 7, ConsoleColor.DarkYellow);
            Console.BackgroundColor = ConsoleColor.DarkYellow;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.SetCursorPosition(consoleX + (25 - "IboBank".Length) / 2, consoleY + 1);
            Console.Write("IboBank");
            Console.SetCursorPosition(consoleX + 3, consoleY + 2);
            Console.Write(card.CardNumber);
            Console.SetCursorPosition(consoleX + 3, consoleY + 4);
            Console.Write($"THRU {card.Month}/{card.Year}");
            Console.SetCursorPosition(consoleX + 25 - 7, consoleY + 4);
            Console.Write("DEBIT");
            Console.SetCursorPosition(consoleX + 3, consoleY + 5);
            Console.Write(card.Name + " " + card.Surname);
        }
        private static void BuyTicket()
        {
            IboBankCard card = new IboBankCard();
            Console.BackgroundColor = ConsoleColor.DarkMagenta;
            Console.ForegroundColor = ConsoleColor.Yellow;
            do
            {
                Console.SetCursorPosition(72, 15);
                Console.Write("                  ");
                Console.SetCursorPosition(73, 15);
            } while (card.SetCardNumber() == false);
            do
            {
                Console.SetCursorPosition(73, 17);
                Console.Write("  ");
                Console.SetCursorPosition(73, 17);
            } while (card.SetMonth() == false);
            do
            {
                Console.SetCursorPosition(76, 17);
                Console.Write("  ");
                Console.SetCursorPosition(76, 17);
            } while (card.SetYear() == false);
            do
            {
                Console.SetCursorPosition(84, 17);
                Console.Write("     ");
                Console.SetCursorPosition(85, 17);
            } while (card.SetCCV() == false);
            do
            {
                Console.SetCursorPosition(72 + 2, 19);
                Console.Write("             ");
                Console.SetCursorPosition(72 + 3, 19);
            } while (card.SetName() == false);
            do
            {
                Console.SetCursorPosition(72 + 2, 21);
                Console.Write("             ");
                Console.SetCursorPosition(72 + 3, 21);
            } while (card.SetSurname() == false);

            DrawIboBankCard(96, 15, card);
        }
        private static void DrawBuyMenu()
        {
            Console.ForegroundColor = ConsoleColor.Black;
            Console.BackgroundColor = ConsoleColor.Cyan;
            Console.SetCursorPosition(60, 15);
            Console.Write("Card Number");
            Console.SetCursorPosition(60, 17);
            Console.Write("Expire Time");
            Console.SetCursorPosition(60, 19);
            Console.Write("   Name");
            Console.SetCursorPosition(60, 21);
            Console.Write("  Surname");

            Console.ForegroundColor = ConsoleColor.Black;
            Console.BackgroundColor = ConsoleColor.DarkMagenta;
            Console.SetCursorPosition(72, 15);
            Console.Write("                  ");
            Console.SetCursorPosition(73, 17);
            Console.Write("  ");
            Console.BackgroundColor = ConsoleColor.Cyan;
            Console.Write("/");
            Console.BackgroundColor = ConsoleColor.DarkMagenta;
            Console.SetCursorPosition(76, 17);
            Console.Write("  ");
            Console.SetCursorPosition(80, 17);
            Console.BackgroundColor = ConsoleColor.Cyan;
            Console.Write("CCV:");
            Console.BackgroundColor = ConsoleColor.DarkMagenta;
            Console.SetCursorPosition(84, 17);
            Console.Write("     ");

            Console.SetCursorPosition(72 + 2, 19);
            Console.Write("             ");
            Console.SetCursorPosition(72 + 2, 21);
            Console.Write("             ");
        }
        private static void DrawButtonsForPaymentSession()
        {
            DrawRect(67, 23, 12, 3, ConsoleColor.Red);
            Console.BackgroundColor = ConsoleColor.Red;
            Console.SetCursorPosition(67 + 4, 24);
            Console.Write("Back");
            DrawRect(80, 23, 12, 3, ConsoleColor.Green);
            Console.BackgroundColor = ConsoleColor.Green;
            Console.SetCursorPosition(80 + 5, 24);
            Console.Write("Buy");
        }
        private static void ErasePaymentSession()
        {
            EraseRect(72, 9, 15, 4); // Menu Name (Payment Session)
            EraseRect(96, 15, 25, 7); // CardPicture
            EraseRect(60, 15, 30, 7); // Session
            EraseRect(67, 23, 30, 3); // Buttons
        }
        private static void EraseButtonsChooseTicket()
        {
            EraseRect(37, 9, 15, 4); // Menu Name (Choose Ticket)

            EraseRect(41, 14, 6, 1);  // Show
            EraseRect(37, 22, 15, 5); // Bottom Buttons
        }
        private static void DrawButtonsForChooseTicket()
        {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.SetCursorPosition(37, 22);
            Console.Write(" Prev ");
            Console.SetCursorPosition(46, 22);
            Console.Write(" Next ");
            Console.SetCursorPosition(41, 14);
            Console.Write(" Show ");

            DrawRect(38, 23, 12, 3, ConsoleColor.Green);
            Console.BackgroundColor = ConsoleColor.Green;
            Console.SetCursorPosition(38 + 3, 24);
            Console.Write("Choose");
        }
        private static void DrawCartMenuName(int consoleX, int consoleY, string text1, string text2)
        {
            DrawRect(consoleX, consoleY, 15, 4, ConsoleColor.DarkYellow);
            Console.BackgroundColor = ConsoleColor.DarkYellow;
            Console.SetCursorPosition(consoleX + (15 - text1.Length) / 2, consoleY + 1);
            Console.Write(text1);
            Console.SetCursorPosition(consoleX + (15 - text1.Length) / 2, consoleY + 1);
            Console.SetCursorPosition(consoleX + (15 - text2.Length) / 2, consoleY + 2);
            Console.Write(text2);
            Console.SetCursorPosition(consoleX + (15 - text2.Length) / 2, consoleY + 2);
            Console.BackgroundColor = ConsoleColor.Cyan;
        }
        private static void DrawCartBoarder(int cursorX, int cursorY, ConsoleColor color)
        {
            Console.ForegroundColor = ConsoleColor.Black;
            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 23; j++)
                {
                    Console.SetCursorPosition(cursorX + j, cursorY + i);
                    Console.BackgroundColor = ConsoleColor.Cyan;
                    if (i == 0 && j == 0)
                        Console.Write("╔");
                    else if (i == 0 && j == 22)
                        Console.Write("╗");
                    else if (i == 6 && j == 0)
                        Console.Write("╚");
                    else if (i == 6 && j == 22)
                        Console.Write("╝");
                    else if (i == 0 || i == 6)
                        Console.Write("═");
                    else if (j == 0 || j == 22)
                        Console.Write("║");
                    else
                    {
                        Console.BackgroundColor = color;
                        Console.Write(" ");
                    }
                }
            }
        }
        private static void DrawChooseTicket(TransportTicket ticket)
        {
            DrawCartBoarder(33, 15, ConsoleColor.DarkGreen);
            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.SetCursorPosition(35, 16);
            Console.Write("From:");
            Console.SetCursorPosition(35, 17);            
            Console.Write(ticket.FromCountry + ", " + ticket.FromCity);
            Console.SetCursorPosition(35, 18);
            Console.Write("To:");
            Console.SetCursorPosition(35, 19);
            Console.Write(ticket.ToCountry + ", " + ticket.ToCity);
            Console.SetCursorPosition(35, 20);
            Console.Write("Price:  " + ticket.Price + " AZN");

            DrawButtonsForChooseTicket();
        }
        private static List<Cart> LoadMyCart(User user)
        {
            var str = File.ReadAllText("Carts.json");
            List<Cart> carts = JsonConvert.DeserializeObject<List<Cart>>(str);
            List<Cart> myCart = new List<Cart>();
            foreach (var item in carts)
            {
                if (item.UserLogin == user.Login)
                    myCart.Add(item);
            }
            return myCart;
        }
        private static void DrawCartMenuChoosen(TransportTicket ticket)
        {            
            DrawChooseTicket(ticket);            
        }
        private static void DrawCartMenu()
        {
            DrawMenuHeader("My Cart", ConsoleColor.Magenta);
        }
        
        private static bool MyProfileMenu(User user)
        {
            DrawSelectedPage();
            DrawEditButton();

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
                            if (record.MouseEvent.dwMousePosition.X >= 90 && record.MouseEvent.dwMousePosition.X <= 95 &&
                                record.MouseEvent.dwMousePosition.Y == 13)  // Edit Name
                            {
                                EditName(user);
                            }
                            else if (record.MouseEvent.dwMousePosition.X >= 90 && record.MouseEvent.dwMousePosition.X <= 95 &&
                                record.MouseEvent.dwMousePosition.Y == 15)  // Edit Surname
                            {
                                EditSurname(user);
                            }
                            else if (record.MouseEvent.dwMousePosition.X >= 90 && record.MouseEvent.dwMousePosition.X <= 95 &&
                                record.MouseEvent.dwMousePosition.Y == 17)  // Edit Age
                            {
                                EditAge(user);
                            }
                            else if (record.MouseEvent.dwMousePosition.X >= 90 && record.MouseEvent.dwMousePosition.X <= 95 &&
                                record.MouseEvent.dwMousePosition.Y == 21)  // Edit Password
                            {
                                EditPassword(user);
                            }
                            else if (record.MouseEvent.dwMousePosition.X >= 43 && record.MouseEvent.dwMousePosition.X <= 51 &&
                                record.MouseEvent.dwMousePosition.Y == 19)  // Change Picture
                            {
                                ChangePicture(user);
                            }
                            else if (record.MouseEvent.dwMousePosition.X >= 35 && record.MouseEvent.dwMousePosition.X <= 45 &&
                                record.MouseEvent.dwMousePosition.Y >= 24 && record.MouseEvent.dwMousePosition.Y <= 26) // Log Out
                            {
                                user.LogOut();
                                Console.Clear();
                                Autorisation.DrawMenu(true);
                                return false;
                            }

                            break;
                        }
                    case NativeMethods.KEY_EVENT:
                        {
                            if (record.KeyEvent.bKeyDown == true && record.KeyEvent.wVirtualKeyCode == 27) // ESC pressed
                            {
                                DrawMenu(user, 3);
                                return true;
                            }
                            break;
                        }
                }
            }

        }
        private static void DrawMenuHeader(string text, ConsoleColor color)
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 30; j++)
                {
                    Console.SetCursorPosition(60 + j, 4 + i);
                    Console.BackgroundColor = ConsoleColor.Cyan;
                    if (i == 0 && j == 0)
                        Console.Write("╔");
                    else if (i == 0 && j == 29)
                        Console.Write("╗");
                    else if (i == 4 && j == 0)
                        Console.Write("╚");
                    else if (i == 4 && j == 29)
                        Console.Write("╝");
                    else if (i == 0 || i == 4)
                        Console.Write("═");
                    else if (j == 0 || j == 29)
                        Console.Write("║");
                    else
                    {
                        Console.BackgroundColor = color;
                        Console.Write(" ");
                    }
                }
            }
            Console.BackgroundColor = color;
            Console.ForegroundColor = ConsoleColor.Yellow;

            Console.SetCursorPosition(60 + (30 - text.Length) / 2, 6);
            Console.Write(text);
        }
        private static void DrawRectForOption(int x, string text)
        {
            int y = 2;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.BackgroundColor = ConsoleColor.Yellow;
            for (int i = 1; i < 5; i++)
            {
                Console.SetCursorPosition(x, y + i);
                for (int j = 0; j < 20 + 2; j++)
                {
                    if (i == 4)
                    {
                        Console.BackgroundColor = ConsoleColor.Cyan;
                        Console.Write("▀");
                    }
                    else if (j == 0 || j == 21)
                    {
                        Console.BackgroundColor = ConsoleColor.Cyan;
                        Console.Write("█");
                    }
                    else
                    {
                        Console.BackgroundColor = ConsoleColor.Yellow;
                        Console.Write(" ");
                    }
                }
            }
            Console.BackgroundColor = ConsoleColor.Yellow;
            Console.SetCursorPosition(x + 5, y + 2);
            Console.Write(text);
        }
        private static void ShowProfileInfo(User user)
        {
            Console.BackgroundColor = ConsoleColor.Cyan;
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.SetCursorPosition(62, 13);
            Console.Write("  Name: ");
            Console.SetCursorPosition(62, 15);
            Console.Write("Surname: ");
            Console.SetCursorPosition(62, 17);
            Console.Write("  Age: ");
            Console.SetCursorPosition(62, 19);
            Console.Write(" Login: ");
            Console.SetCursorPosition(62, 21);
            Console.Write("Password: ");


            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;

            Console.SetCursorPosition(73, 13);         // Name
            Console.Write("               ");
            Console.SetCursorPosition(76, 13);
            Console.Write(user.Name);

            Console.SetCursorPosition(73, 15);         // Surname
            Console.Write("               ");
            Console.SetCursorPosition(76, 15);
            Console.Write(user.Surname);

            Console.SetCursorPosition(73, 17);         // Age
            Console.Write("               ");
            Console.SetCursorPosition(76, 17);
            Console.Write(user.Age);

            Console.SetCursorPosition(73, 19);         // Login
            Console.Write("               ");
            Console.SetCursorPosition(76, 19);
            Console.Write(user.Login);

            Console.SetCursorPosition(73, 21);         // Password
            Console.Write("               ");
            Console.SetCursorPosition(76, 21);
            Console.Write(Encryptor.EncryptOrDecrypt(user.Password, "parol"));
        }
        private static void DrawSelectedPage()
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            for (int i = 2; i < 30; i++)
            {
                for (int j = 30; j < 125; j++)
                {
                    Console.SetCursorPosition(j, i);
                    if (i == 2 || i == 29)
                        Console.Write("█");
                    else
                    {
                        Console.SetCursorPosition(30, i);
                        Console.Write("█");
                        Console.SetCursorPosition(124, i);
                        Console.Write("█");
                        break;
                    }
                }
            }
        }
        private static void DrawProfilePicture(User user)
        {
            Console.BackgroundColor = ConsoleColor.Cyan;
            Console.ForegroundColor = ConsoleColor.Black;
            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 14; j++)
                {
                    Console.SetCursorPosition(40 + j, 12 + i);
                    if (i == 0)
                        Console.Write("▄");
                    else if (i == 6)
                        Console.Write("▀");
                    else if (j == 0 || j == 13)
                        Console.Write("█");
                }
            }
            #region DrawPicture
            var pic = new Picture();
            pic.LoadPicture(user.PictureNum);
            Console.BackgroundColor = ConsoleColor.Yellow;
            Console.SetCursorPosition(41, 13);
            Console.Write(pic.Line1);
            Console.SetCursorPosition(41, 14);
            Console.Write(pic.Line2);
            Console.SetCursorPosition(41, 15);
            Console.Write(pic.Line3);
            Console.SetCursorPosition(41, 16);
            Console.Write(pic.Line4);
            Console.SetCursorPosition(41, 17);
            Console.Write(pic.Line5);
            #endregion
        }
        private static void DrawMyProfileMenu(User user)
        {
            DrawMenuHeader("My Profile", ConsoleColor.Magenta);
            DrawProfilePicture(user);
            ShowProfileInfo(user);
        }
        private static void ChangePicture(User user)
        {
            user.ChangePicture();

            DrawProfilePicture(user);
        }
        private static void EditName(User user)
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;

            do
            {
                Console.SetCursorPosition(98, 13);
                Console.Write("               ");
                Console.SetCursorPosition(101, 13);
            } while (user.EditName() == false);

            DrawMenu(user, 3);
            DrawMyProfileMenu(user);
            DrawSelectedPage();
            DrawEditButton();
        }
        private static void EditSurname(User user)
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;

            do
            {
                Console.SetCursorPosition(98, 15);
                Console.Write("               ");
                Console.SetCursorPosition(101, 15);
            } while (user.EditSurname() == false);

            DrawMenu(user, 3);
            DrawMyProfileMenu(user);
            DrawSelectedPage();
            DrawEditButton();
        }
        private static void EditAge(User user)
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;

            do
            {
                Console.SetCursorPosition(98, 17);
                Console.Write("               ");
                Console.SetCursorPosition(101, 17);
            } while (user.EditAge() == false);

            DrawMenu(user, 3);
            DrawMyProfileMenu(user);
            DrawSelectedPage();
            DrawEditButton();
        }
        private static void EditPassword(User user)
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(98, 23);
            Console.Write("               ");

            string passwordBackup = user.Password;
            if (user.EditPassword() == false)
            {
                user.Password = passwordBackup;
                Console.SetCursorPosition(96, 25);
                Console.BackgroundColor = ConsoleColor.Cyan;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("Enter Password Carefully");
                Console.ReadKey();
            }

            DrawMenu(user, 3);
            DrawMyProfileMenu(user);
            DrawSelectedPage();
            DrawEditButton();
        }
        private static void DrawEditButton()
        {
            Console.BackgroundColor = ConsoleColor.Green;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.SetCursorPosition(90, 13);
            Console.Write(" Edit ");
            Console.SetCursorPosition(90, 15);
            Console.Write(" Edit ");
            Console.SetCursorPosition(90, 17);
            Console.Write(" Edit ");
            Console.SetCursorPosition(90, 21);
            Console.Write(" Edit ");

            Console.SetCursorPosition(43, 19);
            Console.Write(" Change ");

            Console.BackgroundColor = ConsoleColor.Cyan;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.SetCursorPosition(35, 24);
            Console.Write("▄▄▄▄▄▄▄▄▄▄▄");
            Console.SetCursorPosition(35, 25);
            Console.Write("█         █");
            Console.SetCursorPosition(35, 26);
            Console.Write("▀▀▀▀▀▀▀▀▀▀▀");
            Console.BackgroundColor = ConsoleColor.Red;
            Console.SetCursorPosition(36, 25);
            Console.Write(" Log Out ");
        }

        public static void DrawMenu(User user, int MenuNumber = 1)
        {
            #region DrawMenuBoarder
            Console.ForegroundColor = ConsoleColor.Black;
            for (int i = 2; i < 30; i++)
            {
                for (int j = 10; j < 125; j++)
                {
                    Console.SetCursorPosition(j, i);
                    Console.BackgroundColor = ConsoleColor.Cyan;

                    if (i == 2 || i == 30 - 1 || j == 10 || j == 125 - 1 || j == 30)
                        Console.Write("█");
                    else if (j < 30)
                    {
                        Console.BackgroundColor = ConsoleColor.DarkCyan;
                        if (i == 11 || i == 20)
                            Console.Write("█");
                        else switch (MenuNumber)
                            {
                                case 1:
                                    if (i < 11) Console.BackgroundColor = ConsoleColor.DarkBlue;
                                    Console.Write(" ");
                                    break;
                                case 2:
                                    if (i > 11 && i < 20) Console.BackgroundColor = ConsoleColor.DarkBlue;
                                    Console.Write(" ");
                                    break;
                                case 3:
                                    if (i > 20) Console.BackgroundColor = ConsoleColor.DarkBlue;
                                    Console.Write(" ");
                                    break;
                                default:
                                    Console.Write(" ");
                                    break;
                            }
                    }
                    else
                        Console.Write(" ");
                }
            }
            #endregion

            switch (MenuNumber)
            {
                case 1:
                    Console.BackgroundColor = ConsoleColor.DarkBlue;
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.SetCursorPosition(15, 6);
                    Console.Write("  Tickets");
                    Console.SetCursorPosition(15, 7);
                    Console.Write(" ─────────");
                    Console.BackgroundColor = ConsoleColor.DarkCyan;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.SetCursorPosition(15, 15);
                    Console.Write("  My Cart");
                    Console.SetCursorPosition(15, 16);
                    Console.Write(" ─────────");
                    Console.SetCursorPosition(15, 24);
                    Console.Write(" My Profile");
                    Console.SetCursorPosition(15, 25);
                    Console.Write("────────────");
                    break;
                case 2:
                    Console.BackgroundColor = ConsoleColor.DarkBlue;
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.SetCursorPosition(15, 15);
                    Console.Write("  My Cart");
                    Console.SetCursorPosition(15, 16);
                    Console.Write(" ─────────");
                    Console.BackgroundColor = ConsoleColor.DarkCyan;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.SetCursorPosition(15, 6);
                    Console.Write("  Tickets");
                    Console.SetCursorPosition(15, 7);
                    Console.Write(" ─────────");
                    Console.SetCursorPosition(15, 24);
                    Console.Write(" My Profile");
                    Console.SetCursorPosition(15, 25);
                    Console.Write("────────────");
                    break;
                case 3:
                    Console.BackgroundColor = ConsoleColor.DarkBlue;
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.SetCursorPosition(15, 24);
                    Console.Write(" My Profile");
                    Console.SetCursorPosition(15, 25);
                    Console.Write("────────────");
                    Console.BackgroundColor = ConsoleColor.DarkCyan;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.SetCursorPosition(15, 6);
                    Console.Write("  Tickets");
                    Console.SetCursorPosition(15, 7);
                    Console.Write(" ─────────");
                    Console.SetCursorPosition(15, 15);
                    Console.Write("  My Cart");
                    Console.SetCursorPosition(15, 16);
                    Console.Write(" ─────────");
                    break;
            }

            if (MenuNumber == 1)
                DrawTicketsMenu();
            else if (MenuNumber == 2)
                DrawCartMenu();
            else if (MenuNumber == 3)
                DrawMyProfileMenu(user);
        }
    }
}
