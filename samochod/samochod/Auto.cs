using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace samochod
{
    public class Auto
    {
        public string Name { get; set; }
        public AutoType Type { get; set; }
        public TransmissionType Transmission { get; set; } 
        public decimal AmountOfFuel { get; set; }
        public decimal AmountOfTechLiquids { get; set; }
        public int CurrentSpeed { get; set; }
        public bool EngineRunning { get; set; }
        public bool LightsOn { get; set; }
        public EngineGeneration EngineGeneration { get; set; }

        public Auto(string name, AutoType type, TransmissionType transmission, decimal fuel, decimal techLiquids, EngineGeneration engineGeneration)
        {
            Name = name;
            Type = type;
            Transmission = transmission;
            AmountOfFuel = fuel;
            AmountOfTechLiquids = techLiquids;
            CurrentSpeed = 0;
            EngineRunning = false;
            LightsOn = false;
            EngineGeneration = engineGeneration;
        }
    }
}
