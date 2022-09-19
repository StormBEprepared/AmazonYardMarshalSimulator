using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yard_Sim;

namespace ymSim.Entities
{
    class Associate : Human
    {
        public Associate(string name, int age) : base(name, age)
        {
            Role = "Associate-YM";
        }
        public override void Noise()
        {
            Console.WriteLine("Hi! My name is {0}, I am a {1}!", Name, Role);
        }
        public void DoGTDR(Bay b)
        {
            Console.WriteLine($"{this.Name}: The status of the chosen bay is: {b.BayGTDRstatus}. Doing the GTDR now." +
                $"\n It might take a while... I have to go thgrough all those steps...");
            int newIndexOfGTDR = Bay.GTDRstatuses.IndexOf(b.BayGTDRstatus) + 1;
            b.BayGTDRstatus = Bay.GTDRstatuses[newIndexOfGTDR];
            System.Threading.Thread.Sleep(1000);
            Console.WriteLine($"{this.Name}: Done! The current status of the GTDR is: {b.BayGTDRstatus}");
            if (b.BayGTDRstatus == Bay.GTDRstatuses[4])
            {
                var elem = Sim.RegVRID.Where(x => x.Value == b.VRIDassigned).ToList();
                string RegOfTruck = elem[0].Key;
                var elem2 = Sim.TruckReg.Where(y => y.RegPlate == RegOfTruck).ToList();
                Truck actualTruck=elem2[0];
                actualTruck.VRIDstate = false;
                actualTruck.VRID = string.Empty;
                actualTruck.CheckedIn = false;
                Console.WriteLine("This truck is done. Checking out...");
                System.Threading.Thread.Sleep(1000);
                Console.WriteLine("Done!");
                b.VRIDassigned = string.Empty;
                b.Filled = false;
                Sim.AvailableTrucks.Add(actualTruck);
                Sim.RegVRID.Remove(RegOfTruck);
                var elemBay = Sim.bays.Where(x=>x.BayNumber==b.BayNumber).ToList();
                elemBay[0].VRIDassigned = string.Empty;
                elemBay[0].Filled = false;
                elemBay[0].BayGTDRstatus = string.Empty;
            }
        }
    }
}
