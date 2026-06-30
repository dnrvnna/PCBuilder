namespace PCBuilder.Models
{
    public class MemoryModule : ComponentBase
    {
        public int Capacity_GB { get; set; }
        public string MemoryType { get; set; } // DDR4, DDR5
        public int Frequency_MHz { get; set; }
        public string Timings { get; set; } // CL30, CL36
        public int KitSize { get; set; } // 2x, 1x, 4x

        public override bool IsCompatibleWith(ComponentBase other)
        {
            if (other is Motherboard mb)
                return mb.MemoryType == this.MemoryType;
            return true;
        }

        public override string GetCompatibilityError(ComponentBase other)
        {
            if (other is Motherboard mb && mb.MemoryType != this.MemoryType)
                return $"Память ({MemoryType}) не совместима с материнской платой ({mb.MemoryType})";
            return null;
        }
    }
}