using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Newtonsoft.Json;
using PCBuilder.Factories;
using PCBuilder.Models;

namespace PCBuilder.Services
{
    public class DataManager : IDataManager
    {
        private static readonly string DataFolder = Path.Combine(Application.StartupPath, "data");
        private List<ComponentBase> components = new List<ComponentBase>();

        public List<ComponentBase> Components => components;

        public event EventHandler DataLoaded;
        public event EventHandler DataSaved;

        public DataManager()
        {
            LoadData();
        }

        public void LoadData()
        {
            components.Clear();

            try
            {
                if (!Directory.Exists(DataFolder))
                {
                    Directory.CreateDirectory(DataFolder);
                    CreateDefaultData();
                    return;
                }

                var jsonFiles = Directory.GetFiles(DataFolder, "*.json");
                if (jsonFiles.Length == 0)
                {
                    CreateDefaultData();
                    return;
                }

                foreach (string filePath in jsonFiles)
                {
                    try
                    {
                        string json = File.ReadAllText(filePath);
                        var wrapper = JsonConvert.DeserializeObject<ComponentWrapper>(json);
                        if (wrapper?.Components != null)
                            components.AddRange(wrapper.Components);
                    }
                    catch
                    {
                        // Пропускаем проблемный файл
                    }
                }

                if (components.Count == 0)
                    CreateDefaultData();

                DataLoaded?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                CreateDefaultData();
            }
        }

