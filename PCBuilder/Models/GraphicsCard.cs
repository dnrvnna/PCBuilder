namespace PCBuilder.Models
{
    public class GraphicsCard : ComponentBase
    {
        public int VRAM_GB { get; set; }
        public string VRAM_Type { get; set; } // GDDR6, GDDR6X
        public int PowerConsumption_W { get; set; }
        public int CudaCores { get; set; }

        public override bool IsCompatibleWith(ComponentBase other)
        {
            if (other is PowerSupply psu)
                return psu.Wattage >= PowerConsumption_W + 100; // 100W запас
            return true;
        }

        public override string GetCompatibilityError(ComponentBase other)
        {
            if (other is PowerSupply psu && psu.Wattage < PowerConsumption_W + 100)
                return $"Видеокарта требует {PowerConsumption_W + 100}W, БП только {psu.Wattage}W";
            return null;
        }
    }
}