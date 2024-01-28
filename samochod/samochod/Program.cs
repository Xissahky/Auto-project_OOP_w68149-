using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace samochod
{
    public enum AutoType
    {
        SportCar,
        Crossover,
        Pickup,
        Minivan
    }



    public class Auto
    {
        public string Name { get; set; }
        public AutoType Type { get; set; }
        public decimal AmountOfFuel { get; set; }
        public decimal AmountOfTechLiquids { get; set; }
        public int CurrentSpeed { get; set; }
        public bool EngineRunning { get; set; }

        public Auto(string name, AutoType type, decimal fuel, decimal techLiquids)
        {
            Name = name;
            Type = type;
            AmountOfFuel = fuel;
            AmountOfTechLiquids = techLiquids;
            CurrentSpeed = 0;
            EngineRunning = false;
        }
    }

    public class Drive
    {
        public void StartEngine(Auto car)
        {
            if (!car.EngineRunning)
            {
                if (car.AmountOfFuel > 0 && car.AmountOfTechLiquids > 0)
                {
                    car.EngineRunning = true;
                    Console.WriteLine($"{car.Name} engine started.");
                }
                else
                {
                    Console.WriteLine($"{car.Name} cannot start engine due to lack of fuel or technical fluids.");
                }
                
            }
            else
            {
                Console.WriteLine($"{car.Name} engine is already running.");
            }
        }

        public void StopEngine(Auto car)
        {
            if (car.EngineRunning)
            {
                car.EngineRunning = false;
                Console.WriteLine($"{car.Name} engine stopped.");
            }
            else
            {
                Console.WriteLine($"{car.Name} engine is already turned off.");
            }
        }

        public void IncreaseSpeed(Auto car, int speedIncrement)
        {
            if (car.EngineRunning)
            {
                if (car.AmountOfFuel > 0 && car.AmountOfTechLiquids > 0)
                {
                    car.CurrentSpeed += speedIncrement;
                    Console.WriteLine($"{car.Name} increases speed to {car.CurrentSpeed} km/h.");


                    if (car.CurrentSpeed >= 90)
                    {
                        Console.WriteLine($"Fifth gear engaged.");
                    }
                    else if (car.CurrentSpeed >= 65)
                    {
                        Console.WriteLine($"Fourth gear engaged.");
                    }
                    else if (car.CurrentSpeed >= 40)
                    {
                        Console.WriteLine($"Third gear engaged.");
                    }
                    else if (car.CurrentSpeed >= 20)
                    {
                        Console.WriteLine($"Second gear engaged.");
                    }

                    car.AmountOfFuel -= 3;
                }
                else
                {
                    Console.WriteLine($"{car.Name} cannot increase speed due to lack of fuel or technical fluids.");
                }
            }
            else
            {
                Console.WriteLine($"{car.Name} engine is turned off. Please start the engine.");
            }
        }

        public void DecreaseSpeed(Auto car, int speedDecrement)
        {
            if (car.EngineRunning)
            {

                car.CurrentSpeed -= speedDecrement;
                if (car.CurrentSpeed < 0)
                {
                    car.CurrentSpeed = 0;
                }
                Console.WriteLine($"{car.Name} decreases speed to {car.CurrentSpeed} km/h.");
            }
            else
            {
                Console.WriteLine($"{car.Name} engine is turned off. Please start the engine.");
            }
        }
    }

    class Program
    {
        static void Main()
        {
            List<Auto> cars = LoadCarsFromFile("auto_info.txt");

            if (cars.Count > 0)
            {
                Console.WriteLine("Cars loaded from 'auto_info.txt'.");
                Console.WriteLine("Choose a car:");
                foreach (var car in cars)
                {
                    Console.WriteLine($"{car.Name} - {car.Type}");
                }

                Console.Write("Enter the car name to select: ");
                string selectedCarName = Console.ReadLine();

                Auto selectedCar = cars.Find(car => car.Name.Equals(selectedCarName, StringComparison.OrdinalIgnoreCase));

                if (selectedCar != null)
                {
                    Drive carDrive = new Drive();
                    Console.WriteLine($"You selected the car: {selectedCar.Name} - {selectedCar.Type}");

                    Console.WriteLine("1. Start the engine");
                    Console.WriteLine("2. Exit");

                    int engineOption = int.Parse(Console.ReadLine());

                    switch (engineOption)
                    {
                        case 1:
                            TryStartEngine(selectedCar, carDrive, cars);
                            break;

                        case 2:
                            Console.WriteLine("Exiting the program.");
                            break;

                        default:
                            Console.WriteLine("Invalid option. Exiting the program.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Car not found.");
                }
            }
            else
            {
                Console.WriteLine("No cars found in 'auto_info.txt'. Exiting the program.");
            }

            Console.ReadLine();
        }

        static void TryStartEngine(Auto selectedCar, Drive carDrive, List<Auto> cars)
        {
            carDrive.StartEngine(selectedCar);

            if (!selectedCar.EngineRunning)
            {
                Console.WriteLine("Do you want to choose another car?");
                Console.WriteLine("1. Yes");
                Console.WriteLine("2. No");

                int chooseAnotherCarOption = int.Parse(Console.ReadLine());

                switch (chooseAnotherCarOption)
                {
                    case 1:
                        Main(); // Restart the program to choose another car
                        break;

                    case 2:
                        Console.WriteLine("Exiting the program.");
                        break;

                    default:
                        Console.WriteLine("Invalid option. Exiting the program.");
                        break;
                }
            }
            else
            {
                DriveCar(selectedCar, carDrive);
            }
        }
        static List<Auto> LoadCarsFromFile(string filePath)
        {
            List<Auto> cars = new List<Auto>();

            try
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] parts = line.Split(',');

                        if (parts.Length == 4)
                        {
                            string name = parts[0];
                            if (Enum.TryParse(parts[1], out AutoType type) && decimal.TryParse(parts[2], out decimal fuel) && decimal.TryParse(parts[3], out decimal techLiquids))
                            {
                                Auto car = new Auto(name, type, fuel, techLiquids);
                                cars.Add(car);
                            }
                        }
                    }
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Error reading file: {ex.Message}");
            }

            return cars;
        }

        static void DriveCar(Auto selectedCar, Drive carDrive)
        {
            while (selectedCar.EngineRunning)
            {
                Console.WriteLine($"Current speed: {selectedCar.CurrentSpeed} km/h");
                Console.WriteLine("Choose an option:");
                Console.WriteLine("1. Increase speed");
                Console.WriteLine("2. Decrease speed");
                Console.WriteLine("3. Stop the auto");

                int speedOption = int.Parse(Console.ReadLine());

                switch (speedOption)
                {
                    case 1:
                        Console.Write("Enter the speed increment (km/h): ");
                        int speedIncrement = int.Parse(Console.ReadLine());
                        carDrive.IncreaseSpeed(selectedCar, speedIncrement);
                        break;

                    case 2:
                        Console.Write("Enter the speed decrement (km/h): ");
                        int speedDecrement = int.Parse(Console.ReadLine());
                        carDrive.DecreaseSpeed(selectedCar, speedDecrement);

                        
                        if (selectedCar.CurrentSpeed >= 90)
                        {
                            Console.WriteLine($"Fifth gear engaged.");
                        }
                        else if (selectedCar.CurrentSpeed >= 65)
                        {
                            Console.WriteLine($"Fourth gear engaged.");
                        }
                        else if (selectedCar.CurrentSpeed >= 40)
                        {
                            Console.WriteLine($"Third gear engaged.");
                        }
                        else if (selectedCar.CurrentSpeed >= 20)
                        {
                            Console.WriteLine($"Second gear engaged.");
                        }
                        break;

                    case 3:
                        
                        Console.WriteLine($"{selectedCar.Name} is coming to a complete stop.");
                        selectedCar.CurrentSpeed = 0;

                        Console.WriteLine("Choose an option:");
                        Console.WriteLine("1. Increase speed");
                        Console.WriteLine("2. Turn off the engine");

                        int stopOption = int.Parse(Console.ReadLine());

                        switch (stopOption)
                        {
                            case 1:
                                Console.Write("Enter the speed increment (km/h): ");
                                int stopSpeedIncrement = int.Parse(Console.ReadLine());
                                carDrive.IncreaseSpeed(selectedCar, stopSpeedIncrement);
                                break;

                            case 2:
                                carDrive.StopEngine(selectedCar);
                                if (!selectedCar.EngineRunning)
                                {
                                    Console.WriteLine("Do you want to choose another car?");
                                    Console.WriteLine("1. Yes");
                                    Console.WriteLine("2. No");

                                    int chooseAnotherCarOption = int.Parse(Console.ReadLine());

                                    switch (chooseAnotherCarOption)
                                    {
                                        case 1:
                                            Main(); 
                                            break;

                                        case 2:
                                            Console.WriteLine("Exiting the program.");
                                            break;

                                        default:
                                            Console.WriteLine("Invalid option. Exiting the program.");
                                            break;
                                    }
                                }
                                else
                                {
                                    DriveCar(selectedCar, carDrive);
                                }
                                break;

                            default:
                                Console.WriteLine("Invalid option. Turning off the engine.");
                                carDrive.StopEngine(selectedCar);
                                break;
                        }
                        break;

                    default:
                        Console.WriteLine("Invalid option.");
                        break;
                }
            }

            Console.WriteLine($"{selectedCar.Name} engine is turned off.");
        }
    }
}