using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yard_Sim;
using ymSim.Entities;

namespace ymSim
{
    public class Menus
    {
        public void ShowMainMenu()
        {
            Console.WriteLine("\t\tMenu:\n" +
                    "Please choose from the following options:\n" +
                    "\n1. See resources: (Drivers, Trucks and Yard-associates)" +
                    "\n2. Assign VRID (truck only)" +
                    "\n3. Do check-In (Yard-associates only)" +
                    "\n4. Do GTDR (Yard-associates only, Checked-In only)" +
                    "\n5. Load/Unload Truck.");
        }

        public void ShowResourcesMenu()
        {
            Console.WriteLine("1.Drivers" +
                "\n2.Yard Marshals" +
                "\n3.Trucks" +
                "\n4.All resources" +
                "\n5.Back.");
            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    foreach (var Driver in Sim.Drivers)
                    {
                        Console.Write($"{Driver.Name} - id: {Driver.IDnumber}; Assigned truck:{Driver.AllocatedTruck}\n");
                    }
                    break;
                case "2":
                    foreach (var Associate in Sim.Associates)
                    {
                        Console.Write($"{Associate.Name} - id: {Associate.IDnumber};\n");
                    }
                    break;
                case "3":
                    foreach (var truck in Sim.AvailableTrucks.Where(x=>x.VRIDstate==false))
                    {
                        Console.WriteLine($"{truck.RegPlate} is available.");
                    }
                    foreach (KeyValuePair<string,string> kvp in Sim.RegVRID)
                    {
                        Console.WriteLine($"{kvp.Key} is ON DUTY with VRID: {kvp.Value}.");
                    }
                    break;
                case "4":
                    foreach (var Driver in Sim.Drivers)
                    {
                        Console.Write($"{Driver.Name} - id: {Driver.IDnumber}; Assigned truck:{Driver.AllocatedTruck}\n");
                    }
                    foreach (var Associate in Sim.Associates)
                    {
                        Console.Write($"{Associate.Name} - id: {Associate.IDnumber};\n");
                    }
                    foreach (var truck in Sim.AvailableTrucks.Where(x => x.VRIDstate == false))
                    {
                        Console.WriteLine($"{truck.RegPlate} is available.");
                    }
                    foreach (KeyValuePair<string, string> kvp in Sim.RegVRID)
                    {
                        Console.WriteLine($"{kvp.Key} is ON DUTY with VRID: {kvp.Value}.");
                    }
                    break;
                case "5":
                    ShowMainMenu();
                    break;
                default:
                    Console.WriteLine("\n\nUnknown selection");
                    ShowResourcesMenu();
                    break;
            }
        }

        public void AssignVRIDmenu()
        {
            SortedList<int, string> list = new SortedList<int, string>();
            Console.WriteLine("Choose the truck for which you want to assign VRID:");
            int i = 1;
            foreach (var truck in Sim.AvailableTrucks)
            {
                Console.WriteLine($"{i}. {truck.RegPlate}");
                list.Add(i, truck.RegPlate);
                i++;
            }

            string option = Console.ReadLine();
            if (option.Length == 1 && list.ContainsKey(Convert.ToInt32(option)))
            {
                var list1 = list.Where(x => x.Key == Convert.ToInt32(option)).ToList();
                Console.WriteLine(list1[0].Value.ToString());//To evidentiate the reg plate of the chosen truck
                var tractor = Sim.AvailableTrucks.Where(x => x.RegPlate == list1[0].Value.ToString()).ToList();
                Truck CurrentTractor = tractor[0];
                CurrentTractor.AssignVRID();
                Console.WriteLine($"Status of {CurrentTractor.RegPlate} with VRID: {CurrentTractor.VRID} is {CurrentTractor.CheckVRID(CurrentTractor).ToString()}.");

            }
            else
            {
                Console.WriteLine("Please choose a reg number from the list! ex: for 1. GF 64 NKX press '1' and ENTER");
                AssignVRIDmenu();
            }
        }

