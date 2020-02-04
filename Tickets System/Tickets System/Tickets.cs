using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace Tickets_System
{
    class Location
    {
        public string Country { get; set; }
        public List<string> Cities = new List<string>();

        public void LoadLocation(string country)
        {
            var str = File.ReadAllText("Locations.json");
            dynamic locs = JsonConvert.DeserializeObject(str);
            foreach (var item in locs)
            {
                if (item["Country"] == country)
                {
                    Country = item["Country"];
                    foreach (var itm in item["Cities"])
                    {
                        Cities.Add(itm.ToString());
                    }
                    break;
                }
            }
        }
        private void SaveLocation()
        {
            List<Location> loc = null;
            var str = File.ReadAllText("Locations.json");
            loc = JsonConvert.DeserializeObject<List<Location>>(str);
            if (loc != null)
            {
                loc.Add(this);
            }
            else
            {
                loc = new List<Location>();
                loc.Add(this);
            }

            var ser = JsonConvert.SerializeObject(loc, Formatting.Indented);
            File.WriteAllText("Locations.json", ser);
        }
        public void AddLocation()
        {
            Country = "Ukraine";
            Cities.Add("Kiev");
            Cities.Add("Lviv");
            Cities.Add("Odessa");
            Cities.Add("Donesk");
            Cities.Add("Vinnytsia");
            Cities.Add("Ternopil");
            SaveLocation();
        }
        public void ShowLocation()
        {
            Console.WriteLine(Country);
            foreach (var item in Cities)
            {
                Console.WriteLine(item);
            }
        }
    }
    class TicketPicture
    {
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string Line3 { get; set; }
        public string Line4 { get; set; }

        public void LoadPicture(string Type)
        {
            int num = -1;
            if (Type == "Train")
                num = 0;
            else if (Type == "Bus")
                num = 1;
            else if (Type == "Airplane")
                num = 2;

            var ser = File.ReadAllText("TicketPicture.json");
            TicketPicture[] pictures = JsonConvert.DeserializeObject<TicketPicture[]>(ser);
            if (num != -1)
            {
                Line1 = pictures[num].Line1;
                Line2 = pictures[num].Line2;
                Line3 = pictures[num].Line3;
                Line4 = pictures[num].Line4;
            }
            else
            {
                Line1 = "";
                Line2 = "";
                Line3 = "";
                Line4 = "";
            }
        }
        private void SavePicture()
        {
            List<TicketPicture> pics = null;
            var str = File.ReadAllText("TicketPicture.json");
            pics = JsonConvert.DeserializeObject<List<TicketPicture>>(str);
            if (pics != null)
            {
                pics.Add(this);
            }
            else
            {
                pics = new List<TicketPicture>();
                pics.Add(this);
            }

            var ser = JsonConvert.SerializeObject(pics, Formatting.Indented);
            File.WriteAllText("TicketPicture.json", ser);
        }
        public void AddPicture(string line1, string line2, string line3, string line4)
        {
            Line1 = line1;
            Line2 = line2;
            Line3 = line3;
            Line4 = line4;

            SavePicture();
        }
        private void DrawPictureBoarder(int consoleX, int consoleY, ConsoleColor color)
        {
            Console.BackgroundColor = color;
            Console.ForegroundColor = ConsoleColor.Black;
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 15; j++)
                {
                    Console.SetCursorPosition(consoleX + j, consoleY + i);
                    if (i == 0)
                        Console.Write("▄");
                    else if (i == 5)
                        Console.Write("▀");
                    else if (j == 0 || j == 14)
                        Console.Write("█");
                }
            }
        }
        public void ShowPicture(int consoleX, int consoleY, ConsoleColor color)
        {
            DrawPictureBoarder(consoleX, consoleY, color);
            Console.BackgroundColor = ConsoleColor.DarkYellow;
            Console.SetCursorPosition(consoleX + 1, consoleY + 1);
            Console.Write(Line1);
            Console.SetCursorPosition(consoleX + 1, consoleY + 2);
            Console.Write(Line2);
            Console.SetCursorPosition(consoleX + 1, consoleY + 3);
            Console.Write(Line3);
            Console.SetCursorPosition(consoleX + 1, consoleY + 4);
            Console.Write(Line4);
        }
    }
    class Ticket
    {
        public string FromCountry { get; set; }
        public string FromCity { get; set; }
        public string ToCountry { get; set; }
        public string ToCity { get; set; }
        public DateTime LeftTime { get; set; }
        public DateTime ArriveTime { get; set; }
        public string Status { get; set; }
        public int Price { get; set; }
    }
    class TransportTicket : Ticket
    {
        public string Type { get; set; }
        [JsonIgnore]
        TicketPicture ticketPicture = new TicketPicture();
        public TransportTicket()
        {
            LeftTime = DateTime.Parse("1/1/1");
            ArriveTime = DateTime.Parse("1/1/1");
        }

        public static List<TransportTicket> LoadAllTickets()
        {
            var str = File.ReadAllText("Tickets.json");
            List<TransportTicket> tickets = JsonConvert.DeserializeObject<List<TransportTicket>>(str);
            return tickets;
        }
        public void SaveTicket()
        {
            List<TransportTicket> tickets = null;
            var str = File.ReadAllText("Tickets.json");
            tickets = JsonConvert.DeserializeObject<List<TransportTicket>>(str);
            if (tickets != null)
            {
                tickets.Add(this);
            }
            else
            {
                tickets = new List<TransportTicket>();
                tickets.Add(this);
            }
            var ser = JsonConvert.SerializeObject(tickets, Formatting.Indented);
            File.WriteAllText("Tickets.json", ser);
        }

        public override bool Equals(object obj)
        {
            if (obj is TransportTicket t)
                if (t == this)
                    return true;
                else return false;
            return false;
        }
        public static bool operator!= (TransportTicket a, TransportTicket b)
        {
            if (a == b)
                return false;
            return true;
        }
        public static bool operator ==(TransportTicket a, TransportTicket b)
        {
            if (a.ArriveTime == b.ArriveTime && a.FromCity == b.FromCity && a.FromCountry == b.FromCountry &&
                a.LeftTime == b.LeftTime && a.Price == b.Price && a.Status == b.Status && a.ToCity == b.ToCity &&
                a.ToCountry == b.ToCountry && a.Type == b.Type)
                return true;
            else return false;
        }
        private void DrawTicketBoarder(int consoleX, int consoleY, ConsoleColor color)
        {
            Console.BackgroundColor = color;
            Console.ForegroundColor = ConsoleColor.Black;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 53; j++)
                {
                    Console.SetCursorPosition(consoleX + j, consoleY + i);
                    if (i == 0)
                    {
                        Console.BackgroundColor = ConsoleColor.Cyan;
                        Console.Write("▄");
                        Console.BackgroundColor = color;
                    }
                    else if (i == 7)
                    {
                        Console.BackgroundColor = ConsoleColor.Cyan;
                        Console.Write("▀");
                        Console.BackgroundColor = color;
                    }
                    else if (j == 0 || j == 52)
                        Console.Write("█");
                    else
                        Console.Write(" ");
                }
            }
        }
        public void ShowTicket(int consoleX, int consoleY)
        {
            DrawTicketBoarder(consoleX, consoleY, ConsoleColor.DarkGreen);
            ticketPicture.LoadPicture(Type);
            ticketPicture.ShowPicture(consoleX + 2, consoleY + 1, ConsoleColor.DarkGreen);

            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.SetCursorPosition(consoleX + 18, consoleY + 1);
            Console.Write("Type:" + Type + "    Status:" + Status);
            Console.SetCursorPosition(consoleX + 18, consoleY + 2);
            Console.Write("LeftTime:   " + LeftTime.ToShortDateString() + " " + LeftTime.ToShortTimeString());
            Console.SetCursorPosition(consoleX + 18, consoleY + 3);
            Console.Write("ArriveTime: " + ArriveTime.ToShortDateString() + " " + ArriveTime.ToShortTimeString());
            Console.SetCursorPosition(consoleX + 18, consoleY + 4);
            Console.Write("From: " + FromCountry + ", " + FromCity);
            Console.SetCursorPosition(consoleX + 18, consoleY + 5);
            Console.Write("To:   " + ToCountry + ", " + ToCity);
            Console.SetCursorPosition(consoleX + 25, consoleY + 6);
            Console.Write("Price: " + Price + " AZN");
        }
        public static bool ShowTicket(int consoleX, int consoleY, int num)
        {
            List<TransportTicket> tickets = LoadAllTickets();
            if (num < 0 || num >= tickets.Count)
                throw new IndexOutOfRangeException();

            tickets[num].ShowTicket(consoleX, consoleY);
            return true;
        }
    }

    class IboBankCard
    {
        public string CardNumber { get; set; }
        public int CCV { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }

        public bool SetName()
        {
            Name = Console.ReadLine();
            if (Name.Length < 3)
                return false;
            else return true;
        }
        public bool SetSurname()
        {
            Surname = Console.ReadLine();
            if (Surname.Length < 3)
                return false;
            else return true;
        }
        public bool SetCardNumber()
        {
            string input = Console.ReadLine();
            if (input.Length != 16)
                return false;
            try
            {
                long.Parse(input);
                CardNumber = "";
                int k = 0;
                foreach (var item in input)
                {
                    if (k != 4)
                    {
                        CardNumber += item;
                        k++;
                    }
                    else
                    {
                        CardNumber += " " + item;
                        k = 1;
                    }
                }
                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }
        public bool SetCCV()
        {
            string input = Console.ReadLine();
            if (input.Length != 3)
                return false;
            try
            {
                CCV = int.Parse(input);
                if (CCV < 0)
                    return false;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool SetMonth()
        {
            string input = Console.ReadLine();
            if (input.Length > 2)
                return false;
            try
            {
                Month = int.Parse(input);
                if (Month > 0 && Month <= 12)
                    return true;
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool SetYear()
        {
            string input = Console.ReadLine();
            if (input.Length != 2)
                return false;
            try
            {
                Year = int.Parse(input);
                if (Year > 19)
                    return true;
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
