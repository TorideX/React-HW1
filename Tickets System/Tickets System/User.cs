using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Tickets_System
{
    class Human
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public int Age { get; set; }

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
        public bool SetAge()
        {
            try
            {
                Age = int.Parse(Console.ReadLine());
                if (Age < 5 || Age > 100)
                    return false;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
    class Encryptor
    {
        public static string EncryptOrDecrypt(string text, string key)
        {
            var result = new StringBuilder();

            for (int c = 0; c < text.Length; c++)
                result.Append((char)((uint)text[c] ^ (uint)key[c % key.Length]));

            return result.ToString();
        }
    }
    class User : Human, IAutorised
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public int PictureNum { get; set; }

        public override string ToString()
        {
            return $"{Login} {Password} {Name} {Surname}";
        }
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

        public bool EditName()
        {
            if (SetName() == true)
            {
                var str = File.ReadAllText("Users.json");
                dynamic users = JsonConvert.DeserializeObject(str);
                List<User> newUsers = new List<User>();
                foreach (var item in users)
                {
                    if (Login == item["Login"].ToString())
                    {
                        newUsers.Add(this);
                    }
                    else
                    {
                        var tempUser = new User();
                        tempUser.Name = item["Name"];
                        tempUser.Surname = item["Surname"];
                        tempUser.Age = item["Age"];
                        tempUser.Login = item["Login"];
                        tempUser.Password = item["Password"];
                        tempUser.PictureNum = item["PictureNum"];

                        newUsers.Add(tempUser);
                    }
                }
                var ser = JsonConvert.SerializeObject(newUsers, Formatting.Indented);
                File.WriteAllText("Users.json", ser);
                return true;
            }
            else return false;                
        }
        public bool EditSurname()
        {
            if (SetSurname() == true)
            {
                var str = File.ReadAllText("Users.json");
                dynamic users = JsonConvert.DeserializeObject(str);
                List<User> newUsers = new List<User>();
                foreach (var item in users)
                {
                    if (Login == item["Login"].ToString())
                    {
                        newUsers.Add(this);
                    }
                    else
                    {
                        var tempUser = new User();
                        tempUser.Name = item["Name"];
                        tempUser.Surname = item["Surname"];
                        tempUser.Age = item["Age"];
                        tempUser.Login = item["Login"];
                        tempUser.Password = item["Password"];
                        tempUser.PictureNum = item["PictureNum"];

                        newUsers.Add(tempUser);
                    }
                }
                var ser = JsonConvert.SerializeObject(newUsers, Formatting.Indented);
                File.WriteAllText("Users.json", ser);
                return true;
            }
            else return false;
        }
        public bool EditAge()
        {
            if (SetAge() == true)
            {
                var str = File.ReadAllText("Users.json");
                dynamic users = JsonConvert.DeserializeObject(str);
                List<User> newUsers = new List<User>();
                foreach (var item in users)
                {
                    if (Login == item["Login"].ToString())
                    {
                        newUsers.Add(this);
                    }
                    else
                    {
                        var tempUser = new User();
                        tempUser.Name = item["Name"];
                        tempUser.Surname = item["Surname"];
                        tempUser.Age = item["Age"];
                        tempUser.Login = item["Login"];
                        tempUser.Password = item["Password"];
                        tempUser.PictureNum = item["PictureNum"];

                        newUsers.Add(tempUser);
                    }
                }
                var ser = JsonConvert.SerializeObject(newUsers, Formatting.Indented);
                File.WriteAllText("Users.json", ser);
                return true;
            }
            else return false;
        }
        public bool EditPassword()
        {
            do
            {
                Console.SetCursorPosition(98, 21);
                Console.Write("               ");
                Console.SetCursorPosition(101, 21);
            } while (SetPassword() == false);
            string firstPass = Password;
            do
            {
                Console.SetCursorPosition(98, 23);
                Console.Write("               ");
                Console.SetCursorPosition(101, 23);
            } while (SetPassword() == false);

            if (firstPass == Password)
            {
                var str = File.ReadAllText("Users.json");
                dynamic users = JsonConvert.DeserializeObject(str);
                List<User> newUsers = new List<User>();
                foreach (var item in users)
                {
                    if (Login == item["Login"].ToString())
                    {
                        newUsers.Add(this);
                    }
                    else
                    {
                        var tempUser = new User();
                        tempUser.Name = item["Name"];
                        tempUser.Surname = item["Surname"];
                        tempUser.Age = item["Age"];
                        tempUser.Login = item["Login"];
                        tempUser.Password = item["Password"];
                        tempUser.PictureNum = item["PictureNum"];

                        newUsers.Add(tempUser);
                    }
                }
                var ser = JsonConvert.SerializeObject(newUsers, Formatting.Indented);
                File.WriteAllText("Users.json", ser);
                return true;
            }
            else return false;
        }
        public void ChangePicture()
        {
            var str = File.ReadAllText("Pictures.json");
            dynamic pics = JsonConvert.DeserializeObject(str);

            if (PictureNum < pics.Count - 1)
                PictureNum++;
            else
                PictureNum = -1;

            var str1 = File.ReadAllText("Users.json");
            dynamic users = JsonConvert.DeserializeObject(str1);
            var newUsers = new List<User>(); 
            foreach (var item in users)
            {
                if (Login == item["Login"].ToString())
                {
                    newUsers.Add(this);
                }
                else
                {
                    var tempUser = new User();
                    tempUser.Name = item["Name"];
                    tempUser.Surname = item["Surname"];
                    tempUser.Age = item["Age"];
                    tempUser.Login = item["Login"];
                    tempUser.Password = item["Password"];
                    tempUser.PictureNum = item["PictureNum"];

                    newUsers.Add(tempUser);
                }
            }
            var ser = JsonConvert.SerializeObject(newUsers, Formatting.Indented);
            File.WriteAllText("Users.json", ser);
        }

        public void SignIn()
        {            
            List<User> users = null;
            var str = File.ReadAllText("Users.json");
            users = JsonConvert.DeserializeObject<List<User>>(str);
            if (users != null)
            {
                users.Add(this);
            }
            else
            {
                users = new List<User>();
                users.Add(this);
            }

            var ser = JsonConvert.SerializeObject(users, Formatting.Indented);
            File.WriteAllText("Users.json", ser);
        }
        public bool LogIn()
        {
            var ser = File.ReadAllText("Users.json");
            dynamic users = JsonConvert.DeserializeObject(ser);

            bool isLogged = false;
            if (users != null)
            {
                foreach (var item in users)
                {
                    if (Login == item["Login"].ToString() && Password == item["Password"].ToString())
                    {
                        Name = item["Name"].ToString();
                        Surname = item["Surname"].ToString();
                        Age = int.Parse(item["Age"].ToString());
                        PictureNum = int.Parse(item["PictureNum"].ToString());
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
            Name = null;
            Surname = null;
            Age = 0;
            PictureNum = -1;
        }
    }

    class Cart
    {
        public string UserLogin { get; set; }
        public bool isBought { get; set; }
        public TransportTicket ticket { get; set; }

        public void SaveCart()
        {
            var ser = File.ReadAllText("Carts.json");
            List<Cart> carts = JsonConvert.DeserializeObject<List<Cart>>(ser);
            if (carts != null)
            {
                carts.Add(this);
            }
            else
            {
                carts = new List<Cart>();
                carts.Add(this);
            }
            var str = JsonConvert.SerializeObject(carts, Formatting.Indented);
            File.WriteAllText("Carts.json", str);
        }
    }
}
