using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
namespace zoo_project
{
    class DataBase
    {
        private string fileName;
        private SQLiteConnection dbConnection;
        private List<Animal> loadedAnimals;
        private List<Animal> newAnimals;
        private List<string> tables;

        public DataBase(string fn)
        {
            fileName = fn;

            string connection = "Data Source=" + fileName + ";" + "Version=3;";
            dbConnection = new SQLiteConnection(connection);
            dbConnection.Open();

            newAnimals = new List<Animal>();
            loadedAnimals = new List<Animal>();
            tables = new List<string>();
            for(int i=0;i<6;i++)
            {
                tables.Add("");
            }
        }

        public void AddAnimal(Animal a)
        {
            newAnimals.Add(a);
            if (!tables.Exists(x => x == a.category.ToString()))
            {
                int t = (int)a.category;
                tables.Insert((int)a.category, a.category.ToString());
            }
        }

        private void LoadAnimal(Animal a)
        {
            loadedAnimals.Add(a);
            if (!tables.Exists(x => x == a.category.ToString()))
            {
                tables.Insert((int)a.category, a.category.ToString());
            }
        }

        private void InsertToDataBase(Animal a)
        {
            if (!tables.Exists(x => x == a.category.ToString()))
            {
                tables.Insert((int)a.category, a.category.ToString());
                string s = "CREATE TABLE " + a.category.ToString() + "(id INT PRIMARY KEY, name TEXT UNIQUE, height INT, weight INT,notes TEXT)";
                SQLiteCommand c = new SQLiteCommand(s, dbConnection);
                c.ExecuteNonQuery();
            }

            string sql = "INSERT INTO " + a.category.ToString() + "(id,name,height,weight,notes) VALUES(" + a.Id + ",'" + a.Name + "'," + a.Height + "," + a.Weight + ",'" + a.SpecialNotes + "');";
            SQLiteCommand command = new SQLiteCommand(sql, dbConnection);
            command.ExecuteNonQuery();
        }

        public void Save()
        {
            for(int i=0;i<newAnimals.Count;i++)
            {
                InsertToDataBase(newAnimals[i]);
            }
            System.Console.WriteLine("data saved succesfuly");
        }

        public void Load()
        {
            string sql = "";
            for(int i=0;i<(int)Category.Count;i++)
            {
                //checks if the the table exist
                sql = "SELECT * FROM sqlite_master WHERE type='table' AND name='" + ((Category)i).ToString() + "';";
                SQLiteCommand command = new SQLiteCommand(sql, dbConnection);
                SQLiteDataReader reader = command.ExecuteReader();
                if (reader.Read() == false)
                {
                    sql = "CREATE TABLE " + ((Category)i).ToString() + "(id INT PRIMARY KEY, name TEXT UNIQUE, height INT, weight INT,notes TEXT);";
                    SQLiteCommand c = new SQLiteCommand(sql, dbConnection);
                    c.ExecuteNonQuery();
                    c.Dispose();
                }
                command.Dispose();
                reader.Close();

                sql = "SELECT * FROM " + ((Category)i).ToString();
                SQLiteCommand com = new SQLiteCommand(sql, dbConnection);
                SQLiteDataReader rea = com.ExecuteReader();
                while(rea.Read())
                {
                    string name = (string)rea["name"];
                    int height = (int)rea["height"];
                    int weight = (int)rea["weight"];
                    string notes = (string)rea["notes"];
                    Animal a = new Animal(name, (Category)i, height, weight, notes);
                    LoadAnimal(a);
                }
                com.Dispose();
                rea.Close();
            }
            System.Console.WriteLine("data loaded succesfuly");
        }

        public void PrintTable(string tn)
        {
            int index = tables.FindIndex(x => x == tn);
            if (index != -1)
            {
                for(int i=0;i<loadedAnimals.Count;i++)
                {
                    if(tables[index]== loadedAnimals[i].category.ToString())
                    {
                        System.Console.WriteLine(loadedAnimals[i].ToString());
                        System.Console.WriteLine();
                    }
                }
                for (int i = 0; i < newAnimals.Count; i++)
                {
                    if (tables[index] == newAnimals[i].category.ToString())
                    {
                        System.Console.WriteLine(newAnimals[i].ToString());
                        System.Console.WriteLine();
                    }
                }
            }
        }

        public void Print()
        {
            for(int i=0;i<tables.Count;i++)
            {
                PrintTable(tables[i]);
            }
        }

       public bool Delete(string name)
        {
            int id = GetId(name);
            if(id==-1)
            { 
                return false;
            }
            int c = id % 10;
            string table = ((Category)c).ToString();
            for(int i=0;i<newAnimals.Count;i++)
            {
                if(newAnimals[i].Id==id)
                {
                    System.Console.WriteLine("deleted " + newAnimals[i].ToString() + " from the database");
                    newAnimals.Remove(newAnimals[i]);
                    return true;
                }
            }
            for(int i=0;i<loadedAnimals.Count;i++)
            {
                if(loadedAnimals[i].Id==id)
                {
                    string sql = "DELETE FROM " + table + " WHERE id=" + id + ";";
                    SQLiteCommand com = new SQLiteCommand(sql, dbConnection);
                    com.ExecuteNonQuery();
                    System.Console.WriteLine("deleted " + loadedAnimals[i].ToString() + " from the database");
                    loadedAnimals.Remove(loadedAnimals[i]);
                    return true;
                }
            }
            return false;
        } 

        public int GetId(string name)
        {
            for (int i = 0; i < newAnimals.Count; i++)
            {
                if(name==newAnimals[i].Name)
                {
                    return newAnimals[i].Id;
                }
            }
            for (int i = 0; i < loadedAnimals.Count; i++)
            {
                if(name==loadedAnimals[i].Name)
                {
                    return loadedAnimals[i].Id;
                }
            }
            return -1;
        }

        public void Update(int id,string sql)
        {
            sql += " WHERE id=" + id + ";";
            SQLiteCommand com = new SQLiteCommand(sql,dbConnection );
            com.ExecuteNonQuery();
        }

        public Animal GetAnimal(int id)
        {
            for (int i = 0; i < newAnimals.Count; i++)
            {
                if (newAnimals[i].Id == id)
                {
                    return newAnimals[i];
                }
            }
            for (int i = 0; i < loadedAnimals.Count; i++)
            {
                if (loadedAnimals[i].Id == id)
                {
                    return loadedAnimals[i];
                }
            }
            return null; 
        }

        ~DataBase()
        {
            dbConnection.Close();
        }
    }
}
