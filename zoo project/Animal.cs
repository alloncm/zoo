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
        private int id;
        private string name;
        private Category category;
        private int height;
        private int weight;
        private string specialNotes;

        private static int[] nextId = new int [6];

        public int Id
        {
            get
            {
                return id;
            }
        }
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }
        public int Height
        {
            get
            {
                return height;
            }
            set
            {
                height = value;
            }
        }
        public int Weight
        {
            get
            {
                return weight;
            }
            set
            {
                weight = value;
            }
        }
        public Category CCategory
        {
            get
            {
                return category;
            }
        }
        public string SpecailNotes
        {
            get
            {
                return specialNotes;
            }
            set
            {
                specialNotes = value;
            }
        }

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
            name = n;
            category = c;
            height = h;
            weight = w;
            specialNotes = sn;
            id = GetNextId();
        }
        public void AddSpecialNotes(string note)
        {
            specialNotes += '\n' + note;
        }
        public override string ToString()
        {
            string s = "id: " + id + " ";
            s += "name: " + name + " ";
            s += "category: " + category.ToString() + " ";
            s += "height: " + height + " ";
            s += "weight: " + weight + "\n";
            s += "Special Notes: \n" + specialNotes;
            return s;
        }
    }
}
