using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace samochod
{
    public class Engine
    {
        public void Start(Auto car)
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

        public void Stop(Auto car)
        {
            if (car.EngineRunning)
            {
                car.EngineRunning = false;
                car.LightsOn = false; // Turn off lights when the engine stops
                Console.WriteLine($"{car.Name} engine stopped.");
            }
            else
            {
                Console.WriteLine($"{car.Name} engine is already turned off.");
            }
        }
    }


}
