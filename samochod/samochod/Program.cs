using System;
using System.Collections.Generic;
using System.IO;

namespace samochod
{

    class Program
    {
        private readonly Engine _engine;
        private readonly SpeedControl _speedControl;
        private readonly LightControl _lightControl;
        private readonly TransmissionControl _transmissionControl; // Added TransmissionControl

        public Program()
        {
            _engine = new Engine();
            _speedControl = new SpeedControl();
            _lightControl = new LightControl();
            _transmissionControl = new TransmissionControl(); // Initialized TransmissionControl
        }

        static void Main()
        {
            Program program = new Program();
            List<Auto> cars = LoadCarsFromFile("auto_info.txt");

            Console.WriteLine("Choose an option:");
            Console.WriteLine("1. Select a car from the list");
            Console.WriteLine("2. Add your own car");

            int option = int.Parse(Console.ReadLine());

            switch (option)
            {
                case 1:
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
                            program.TryStartEngine(selectedCar, cars);
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
                    break;

                case 2:
                    Auto userCar = program.AddUserCar();
                    if (userCar != null)
                    {
                        cars.Add(userCar);
                        program.TryStartEngine(userCar, cars);
                    }
                    else
                    {
                        Console.WriteLine("Invalid car information. Exiting the program.");
                    }
                    break;

                default:
                    Console.WriteLine("Invalid option. Exiting the program.");
                    break;
            }

            Console.ReadLine();
        }

        public Auto AddUserCar()
        {
            Console.WriteLine("Enter the details for your car:");
            Console.Write("Name: ");
            string name = Console.ReadLine();

            Console.WriteLine("Choose car type:");
            Console.WriteLine("0. SportCar");
            Console.WriteLine("1. Crossover");
            Console.WriteLine("2. Pickup");
            Console.WriteLine("3. Minivan");

            if (int.TryParse(Console.ReadLine(), out int carTypeInput) && Enum.IsDefined(typeof(AutoType), carTypeInput))
            {
                AutoType type = (AutoType)carTypeInput;

                Console.WriteLine("Choose transmission type:");
                Console.WriteLine("0. Manual");
                Console.WriteLine("1. Automatic");

                if (int.TryParse(Console.ReadLine(), out int transmissionTypeInput) && Enum.IsDefined(typeof(TransmissionType), transmissionTypeInput))
                {
                    TransmissionType transmission = (TransmissionType)transmissionTypeInput;

                    Console.Write("Amount of fuel: ");
                    decimal fuel;
                    if (decimal.TryParse(Console.ReadLine(), out fuel))
                    {
                        Console.Write("Amount of technical liquids: ");
                        decimal techLiquids;
                        if (decimal.TryParse(Console.ReadLine(), out techLiquids))
                        {
                            Console.WriteLine("Choose engine generation:");
                            Console.WriteLine("0. Generation1");
                            Console.WriteLine("1. Generation2");
                            Console.WriteLine("2. Generation3");

                            if (int.TryParse(Console.ReadLine(), out int engineGenerationInput) && Enum.IsDefined(typeof(EngineGeneration), engineGenerationInput))
                            {
                                EngineGeneration engineGeneration = (EngineGeneration)engineGenerationInput;

                                Auto newUserCar = new Auto(name, type, transmission, fuel, techLiquids, engineGeneration);

                                
                                SaveCarToFile(newUserCar, "auto_info.txt");

                                return newUserCar;
                            }
                            else
                            {
                                Console.WriteLine("Invalid engine generation. Exiting the program.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid technical liquids amount. Exiting the program.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid fuel amount. Exiting the program.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid transmission type. Exiting the program.");
                }
            }
            else
            {
                Console.WriteLine("Invalid car type. Exiting the program.");
            }

            return null;
        }

        private static void SaveCarToFile(Auto car, string filePath)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    
                    writer.WriteLine($"{car.Name};{car.Type};{car.Transmission};{car.AmountOfFuel};{car.AmountOfTechLiquids};{car.EngineGeneration}");
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Error writing to file: {ex.Message}");
            }
        }

        void TryStartEngine(Auto selectedCar, List<Auto> cars)
        {
            _engine.Start(selectedCar);

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
                
                SaveCarsToFile(cars, "auto_info.txt");

                DriveCar(selectedCar, cars);
            }
        }

        private static void SaveCarsToFile(List<Auto> cars, string filePath)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(filePath, false))
                {
                    foreach (var car in cars)
                    {
                        writer.WriteLine($"{car.Name};{car.Type};{car.Transmission};{car.AmountOfFuel};{car.AmountOfTechLiquids};{car.EngineGeneration}");
                    }
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Error writing to file: {ex.Message}");
            }
        }

        void DriveCar(Auto selectedCar, List<Auto> cars)
        {
            while (selectedCar.EngineRunning)
            {
                Console.WriteLine($"Current speed: {selectedCar.CurrentSpeed} km/h");
                Console.WriteLine("Choose an option:");
                Console.WriteLine("1. Increase speed");
                Console.WriteLine("2. Decrease speed");
                Console.WriteLine("3. Toggle lights");
                Console.WriteLine("4. Switch transmission");
                Console.WriteLine("5. Stop the auto");

                int speedOption = int.Parse(Console.ReadLine());

                switch (speedOption)
                {
                    case 1:
                        Console.Write("Enter the speed increment (km/h): ");
                        int speedIncrement = int.Parse(Console.ReadLine());
                        _speedControl.IncreaseSpeed(selectedCar, speedIncrement);
                        break;

                    case 2:
                        Console.Write("Enter the speed decrement (km/h): ");
                        int speedDecrement = int.Parse(Console.ReadLine());
                        _speedControl.DecreaseSpeed(selectedCar, speedDecrement);

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
                        _lightControl.ToggleLights(selectedCar);
                        break;

                    case 4:
                        Console.WriteLine("Choose transmission type:");
                        Console.WriteLine("1. Manual");
                        Console.WriteLine("2. Automatic");
                        int transmissionOption = int.Parse(Console.ReadLine());
                        TransmissionType newTransmission = (transmissionOption == 1) ? TransmissionType.Manual : TransmissionType.Automatic;
                        _transmissionControl.SwitchTransmission(selectedCar, newTransmission);
                        break;

                    case 5:
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
                                _speedControl.IncreaseSpeed(selectedCar, stopSpeedIncrement);
                                break;

                            case 2:
                                _engine.Stop(selectedCar);
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
                                    DriveCar(selectedCar, cars);
                                }
                                break;

                            default:
                                Console.WriteLine("Invalid option. Turning off the engine.");
                                _engine.Stop(selectedCar);
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
                        string[] parts = line.Split(';');

                        if (parts.Length == 6) 
                        {
                            string name = parts[0];
                            if (Enum.TryParse(parts[1], out AutoType type) && Enum.TryParse(parts[2], out TransmissionType transmission) &&
                                decimal.TryParse(parts[3], out decimal fuel) && decimal.TryParse(parts[4], out decimal techLiquids) &&
                                Enum.TryParse(parts[5], out EngineGeneration engineGeneration))
                            {
                                Auto car = new Auto(name, type, transmission, fuel, techLiquids, engineGeneration);
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
    }
}