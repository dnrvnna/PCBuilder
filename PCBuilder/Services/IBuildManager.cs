using System;
using System.Collections.Generic;
using PCBuilder.Models;

namespace PCBuilder.Services
{
    public interface IBuildManager
    {
        List<ComponentBase> SelectedComponents { get; }
        decimal TotalPrice { get; }
        bool HasComponent(string category);
        void AddComponent(ComponentBase component);
        void RemoveComponent(ComponentBase component);
        void ClearBuild();
        List<string> CheckCompatibility();

        event EventHandler<ComponentEventArgs> ComponentAdded;
        event EventHandler<ComponentEventArgs> ComponentRemoved;
        event EventHandler BuildCleared;
        event EventHandler CompatibilityChecked;
    }

    public class ComponentEventArgs : EventArgs
    {
        public ComponentBase Component { get; }
        public ComponentEventArgs(ComponentBase component) => Component = component;
    }
}