using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ymSim.Entities
{
    class Driver : Human
    {
        public string AllocatedTruck { get; set; }
        public Driver(string name, int age, string allocatedTruck) : base(name, age)
        {
            Role = "Driver";
            AllocatedTruck = allocatedTruck;
        }
        public override void Noise()
        {
            Console.WriteLine("Hi! My name is {0}, I am a {1}!", Name, Role);
        }
        public static void DriveTruck(string location, string name, string reg)
        {
            Console.WriteLine($"{name} drives the truck {reg} to {location}.");
        }
    }
}
