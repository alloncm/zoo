using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zoo_project
{
    enum Category
    {
        Predator,
        GrassEater,
        Bird,
        Crawl,
        Fish,
        DobleLife,
        Count
    }
    class Animal
    {
        private static int[] nextId = new int [6];


        public int Id { get; }

        public string Name { get; set; }
        
        public int Height { get; set; }

        public int Weight { get; set; }

        public Category category { get; }

        public string SpecialNotes { get; set; }


        public int GetNextId()
        {
            int c = ((int)category);
            nextId[c]++;
            int id = nextId[c];

            //make sure the category is the last digit in the id
            id *= 10;
            id += c;
            return id;
        }
       
        public Animal(string n,Category c, int h, int w,string sn)
        {
            Name = n;
            category = c;
            Height = h;
            Weight = w;
            SpecialNotes = sn;
            Id = GetNextId();
        }

        public void AddSpecialNotes(string note)
        {
            SpecialNotes += '\n' + note;
        }

        public override string ToString()
        {
            string s = "id: " + Id + " ";
            s += "name: " + Name + " ";
            s += "category: " + category.ToString() + " ";
            s += "height: " + Height + " ";
            s += "weight: " + Weight + " ";
            s += "Special Notes: " + SpecialNotes;
            return s;
        }
    }
}
