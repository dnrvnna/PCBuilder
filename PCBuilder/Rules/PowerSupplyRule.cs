using System.Collections.Generic;
using System.Linq;
using PCBuilder.Models;

namespace PCBuilder.Rules
{
    public class PowerSupplyRule : ICompatibilityRule
    {
        public CompatibilityResult Check(List<ComponentBase> components)
        {
            var psu = components.FirstOrDefault(c => c is PowerSupply) as PowerSupply;
            var cpu = components.FirstOrDefault(c => c is Processor) as Processor;
            var gpu = components.FirstOrDefault(c => c is GraphicsCard) as GraphicsCard;

            if (psu == null)
                return CompatibilityResult.Failure("⚠️ Нет блока питания!");

            if (cpu == null && gpu == null)
                return CompatibilityResult.Success();

            int requiredPower = 100; // базовое потребление

            if (cpu != null)
                requiredPower += cpu.TDP;

            if (gpu != null)
                requiredPower += gpu.PowerConsumption_W;

            if (psu.Wattage < requiredPower)
                return CompatibilityResult.Failure($"❌ Недостаточная мощность БП: требуется {requiredPower}W, а у вас {psu.Wattage}W");

            return CompatibilityResult.Success();
        }
    }
}