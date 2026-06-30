using System.Collections.Generic;
using PCBuilder.Models;

namespace PCBuilder.Rules
{
    public interface ICompatibilityRule
    {
        CompatibilityResult Check(List<ComponentBase> components);
    }

    public class CompatibilityResult
    {
        public bool IsCompatible { get; set; }
        public string ErrorMessage { get; set; }

        public static CompatibilityResult Success() =>
            new CompatibilityResult { IsCompatible = true };

        public static CompatibilityResult Failure(string message) =>
            new CompatibilityResult { IsCompatible = false, ErrorMessage = message };
    }
}