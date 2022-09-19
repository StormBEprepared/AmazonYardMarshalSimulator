using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yard_Sim;

namespace ymSim.Entities
{
    public class Bay
    {
        public string BayNumber { get; set; }
        public bool Filled { get; set; }
        public string VRIDassigned { get; set; }
        public string BayGTDRstatus { get; set; }    //gtdr phases: checked-in, gtdr-out START, gtdr-in START, gtdr-in LEAVE, gtdr-out LEAVE
        public static List<string> GTDRstatuses = new List<string>() { "Checked-In", "GTDR-out START", "GTDR-in Start", "GTDR-in LEAVE", "GTDR-out LEAVE"};

        public Bay(string bayNumber, bool filled, string vRIDassigned, string bayGTDRstatus)
        {
            BayNumber = bayNumber;
            Filled = filled;
            VRIDassigned = vRIDassigned;
            BayGTDRstatus = bayGTDRstatus;
            Console.WriteLine($"Bay {this.BayNumber} was created.");
            Sim.bays.Add(this);
        }

        public static void parkTruck(Bay bay, string VRID, Truck tr)
        {
            if (bay.Filled == true)
            {
                Console.WriteLine("The bay is full!");
            }
            else
            {
                Console.WriteLine($"{tr.Owner} parks {tr.RegPlate} in {bay.BayNumber}. Parked Successfull!");
                bay.Filled = true; bay.BayGTDRstatus = GTDRstatuses[0]; bay.VRIDassigned = VRID;
            }
        }
    }
}
