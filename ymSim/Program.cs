using Fare;
using ymSim;
using ymSim.Entities;

namespace Yard_Sim
{
    class Sim
    {
        public static List<Associate> Associates = new List<Associate>();
        public static List<Driver> Drivers = new List<Driver>();
        public static List<Truck> AvailableTrucks = new List<Truck>();
        public static List<Truck> TruckReg = new List<Truck>();
        public static SortedList<string, string> RegVRID = new SortedList<string, string>();
        public static List<Bay> bays = new List<Bay>();//bay and it's filled status (true for filled and false for empty- default on creation)

        private static Random random = new Random();
        static void Main(string[] args)
        {
            Menus menu = new Menus();//The instance of the menu that is called whenever a menu needs to be shown

            string[] names = {"Stefan", "Andrei", "Marcel", "Silviu", "Ceausescu", "Paula", "Laura", "Diana", "Dana", "Sara"};
            List<string> Names=new List<string>();
            Names.AddRange(names);
            for (int i = 0; i < 5; i++)//Creating yard marshalls
            {
                Associate associate = new Associate(Names[random.Next(Names.Count())],random.Next(18,65));
                associate.Noise();
                Names.Remove(associate.Name);
                Associates.Add(associate);
            }
            for (int i = 0; i < 5; i++)//creating Trucks and drivers
            {
                Truck truck = new Truck();
                AvailableTrucks.Add(truck);//Adding the truck to the list of available trucks for work
                TruckReg.Add(truck);
                Driver driver = new Driver(Names[random.Next(Names.Count())], random.Next(18, 65), truck.RegPlate);
                truck.TruckSoundsAtOwner(truck, driver.Name);
                driver.Noise();
                Names.Remove(driver.Name);
                Drivers.Add(driver);
            }
            for (int i = 101; i <=110; i++)
            {
                //The creation of 10 empty bays
                Bay bay = new Bay(i.ToString(), false, string.Empty,string.Empty);
            }

            //The loop for running the menus and functions
            do
            {
                menu.ShowMainMenu();
                string input = Console.ReadLine();
                switch (input)
                {
                    case "1":
                        menu.ShowResourcesMenu();//Done
                        break;
                    case "2":
                        menu.AssignVRIDmenu();//Done
                        break;
                    case"3":
                        menu.CheckInMenu();//Done
                        break;
                    case"4":
                        menu.GTDRMenu();
                        break;
                    case "5":
                        menu.UnloadLoadTrailerMenu();
                        break;
                    default:
                        Console.WriteLine("\n\nUnknown option. Type only the number of the option then press enter!");
                        menu.ShowMainMenu();
                        break;
                }
            } while (true);
            Console.ReadLine();
        }
    }
}