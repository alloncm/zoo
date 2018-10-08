using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;

namespace zoo_project
{
    
    class Program
    {
        static void Main(string[] args)
        {
            System.IO.BinaryReader fin = new System.IO.BinaryReader(File.Open("database.sqlite",FileMode.OpenOrCreate));
            fin.Close();
            DataBase db = new DataBase("database.sqlite");
            db.Load();
            bool quit = false;
            do
            {
                System.Console.WriteLine("\n'Add' to add animal\n'Print' to print the animals\n'Delete' to remove animal\n'Quit' to save and exit the program");
                string command = System.Console.ReadLine();
                switch (command)
                {
                    case "Add":
                        Add(db);
                        break;
                    case "Print":
                        System.Console.WriteLine("enter the animal Category\nfor predator 0\nfor GrassEater 1\nfor bird 2\nfor crawl 3\nfor fish 4\nfor double life 5\nto print all 6");
                        int c = GetInt();
                        if (c>=0 && c<6)
                        {
                            db.PrintTable(((Category)c).ToString());
                        }
                        else
                        {
                            db.Print();
                        }
                        break;
                    case "Delete":
                        Delete(db);
                        break;
                    case "Quit":
                        quit = true;
                        break;
                    default:
                        System.Console.WriteLine("unrecognized command");
                        break;
                }
            }
            while (!quit);
            db.Save();
            System.Console.Read();
        }
        public static void Add(DataBase db)
        {
            /*
                Predator=0,
                GrassEater=1,
                Bird=2,
                Crawl=3,
                Fish=4,
                DobleLife=5,
             */
            System.Console.WriteLine("enter the animal name");
            string name = System.Console.ReadLine();
            System.Console.WriteLine("enter the animal height");
            int height = GetInt();
            System.Console.WriteLine("enter the animal wieght");
            int wieght = GetInt();
            System.Console.WriteLine("enter any notes on the animal");
            string notes = System.Console.ReadLine();
            System.Console.WriteLine("enter the animal Category\nfor predator 0\nfor GrassEater 1\nfor bird 2\nfor crawl 3\nfor fish 4\nfor double life5");
            int c = GetInt();
            Animal a = new Animal(name, (Category)c, height, wieght, notes);
            db.AddAnimal(a);
        }
        public static void Delete(DataBase db)
        {
            System.Console.WriteLine("enter name to delete");
            string name = System.Console.ReadLine();
            if(db.Delete(name))
            {
                System.Console.WriteLine(name + " deleted succesfully");
            }
            else
            {
                System.Console.WriteLine(name + " does not exist");
            }
        }
        public static int GetInt()
        {
            int x = 0;
            try
            {
                x = Convert.ToInt32(System.Console.ReadLine());
            }
            catch(FormatException)
            {
                GetInt();
            }
            return x;
        }
    }
}
