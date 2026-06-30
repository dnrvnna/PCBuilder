using PCBuilder.Models;
using System;
using System.Collections.Generic;

namespace PCBuilder.Services
{
    public interface IDataManager
    {
        List<ComponentBase> Components { get; }
        void LoadData();
        void SaveData();
        List<ComponentBase> GetComponentsByCategory(string category);
        List<string> GetCategories();
        ComponentBase GetComponentById(string id);
        event EventHandler DataLoaded;
        event EventHandler DataSaved;
    }
}