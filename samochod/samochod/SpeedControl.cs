using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace samochod
{
    public class SpeedControl
    {
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
}