        public void SaveData()
        {
            try
            {
                if (!Directory.Exists(DataFolder))
                    Directory.CreateDirectory(DataFolder);

                var filePath = Path.Combine(DataFolder, "components.json");
                var wrapper = new ComponentWrapper { Components = components };
                string json = JsonConvert.SerializeObject(wrapper, Formatting.Indented);
                File.WriteAllText(filePath, json);

                DataSaved?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения данных: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public List<ComponentBase> GetComponentsByCategory(string category)
        {
            return components.Where(c => c.Category == category).ToList();
        }

        public List<string> GetCategories()
        {
            return components.Select(c => c.Category).Distinct().ToList();
        }

        public ComponentBase GetComponentById(string id)
        {
            return components.FirstOrDefault(c => c.Id == id);
        }

        private void CreateDefaultData()
        {
            // Используем фабрику для создания компонентов
            components = new List<ComponentBase>
            {
                // Процессоры
                ComponentFactory.CreateProcessor("Intel Core i9-13900K", 55000, "LGA1700", 24, 32, 5.8, 253),
                ComponentFactory.CreateProcessor("Intel Core i7-13700K", 38000, "LGA1700", 16, 24, 5.4, 253),
                ComponentFactory.CreateProcessor("Intel Core i5-13600K", 25000, "LGA1700", 14, 20, 5.1, 181),
                ComponentFactory.CreateProcessor("AMD Ryzen 9 7950X", 52000, "AM5", 16, 32, 5.7, 170),
                ComponentFactory.CreateProcessor("AMD Ryzen 7 7800X3D", 40000, "AM5", 8, 16, 5.0, 120),
                ComponentFactory.CreateProcessor("AMD Ryzen 5 7600X", 22000, "AM5", 6, 12, 5.3, 105),
                ComponentFactory.CreateProcessor("AMD Ryzen 7 5800X3D", 30000, "AM4", 8, 16, 4.5, 105),
                ComponentFactory.CreateProcessor("Intel Core i9-14900K", 62000, "LGA1700", 24, 32, 6.0, 253),
                
                // Материнские платы
                ComponentFactory.CreateMotherboard("ASUS ROG Maximus Z790 Hero", 45000, "LGA1700", "DDR5", 4, 128),
                ComponentFactory.CreateMotherboard("MSI MPG Z790 Carbon WiFi", 32000, "LGA1700", "DDR5", 4, 128),
                ComponentFactory.CreateMotherboard("ASUS TUF Gaming B760-Plus", 18000, "LGA1700", "DDR5", 4, 128),
                ComponentFactory.CreateMotherboard("ASUS ROG Crosshair X670E Hero", 50000, "AM5", "DDR5", 4, 128),
                ComponentFactory.CreateMotherboard("MSI MAG X670E Tomahawk", 28000, "AM5", "DDR5", 4, 128),
                ComponentFactory.CreateMotherboard("ASUS TUF Gaming B650-Plus", 15000, "AM5", "DDR5", 4, 128),
                ComponentFactory.CreateMotherboard("MSI B550 Tomahawk", 12000, "AM4", "DDR4", 4, 128),
                ComponentFactory.CreateMotherboard("ASUS ROG Strix B550-F", 14000, "AM4", "DDR4", 4, 128),
                
                // Видеокарты
                ComponentFactory.CreateGraphicsCard("NVIDIA RTX 4090", 160000, 24, "GDDR6X", 450),
                ComponentFactory.CreateGraphicsCard("NVIDIA RTX 4080 Super", 100000, 16, "GDDR6X", 320),
                ComponentFactory.CreateGraphicsCard("NVIDIA RTX 4070 Ti Super", 72000, 16, "GDDR6X", 285),
                ComponentFactory.CreateGraphicsCard("NVIDIA RTX 4070", 50000, 12, "GDDR6X", 200),
                ComponentFactory.CreateGraphicsCard("AMD RX 7900 XTX", 85000, 24, "GDDR6", 355),
                ComponentFactory.CreateGraphicsCard("AMD RX 7900 XT", 65000, 20, "GDDR6", 315),
                ComponentFactory.CreateGraphicsCard("AMD RX 7800 XT", 45000, 16, "GDDR6", 263),
                
                // Оперативная память
                ComponentFactory.CreateMemoryModule("32GB DDR5 6000MHz", 15000, 32, "DDR5", 6000, "CL30"),
                ComponentFactory.CreateMemoryModule("32GB DDR5 5600MHz", 12000, 32, "DDR5", 5600, "CL36"),
                ComponentFactory.CreateMemoryModule("16GB DDR5 5600MHz", 8000, 16, "DDR5", 5600, "CL36"),
                ComponentFactory.CreateMemoryModule("32GB DDR4 3600MHz", 10000, 32, "DDR4", 3600, "CL18"),
                ComponentFactory.CreateMemoryModule("16GB DDR4 3600MHz", 6000, 16, "DDR4", 3600, "CL18"),
                ComponentFactory.CreateMemoryModule("64GB DDR5 6000MHz", 28000, 64, "DDR5", 6000, "CL30"),
                
                // Накопители
                ComponentFactory.CreateStorage("SSD 2TB NVMe Gen4", 14000, 2000, "NVMe", 7000, 5000),
                ComponentFactory.CreateStorage("SSD 1TB NVMe Gen4", 8000, 1000, "NVMe", 7000, 5000),
                ComponentFactory.CreateStorage("SSD 500GB NVMe Gen4", 4500, 500, "NVMe", 5000, 3000),
                ComponentFactory.CreateStorage("SSD 2TB SATA III", 12000, 2000, "SATA", 560, 530),
                ComponentFactory.CreateStorage("SSD 1TB SATA III", 7000, 1000, "SATA", 560, 530),
                
                // Блоки питания
                ComponentFactory.CreatePowerSupply("Блок питания 1200W", 18000, 1200, "80+ Platinum", true),
                ComponentFactory.CreatePowerSupply("Блок питания 1000W", 12000, 1000, "80+ Platinum", true),
                ComponentFactory.CreatePowerSupply("Блок питания 850W", 9000, 850, "80+ Gold", true),
                ComponentFactory.CreatePowerSupply("Блок питания 750W", 7000, 750, "80+ Gold", false),
                ComponentFactory.CreatePowerSupply("Блок питания 650W", 5500, 650, "80+ Bronze", false)
            };

            SaveData();
        }

        private class ComponentWrapper
        {
            public List<ComponentBase> Components { get; set; } = new List<ComponentBase>();
        }
    }
}