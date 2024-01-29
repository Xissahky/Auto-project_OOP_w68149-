using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace samochod
{
    public class LightControl
    {
        public void ToggleLights(Auto car)
        {
            if (car.EngineRunning)
            {
                car.LightsOn = !car.LightsOn;
                Console.WriteLine($"{car.Name} lights are {(car.LightsOn ? "on" : "off")}");
            }
            else
            {
                Console.WriteLine($"{car.Name} engine is turned off. Please start the engine.");
            }
        }
    }
}
