using System;
using System.Data.SQLite;
using System.IO;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "Data Source=hueta.db;Version=3;";

            if (!File.Exists("hueta.db"))
            {
                SQLiteConnection.CreateFile("hueta.db");
                Console.WriteLine("База данных hueta.db создана.");
            }

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string createTableQuery = "CREATE TABLE IF NOT EXISTS LogPass (ID INTEGER PRIMARY KEY AUTOINCREMENT, Log TEXT, Pass TEXT, DateAdded DATETIME DEFAULT CURRENT_TIMESTAMP)";
                using (var command = new SQLiteCommand(createTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
            }

            while (true)
            {
                Console.WriteLine("cifry:");
                Console.WriteLine("1. dobavit login:pass");
                Console.WriteLine("2. pokazat iz db");
                Console.WriteLine("3. poisk po loginu");
                Console.WriteLine("4. exit");
                Console.Write("vibor: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.Clear();
                        Console.WriteLine("format -  Login:pass:");
                        string loginPass = Console.ReadLine();
                        string[] loginPassArr = loginPass.Split(':');
                        if (loginPassArr.Length != 2)
                        {
                            Console.WriteLine("format hueta");
                            break;
                        }
                        string login = loginPassArr[0];
                        string pass = loginPassArr[1];
                        using (var connection = new SQLiteConnection(connectionString))
                        {
                            connection.Open();
                            string insertQuery = $"INSERT INTO LogPass (Log, Pass) VALUES ('{login}', '{pass}')";
                            using (var command = new SQLiteCommand(insertQuery, connection))
                            {
                                command.ExecuteNonQuery();
                            }
                        }
                        Console.WriteLine($"dobavil {login}:{pass}");
                        break;

                    case "2":
                        Console.Clear();
                        using (var connection = new SQLiteConnection(connectionString))
                        {
                            connection.Open();
                            string selectQuery = "SELECT * FROM LogPass";
                            using (var command = new SQLiteCommand(selectQuery, connection))
                            {
                                using (var reader = command.ExecuteReader())
                                {
                                    Console.Clear();
                                    Console.WriteLine("{0,-10} {1,-10} {2,-25}", "ID", "Login", "Pass");
                                    while (reader.Read())
                                    {
                                        Console.WriteLine("{0,-10} {1,-10} {2,-25}", reader.GetInt32(0), reader.GetString(1), reader.GetString(2));
                                    }
                                }
                            }
                        }
                        break;

                    case "3":
                        Console.Clear();
                        Console.WriteLine("Введите логин для поиска:");
                        string searchLogin = Console.ReadLine();
                        using (var connection = new SQLiteConnection(connectionString))
                        {
                            connection.Open();
                            string selectQuery = $"SELECT * FROM LogPass WHERE Log='{searchLogin}'";
                            using (var command = new SQLiteCommand(selectQuery, connection))
                            {
                                using (var reader = command.ExecuteReader())
                                {
                                    Console.Clear();
                                    Console.WriteLine("{0,-10} {1,-10} {2,-25}", "ID", "Login", "Pass");
                                    while (reader.Read())
                                    {
                                        Console.WriteLine("{0,-10} {1,-10} {2,-25}", reader.GetInt32(0), reader.GetString(1), reader.GetString(2));
                                    }
                                }
                            }
                        }
                        break;

                    case "4":
                        Console.Clear();
                        Console.WriteLine("bb");
                        return;

                    default:
                        Console.Clear();
                        Console.WriteLine("nekk vibor");
                        break;
                }
            }
        }
    }
}
