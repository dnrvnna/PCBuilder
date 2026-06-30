namespace PCBuilder.Models
{
    public class Storage : ComponentBase
    {
        public int Capacity_GB { get; set; }
        public string Type { get; set; } // NVMe, SATA
        public string FormFactor { get; set; } // M.2, 2.5"
        public int ReadSpeed_MBps { get; set; }
        public int WriteSpeed_MBps { get; set; }

        public override bool IsCompatibleWith(ComponentBase other)
        {
            return true; // Накопители совместимы с любыми компонентами
        }

        public override string GetCompatibilityError(ComponentBase other)
        {
            return null;
        }
    }
}