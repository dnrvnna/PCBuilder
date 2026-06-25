using System;
using System.Collections.Generic;

namespace PCBuilder.Models
{
    public class PCComponent
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string Socket { get; set; }
        public string MemoryType { get; set; }
        public int? PowerConsumption { get; set; }
        public int? RecommendedPSU { get; set; }
        public bool InStock { get; set; } = true;
        public string ImageUrl { get; set; }
        public Dictionary<string, string> Specs { get; set; } = new Dictionary<string, string>();

        public PCComponent() { }

        public PCComponent(string name, string category, decimal price, string description,
                          string socket = null, string memoryType = null, int? powerConsumption = null)
        {
            Name = name;
            Category = category;
            Price = price;
            Description = description;
            Socket = socket;
            MemoryType = memoryType;
            PowerConsumption = powerConsumption;
        }

        public override string ToString()
        {
            return $"{Name} - {Price:N0} ₽";
        }
    }
}