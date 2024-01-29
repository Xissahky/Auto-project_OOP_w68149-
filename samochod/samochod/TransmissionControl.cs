using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace samochod
{
    public class TransmissionControl
    {
        public void SwitchTransmission(Auto car, TransmissionType newTransmission)
        {
            if (car.EngineRunning)
            {
                car.Transmission = newTransmission;
                Console.WriteLine($"{car.Name} transmission switched to {newTransmission}.");
            }
            else
            {
                Console.WriteLine($"{car.Name} engine is turned off. Please start the engine.");
            }
        }
    }
}
