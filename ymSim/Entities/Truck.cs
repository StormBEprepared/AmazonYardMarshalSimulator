using Fare;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yard_Sim;

namespace ymSim.Entities
{
    public class Truck
    {
        public string RegPlate { get; set; }
        public string Owner { get; set; }
        public string VRID { get; set; }
        public bool VRIDstate { get; set; }
        public bool CheckedIn { get; set; }


        private static Random random = new Random();
        public static string RandomVRID(string Reg)
        {
            int length = 8;
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public static string GenerateRegPlate()
        {
            var xeger = new Xeger("([A-Z]{2}[ ][0-9]{2}[ ][A-Z]{3})");//https://regexlib.com/REDetails.aspx?regexp_id=1906 Regex library
            var generatedString = xeger.Generate();
            return generatedString;
        }
        public Truck()
        {
            RegPlate = GenerateRegPlate();
        }
        public void AssignVRID()
        {
            VRID = RandomVRID(RegPlate);

            VRIDstate = true;
            CheckedIn = false;

            Sim.RegVRID.Add(RegPlate, VRID);//Adding the truck to the list of ON DUTY trucks
            Sim.AvailableTrucks.Remove(this);//Removing the truck from the list of available trucks.
        }
        public void TruckSoundsAtOwner(Truck tr, string owner)
        {
            Owner = owner;
            Console.WriteLine("Beep, Beeeeep! (Inline Diesel engine reving...)\n I am {1}, and I love my owner {0}.", Owner, RegPlate);
        }
        public string CheckVRID(Truck truck)
        {
            var truth = string.Format("{0:Ready;0;Done}", true.GetHashCode());
            var unTruth = string.Format("{0:Ready;0;Done}", false.GetHashCode());
            return truck.VRIDstate ? truth : unTruth;
        }
        public void TruckCheckedIn()
        {
            this.CheckedIn = true;

        }
    }
}
