namespace PCBuilder.Models
{
    public class PowerSupply : ComponentBase
    {
        public int Wattage { get; set; }
        public string Efficiency { get; set; } // 80+ Bronze, Gold, Platinum
        public bool IsModular { get; set; }
        public string ATX_Type { get; set; } // ATX 3.0, ATX 2.0

        public override bool IsCompatibleWith(ComponentBase other)
        {
            // Проверка совместимости с видеокартами и процессорами
            if (other is GraphicsCard gpu)
                return Wattage >= gpu.PowerConsumption_W + 100;
            if (other is Processor cpu)
                return Wattage >= cpu.TDP + 50;
            return true;
        }

        public override string GetCompatibilityError(ComponentBase other)
        {
            if (other is GraphicsCard gpu && Wattage < gpu.PowerConsumption_W + 100)
                return $"БП ({Wattage}W) недостаточен для видеокарты ({gpu.PowerConsumption_W + 100}W)";
            return null;
        }
    }
}