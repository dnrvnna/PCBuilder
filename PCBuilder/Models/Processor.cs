namespace PCBuilder.Models
{
    public class Processor : ComponentBase
    {
        public string Socket { get; set; }
        public int CoreCount { get; set; }
        public int ThreadCount { get; set; }
        public double MaxFrequency { get; set; }
        public int TDP { get; set; }

        public override bool IsCompatibleWith(ComponentBase other)
        {
            if (other is Motherboard mb)
                return mb.Socket == this.Socket;
            return true;
        }

        public override string GetCompatibilityError(ComponentBase other)
        {
            if (other is Motherboard mb && mb.Socket != this.Socket)
                return $"Процессор ({Socket}) не совместим с материнской платой ({mb.Socket})";
            return null;
        }
    }
}