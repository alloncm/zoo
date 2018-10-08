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
                System.Console.WriteLine("\n'Add' to add animal\n'Print' to print the animals\n'PrintC' to Print Category of animals \n'Update' to update info of animal \n'Delete' to remove animal\n'Quit' to save and exit the program");
                string command = System.Console.ReadLine();
                switch (command)
                {
                    case "Add":
                        Add(db);
                        break;
                    case "PrintC":
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
                    case "Print":
                        db.Print();
                        break;
                    case "Update":
                        Update(db);
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
                System.Console.WriteLine("enter valid input");
                GetInt();
            }
            return x;
        }

        public static void Update(DataBase db)
        {
            System.Console.WriteLine("enter the name of the animal to Update");
            string name = System.Console.ReadLine();
            int id = db.GetId(name);
            while(id==-1)
            {
                System.Console.WriteLine("enter valid name");
                System.Console.WriteLine("enter the name of the animal to Update");
                name = System.Console.ReadLine();
                id = db.GetId(name);
            }
            System.Console.WriteLine("enter the number of the attribute you would like to change \n-0 name\n-1 height \n-2 weight \n-3 notes\n");
            int choice = GetInt();
            Animal a = db.GetAnimal(id);
            string sql = "UPDATE " + a.category.ToString() + " SET ";
            bool success = true;
            switch (choice)
            {
                case 0:
                    System.Console.WriteLine("enter new name");
                    a.Name = System.Console.ReadLine();
                    sql += "name=" + a.Name;
                    break;
                case 1:
                    System.Console.WriteLine("enter new height");
                    a.Height = GetInt();
                    sql += "height=" + a.Height;
                    break;
                case 2:
                    System.Console.WriteLine("enter new Weight");
                    a.Weight = GetInt();
                    sql += "weight=" + a.Weight;
                    break;
                case 3:
                    System.Console.WriteLine("to add to the existing Notes enter 1 to replace enter 0");
                    int c = GetInt();
                    if(c==0)
                    {
                        a.SpecialNotes = System.Console.ReadLine();
                    }
                    else
                    {
                        a.SpecialNotes += System.Console.ReadLine();
                    }
                    sql += "notes= " + a.SpecialNotes;
                    break;
                default:
                    System.Console.WriteLine("enter valid input next Time");
                    success = false;
                    break;
            }
            db.Update(id, sql);
            if(success)
            {
                System.Console.WriteLine("animal updated succesfully");
            }
            else
            {
                System.Console.WriteLine("failed to update");
            }
        }
    }
}
