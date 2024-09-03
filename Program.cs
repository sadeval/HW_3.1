using System;
using System.Linq;
using UserApp.Models;

namespace UserApp
{
    class Program
    {
        static void Main()
        {
            using (var context = new ApplicationContext())
            {
                context.Database.EnsureCreated();

                Console.WriteLine("1. Register");
                Console.WriteLine("2. Login");
                var choice = Console.ReadLine();

                if (choice == "1")
                {
                    Register(context);
                }
                else if (choice == "2")
                {
                    Login(context);
                }
                else
                {
                    Console.WriteLine("Invalid choice.");
                }
            }
        }

        private static void Register(ApplicationContext context)
        {
            Console.Write("Enter username: ");
            var username = Console.ReadLine();

            Console.Write("Enter password: ");
            var password = Console.ReadLine();

            var salt = PasswordHelper.GenerateSalt();
            var hashedPassword = PasswordHelper.HashPassword(password, salt);

            var user = new User
            {
                Username = username,
                PasswordHash = hashedPassword,
                Salt = salt
            };

            context.Users.Add(user);
            context.SaveChanges();

            Console.WriteLine("Registration successful. You can now log in.");
            MainMenu();
        }

        private static void Login(ApplicationContext context)
        {
            Console.Write("Enter username: ");
            var username = Console.ReadLine();

            Console.Write("Enter password: ");
            var password = Console.ReadLine();

            var user = context.Users.SingleOrDefault(u => u.Username == username);
            if (user != null)
            {
                var hashedPassword = PasswordHelper.HashPassword(password, user.Salt);
                if (hashedPassword == user.PasswordHash)
                {
                    Console.WriteLine("Login successful. Welcome to the main menu!");
                    MainMenu();
                }
                else
                {
                    Console.WriteLine("Incorrect password.");
                }
            }
            else
            {
                Console.WriteLine("User not found.");
            }
        }

        private static void MainMenu()
        {
            Console.WriteLine("This is the main menu.");
           
        }
    }
}
