using System.Collections.Generic;
using System.Linq;
using PCBuilder.Models;

namespace PCBuilder.Rules
{
    public class MemoryTypeRule : ICompatibilityRule
    {
        public CompatibilityResult Check(List<ComponentBase> components)
        {
            var mb = components.FirstOrDefault(c => c is Motherboard) as Motherboard;
            var ram = components.FirstOrDefault(c => c is MemoryModule) as MemoryModule;

            if (mb == null || ram == null)
                return CompatibilityResult.Success();

            if (mb.MemoryType != ram.MemoryType)
                return CompatibilityResult.Failure($"❌ Несовместимость памяти: ОЗУ ({ram.MemoryType}) не подходит для материнской платы ({mb.MemoryType})");

            return CompatibilityResult.Success();
        }
    }
}