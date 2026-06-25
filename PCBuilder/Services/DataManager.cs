using Newtonsoft.Json;
using PCBuilder.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace PCBuilder.Services
{
    public class DataManager
    {   
        private static readonly string DataPath = Path.Combine(Application.StartupPath, "data", "components.json");

        public List<PCComponent> Components { get; private set; } = new List<PCComponent>();

        public DataManager()
        {
            LoadData();
        }

        public void LoadData()
        {
            try
            {
                if (!File.Exists(DataPath))
                {
                    Components = CreateDefaultComponents();
                    SaveData();
                    return;
                }

                string json = File.ReadAllText(DataPath);
                var wrapper = JsonConvert.DeserializeObject<ComponentWrapper>(json);
                Components = wrapper?.Components ?? new List<PCComponent>();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Components = CreateDefaultComponents();
            }
        }

        public void SaveData()
        {
            try
            {
                string directory = Path.GetDirectoryName(DataPath);
                if (!Directory.Exists(directory))
                    Directory.CreateDirectory(directory);

                var wrapper = new ComponentWrapper { Components = Components };
                string json = JsonConvert.SerializeObject(wrapper, Formatting.Indented);
                File.WriteAllText(DataPath, json);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения данных: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public List<PCComponent> GetComponentsByCategory(string category)
        {
            return Components.Where(c => c.Category == category).ToList();
        }

        public List<string> GetCategories()
        {
            return Components.Select(c => c.Category).Distinct().ToList();
        }

        public PCComponent GetComponentById(string id)
        {
            return Components.FirstOrDefault(c => c.Id == id);
        }

        private List<PCComponent> CreateDefaultComponents()
        {
            return new List<PCComponent>
            {
                new PCComponent { Id = "cpu-001", Name = "Intel Core i9-13900K", Category = "Процессор", Price = 55000, Description = "24 ядра, 5.8 ГГц, LGA1700", Socket = "LGA1700", PowerConsumption = 253, InStock = true },
                new PCComponent { Id = "cpu-002", Name = "AMD Ryzen 9 7950X", Category = "Процессор", Price = 52000, Description = "16 ядер, 5.7 ГГц, AM5", Socket = "AM5", PowerConsumption = 170, InStock = true },
                new PCComponent { Id = "gpu-001", Name = "NVIDIA RTX 4090", Category = "Видеокарта", Price = 160000, Description = "24GB GDDR6X, 450W", PowerConsumption = 450, InStock = true },
                new PCComponent { Id = "mb-001", Name = "ASUS ROG Maximus Z790 Hero", Category = "Материнская плата", Price = 45000, Description = "LGA1700, DDR5, ATX", Socket = "LGA1700", MemoryType = "DDR5", InStock = true },
                new PCComponent { Id = "ram-001", Name = "32GB DDR5 6000MHz", Category = "Оперативная память", Price = 15000, Description = "2x16GB, CL30", MemoryType = "DDR5", InStock = true },
                new PCComponent { Id = "ssd-001", Name = "SSD 2TB NVMe Gen4", Category = "SSD", Price = 14000, Description = "Чтение 7000MB/s, M.2", InStock = true },
                new PCComponent { Id = "psu-001", Name = "Блок питания 1000W", Category = "Блок питания", Price = 12000, Description = "80+ Platinum, ATX 3.0", RecommendedPSU = 1000, InStock = true }
            };
        }

        private class ComponentWrapper
        {
            public List<PCComponent> Components { get; set; } = new List<PCComponent>();
        }
    }
}