        public void CheckInMenu()
        {
                Console.WriteLine("Please select the truck.");
                int i = 1;
                foreach (var truckster in Sim.TruckReg.Where(trukster=>trukster.CheckedIn==false&&trukster.VRIDstate==true).ToList())//Printing the ON DUTY trucks with reg and VRID (NEEDS TO BE ONLY THE ONES WHICH WERE NOT CHECKED-IN ALREADY)
                {
                    Console.WriteLine($"{i}. {truckster.RegPlate} -> {truckster.VRID}");
                    i++;
                }
                string input = Console.ReadLine();
                var elem = Sim.RegVRID.ElementAt(Int32.Parse(input)-1);//Choosing which truck we want to park

                if (input.Length == 1 && Sim.RegVRID.Contains(elem))//Printing the list of available bays to park in
                {
                    Console.WriteLine("Choose a bay:");
                    int j = 1;
                    foreach (var bay in Sim.bays.Where(x=>x.Filled==false).ToList())
                    {
                        Console.WriteLine($"{j}. {bay.BayNumber}");
                        j++;
                    }
                    string inputBay = Console.ReadLine();
                    var elemBay = Sim.bays.ElementAt(int.Parse(inputBay) - 1);
                    if (elemBay.Filled==false&&elemBay.BayGTDRstatus==String.Empty)
                    {
                        var RegOfTruck = elem.Key;//Getting the registration of the truck from RegVRID index given above
                        var theTruck = Sim.TruckReg.Where(x => x.RegPlate == RegOfTruck).ToList();//Getting the Truck from the combination of TruckReg
                        var TheDriver = Sim.Drivers.Where(x => x.AllocatedTruck == RegOfTruck).ToList();//Getting the driver from the combination of Driver and Allocated Truck
                        Driver.DriveTruck(elemBay.BayNumber, TheDriver[0].Name, RegOfTruck);//YM orders the driver to move the truck into the selected bay
                        Bay.parkTruck(elemBay, elem.Value, theTruck[0]);//Driver successfully move the truck into the given bay
                        int indexOfTruck = Sim.TruckReg.IndexOf(theTruck[0]);
                    Sim.TruckReg[indexOfTruck].TruckCheckedIn();
                    }
                    else
                    {
                        Console.WriteLine("There is no bay with that index. Try again.");
                    }
                }
                else
                {
                    Console.WriteLine("Unable to find the given index. Try again");
                }
        }

        public void GTDRMenu()//gtdr phases: checked-in, gtdr-out START, gtdr-in START, gtdr-in LEAVE, gtdr-out LEAVE
        {
            List<Bay> bays = new List<Bay>(); 
            Console.WriteLine("Please choose the truck that you want to do the GTDR for.");
            int i = 1;
            foreach (var bay in Sim.bays.Where(x => x.VRIDassigned != String.Empty && x.Filled == true && x.BayGTDRstatus != Bay.GTDRstatuses[4]).ToList())
            {
                Console.WriteLine($"{i}. Bay {bay.BayNumber} - VRID: {bay.VRIDassigned} - GTDR status: {bay.BayGTDRstatus}");
                bays.Add(bay);
                i++;
            }
            int choic3 = Int32.Parse(Console.ReadLine());

            //Letting a random yard marshall to do the process
            Random rand = new Random();
            Sim.Associates[rand.Next(0,Sim.Associates.Count)].DoGTDR(bays[choic3-1]);
        }

        public void UnloadLoadTrailerMenu()
        {
            Console.WriteLine("Please choose the bay to unloaded:");
            var GoodToUnloadBays = Sim.bays.Where(x=>x.Filled==true&&x.BayGTDRstatus==Bay.GTDRstatuses[2]).ToList();
            List<Bay> availToUnloadBays = new List<Bay>();
            int i=1;
            if (GoodToUnloadBays.Count>=1)
            {
                foreach (var bay in GoodToUnloadBays)
                {
                    Console.WriteLine($"{i}. Bay {bay.BayNumber} with VRID {bay.VRIDassigned} is good to be unloaded as it has GTDR status as: {bay.BayGTDRstatus}.");
                    availToUnloadBays.Add(bay);
                    i++;
                }
                int inputIndex = int.Parse(Console.ReadLine()) - 1;
                if (inputIndex<=availToUnloadBays.Count+1)
                {
                    if (availToUnloadBays[inputIndex].BayGTDRstatus != Bay.GTDRstatuses[2])
                    {
                        Console.WriteLine($"The bay is not ready to be unloaded. Please check GDTR. Current status is: {availToUnloadBays[inputIndex].BayGTDRstatus} .");
                    }
                    else
                    {
                        Console.WriteLine("Unloading can start!");
                        System.Threading.Thread.Sleep(1000);
                        Console.WriteLine($"Bay {availToUnloadBays[inputIndex].BayNumber} is unloaded/loaded. Please depart!");
                    }
                }
                else
                {
                    Console.WriteLine("Please choose one of the given options! Enter only numbers!");
                }
            }
            else
            {
                Console.WriteLine("There are no trucks in bays. Please do the GDTR if you have something in bays!");
            }
        }
    }
}
