using System;
using System.Collections.Generic;
using System.Linq;
using PCBuilder.Models;
using PCBuilder.Rules;

namespace PCBuilder.Services
{
    public class BuildManager : IBuildManager
    {
        private readonly List<ComponentBase> selectedComponents = new List<ComponentBase>();
        private readonly List<ICompatibilityRule> rules = new List<ICompatibilityRule>();

        public List<ComponentBase> SelectedComponents => selectedComponents;
        public decimal TotalPrice => selectedComponents.Sum(c => c.Price);

        public event EventHandler<ComponentEventArgs> ComponentAdded;
        public event EventHandler<ComponentEventArgs> ComponentRemoved;
        public event EventHandler BuildCleared;
        public event EventHandler CompatibilityChecked;

        public BuildManager()
        {
            // Регистрируем правила совместимости
            rules.Add(new SocketRule());
            rules.Add(new MemoryTypeRule());
            rules.Add(new PowerSupplyRule());
        }

        public bool HasComponent(string category)
        {
            return selectedComponents.Any(c => c.Category == category);
        }

        public void AddComponent(ComponentBase component)
        {
            // Если уже есть компонент этой категории — удаляем старый
            var existing = selectedComponents.FirstOrDefault(c => c.Category == component.Category);
            if (existing != null)
                RemoveComponent(existing);

            selectedComponents.Add(component);
            ComponentAdded?.Invoke(this, new ComponentEventArgs(component));
        }

        public void RemoveComponent(ComponentBase component)
        {
            if (selectedComponents.Remove(component))
                ComponentRemoved?.Invoke(this, new ComponentEventArgs(component));
        }

        public void ClearBuild()
        {
            selectedComponents.Clear();
            BuildCleared?.Invoke(this, EventArgs.Empty);
        }

        public List<string> CheckCompatibility()
        {
            var errors = new List<string>();

            foreach (var rule in rules)
            {
                var result = rule.Check(selectedComponents);
                if (!result.IsCompatible)
                    errors.Add(result.ErrorMessage);
            }

            // Дополнительная проверка: есть ли все необходимые компоненты
            var categories = selectedComponents.Select(c => c.Category).Distinct();
            var required = new[] { "Процессор", "Материнская плата", "Оперативная память", "Видеокарта", "Блок питания" };

            foreach (var req in required)
            {
                if (!categories.Contains(req))
                    errors.Add($"⚠️ Нет {req.ToLower()}!");
            }

            CompatibilityChecked?.Invoke(this, EventArgs.Empty);
            return errors;
        }
    }
}