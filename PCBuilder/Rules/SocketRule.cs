using System.Collections.Generic;
using System.Linq;
using PCBuilder.Models;

namespace PCBuilder.Rules
{
    public class SocketRule : ICompatibilityRule
    {
        public CompatibilityResult Check(List<ComponentBase> components)
        {
            var cpu = components.FirstOrDefault(c => c is Processor) as Processor;
            var mb = components.FirstOrDefault(c => c is Motherboard) as Motherboard;

            if (cpu == null || mb == null)
                return CompatibilityResult.Success();

            if (cpu.Socket != mb.Socket)
                return CompatibilityResult.Failure($"❌ Несовместимость Socket: Процессор ({cpu.Socket}) не подходит для материнской платы ({mb.Socket})");

            return CompatibilityResult.Success();
        }
    }
}