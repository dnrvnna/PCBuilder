using PCBuilder.Models;

namespace PCBuilder.Factories
{
    public static class ComponentFactory
    {
        public static Processor CreateProcessor(string name, decimal price, string socket, int cores, int threads, double maxFreq, int tdp)
        {
            return new Processor
            {
                Name = name,
                Category = "Процессор",
                Price = price,
                Socket = socket,
                CoreCount = cores,
                ThreadCount = threads,
                MaxFrequency = maxFreq,
                TDP = tdp,
                Description = $"{cores} ядер, {maxFreq} ГГц, {socket}"
            };
        }

        public static Motherboard CreateMotherboard(string name, decimal price, string socket, string memoryType, int slots, int maxMemory)
        {
            return new Motherboard
            {
                Name = name,
                Category = "Материнская плата",
                Price = price,
                Socket = socket,
                MemoryType = memoryType,
                MemorySlots = slots,
                MaxMemoryGB = maxMemory,
                Description = $"{socket}, {memoryType}, ATX"
            };
        }

        public static GraphicsCard CreateGraphicsCard(string name, decimal price, int vramGB, string vramType, int powerConsumption)
        {
            return new GraphicsCard
            {
                Name = name,
                Category = "Видеокарта",
                Price = price,
                VRAM_GB = vramGB,
                VRAM_Type = vramType,
                PowerConsumption_W = powerConsumption,
                Description = $"{vramGB}GB {vramType}, {powerConsumption}W"
            };
        }

        public static MemoryModule CreateMemoryModule(string name, decimal price, int capacity, string memoryType, int frequency, string timings)
        {
            return new MemoryModule
            {
                Name = name,
                Category = "Оперативная память",
                Price = price,
                Capacity_GB = capacity,
                MemoryType = memoryType,
                Frequency_MHz = frequency,
                Timings = timings,
                Description = $"{capacity}GB, {frequency}MHz, {memoryType}"
            };
        }

        public static Storage CreateStorage(string name, decimal price, int capacity, string type, int readSpeed, int writeSpeed)
        {
            return new Storage
            {
                Name = name,
                Category = "SSD",
                Price = price,
                Capacity_GB = capacity,
                Type = type,
                ReadSpeed_MBps = readSpeed,
                WriteSpeed_MBps = writeSpeed,
                Description = $"Чтение {readSpeed}MB/s, {type}"
            };
        }

        public static PowerSupply CreatePowerSupply(string name, decimal price, int wattage, string efficiency, bool isModular)
        {
            return new PowerSupply
            {
                Name = name,
                Category = "Блок питания",
                Price = price,
                Wattage = wattage,
                Efficiency = efficiency,
                IsModular = isModular,
                Description = $"{wattage}W, {efficiency}"
            };
        }
    }
}