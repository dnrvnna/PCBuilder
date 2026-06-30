using System;
using System.Collections.Generic;
using System.Linq;

namespace PCBuilder.Models
{
    public class Build
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime? ModifiedDate { get; set; }
        public List<BuildComponent> Components { get; set; } = new List<BuildComponent>();

        public decimal TotalPrice => Components.Sum(c => c.Price);
        public int ComponentCount => Components.Count;
        public bool IsComplete => HasRequiredComponents();

        public string Summary
        {
            get
            {
                var componentNames = Components.Select(c => c.Name);
                return string.Join(", ", componentNames.Take(3)) + (Components.Count > 3 ? $" и ещё {Components.Count - 3}" : "");
            }
        }

        public string CategorySummary
        {
            get
            {
                var categories = Components.GroupBy(c => c.Category)
                    .Select(g => $"{g.Key}: {g.Count()}")
                    .ToList();
                return string.Join(" | ", categories);
            }
        }

        private bool HasRequiredComponents()
        {
            var required = new[] { "Процессор", "Материнская плата", "Оперативная память", "Видеокарта", "Блок питания" };
            var categories = Components.Select(c => c.Category).Distinct();
            return required.All(r => categories.Contains(r));
        }
    }

    public class BuildComponent
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string Socket { get; set; }
        public string MemoryType { get; set; }
        public int? PowerConsumption { get; set; }
        public int? RecommendedPSU { get; set; }
        public Dictionary<string, string> Specs { get; set; } = new Dictionary<string, string>();

        public static BuildComponent FromComponent(ComponentBase component)
        {
            return new BuildComponent
            {
                Id = component.Id,
                Name = component.Name,
                Category = component.Category,
                Price = component.Price,
                Description = component.Description,
                Socket = component is Processor p ? p.Socket :
                          component is Motherboard mb ? mb.Socket : null,
                MemoryType = component is Motherboard mb2 ? mb2.MemoryType :
                             component is MemoryModule ram ? ram.MemoryType : null,
                PowerConsumption = component is Processor cpu ? cpu.TDP :
                                   component is GraphicsCard gpu ? gpu.PowerConsumption_W : (int?)null,
                RecommendedPSU = component is PowerSupply psu ? psu.Wattage : (int?)null,
                Specs = component.Specs ?? new Dictionary<string, string>()
            };
        }
    }
}