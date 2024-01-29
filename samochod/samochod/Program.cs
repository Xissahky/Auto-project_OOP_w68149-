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

            Console.ReadLine();
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
                DriveCar(selectedCar, cars);
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

                        if (parts.Length == 5) // Updated for TransmissionType
                        {
                            string name = parts[0];
                            if (Enum.TryParse(parts[1], out AutoType type) && Enum.TryParse(parts[2], out TransmissionType transmission) &&
                                decimal.TryParse(parts[3], out decimal fuel) && decimal.TryParse(parts[4], out decimal techLiquids))
                            {
                                Auto car = new Auto(name, type, transmission, fuel, techLiquids);
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