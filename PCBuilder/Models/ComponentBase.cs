using System;
using System.Collections.Generic;

namespace PCBuilder.Models
{
    public abstract class ComponentBase
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public bool InStock { get; set; } = true;
        public Dictionary<string, string> Specs { get; set; } = new Dictionary<string, string>();

        public abstract bool IsCompatibleWith(ComponentBase other);
        public abstract string GetCompatibilityError(ComponentBase other);
        public virtual string GetDisplayName() => Name;

        public override string ToString() => $"{Name} - {Price:N0} ₽";
    }
}