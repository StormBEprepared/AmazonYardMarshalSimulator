using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ymSim.Entities
{
    class Human
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public string Role { get; set; }
        public int IDnumber { get; set; }

        private static Random random = new Random();
        public static int GenerateID()
        {
            int newID = random.Next(1000000, 9999999);
            return newID;
        }
        public virtual void Noise()
        {
            Console.WriteLine("I am a human.");
        }
        public Human(string name, int age)
        {
            Name = name;
            Age = age;
            IDnumber = GenerateID();
        }
    }
}
