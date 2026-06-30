namespace PCBuilder.Models
{
    public class Motherboard : ComponentBase
    {
        public string Socket { get; set; }
        public string MemoryType { get; set; } // DDR4, DDR5
        public int MemorySlots { get; set; }
        public int MaxMemoryGB { get; set; }
        public string FormFactor { get; set; } // ATX, Micro-ATX, Mini-ITX

        public override bool IsCompatibleWith(ComponentBase other)
        {
            if (other is Processor cpu)
                return cpu.Socket == this.Socket;
            if (other is MemoryModule ram)
                return ram.MemoryType == this.MemoryType;
            return true;
        }

        public override string GetCompatibilityError(ComponentBase other)
        {
            if (other is Processor cpu && cpu.Socket != this.Socket)
                return $"Материнская плата ({Socket}) не совместима с процессором ({cpu.Socket})";
            if (other is MemoryModule ram && ram.MemoryType != this.MemoryType)
                return $"Материнская плата ({MemoryType}) не совместима с памятью ({ram.MemoryType})";
            return null;
        }
    }
}