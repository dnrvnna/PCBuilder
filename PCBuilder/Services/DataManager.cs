using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Newtonsoft.Json;
using PCBuilder.Models;

namespace PCBuilder.Services
{
    public class DataManager
    {
        private static readonly string DataFolder = Path.Combine(Application.StartupPath, "data");

        public List<PCComponent> Components { get; private set; } = new List<PCComponent>();

        public DataManager()
        {
            LoadAllData();
        }

        public void LoadAllData()
        {
            Components.Clear();

            try
            {
                if (!Directory.Exists(DataFolder))
                {
                    Directory.CreateDirectory(DataFolder);
                    CreateDefaultData();
                    return;
                }

                // Загружаем все JSON-файлы из папки data
                string[] jsonFiles = Directory.GetFiles(DataFolder, "*.json");

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
                        {
                            Components.AddRange(wrapper.Components);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка загрузки файла {Path.GetFileName(filePath)}: {ex.Message}",
                            "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }

                if (Components.Count == 0)
                {
                    CreateDefaultData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                CreateDefaultData();
            }
        }

        public void SaveAllData()
        {
            try
            {
                if (!Directory.Exists(DataFolder))
                    Directory.CreateDirectory(DataFolder);

                // Сохраняем все компоненты в один файл
                string filePath = Path.Combine(DataFolder, "components.json");
                var wrapper = new ComponentWrapper { Components = Components };
                string json = JsonConvert.SerializeObject(wrapper, Formatting.Indented);
                File.WriteAllText(filePath, json);
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

        private void CreateDefaultData()
        {
            Components = CreateDefaultComponents();
            SaveAllData();
        }

        private List<PCComponent> CreateDefaultComponents()
        {
            return new List<PCComponent>
            {
                // Процессоры (8 шт)
                new PCComponent { Id = "cpu-001", Name = "Intel Core i9-13900K", Category = "Процессор", Price = 55000, Description = "24 ядра, 5.8 ГГц, LGA1700", Socket = "LGA1700", PowerConsumption = 253, InStock = true },
                new PCComponent { Id = "cpu-002", Name = "Intel Core i7-13700K", Category = "Процессор", Price = 38000, Description = "16 ядер, 5.4 ГГц, LGA1700", Socket = "LGA1700", PowerConsumption = 253, InStock = true },
                new PCComponent { Id = "cpu-003", Name = "Intel Core i5-13600K", Category = "Процессор", Price = 25000, Description = "14 ядер, 5.1 ГГц, LGA1700", Socket = "LGA1700", PowerConsumption = 181, InStock = true },
                new PCComponent { Id = "cpu-004", Name = "Intel Core i9-14900K", Category = "Процессор", Price = 62000, Description = "24 ядра, 6.0 ГГц, LGA1700", Socket = "LGA1700", PowerConsumption = 253, InStock = true },
                new PCComponent { Id = "cpu-005", Name = "AMD Ryzen 9 7950X", Category = "Процессор", Price = 52000, Description = "16 ядер, 5.7 ГГц, AM5", Socket = "AM5", PowerConsumption = 170, InStock = true },
                new PCComponent { Id = "cpu-006", Name = "AMD Ryzen 7 7800X3D", Category = "Процессор", Price = 40000, Description = "8 ядер, 5.0 ГГц, AM5", Socket = "AM5", PowerConsumption = 120, InStock = true },
                new PCComponent { Id = "cpu-007", Name = "AMD Ryzen 5 7600X", Category = "Процессор", Price = 22000, Description = "6 ядер, 5.3 ГГц, AM5", Socket = "AM5", PowerConsumption = 105, InStock = true },
                new PCComponent { Id = "cpu-008", Name = "AMD Ryzen 7 5800X3D", Category = "Процессор", Price = 30000, Description = "8 ядер, 4.5 ГГц, AM4", Socket = "AM4", PowerConsumption = 105, InStock = true },
                
                // Материнские платы (8 шт)
                new PCComponent { Id = "mb-001", Name = "ASUS ROG Maximus Z790 Hero", Category = "Материнская плата", Price = 45000, Description = "LGA1700, DDR5, ATX", Socket = "LGA1700", MemoryType = "DDR5", InStock = true },
                new PCComponent { Id = "mb-002", Name = "MSI MPG Z790 Carbon WiFi", Category = "Материнская плата", Price = 32000, Description = "LGA1700, DDR5, ATX", Socket = "LGA1700", MemoryType = "DDR5", InStock = true },
                new PCComponent { Id = "mb-003", Name = "ASUS TUF Gaming B760-Plus", Category = "Материнская плата", Price = 18000, Description = "LGA1700, DDR5, ATX", Socket = "LGA1700", MemoryType = "DDR5", InStock = true },
                new PCComponent { Id = "mb-004", Name = "ASUS ROG Crosshair X670E Hero", Category = "Материнская плата", Price = 50000, Description = "AM5, DDR5, ATX", Socket = "AM5", MemoryType = "DDR5", InStock = true },
                new PCComponent { Id = "mb-005", Name = "MSI MAG X670E Tomahawk", Category = "Материнская плата", Price = 28000, Description = "AM5, DDR5, ATX", Socket = "AM5", MemoryType = "DDR5", InStock = true },
                new PCComponent { Id = "mb-006", Name = "ASUS TUF Gaming B650-Plus", Category = "Материнская плата", Price = 15000, Description = "AM5, DDR5, ATX", Socket = "AM5", MemoryType = "DDR5", InStock = true },
                new PCComponent { Id = "mb-007", Name = "MSI B550 Tomahawk", Category = "Материнская плата", Price = 12000, Description = "AM4, DDR4, ATX", Socket = "AM4", MemoryType = "DDR4", InStock = true },
                new PCComponent { Id = "mb-008", Name = "ASUS ROG Strix B550-F", Category = "Материнская плата", Price = 14000, Description = "AM4, DDR4, ATX", Socket = "AM4", MemoryType = "DDR4", InStock = true },
                
                // Видеокарты (7 шт)
                new PCComponent { Id = "gpu-001", Name = "NVIDIA RTX 4090", Category = "Видеокарта", Price = 160000, Description = "24GB GDDR6X, 450W", PowerConsumption = 450, InStock = true },
                new PCComponent { Id = "gpu-002", Name = "NVIDIA RTX 4080 Super", Category = "Видеокарта", Price = 100000, Description = "16GB GDDR6X, 320W", PowerConsumption = 320, InStock = true },
                new PCComponent { Id = "gpu-003", Name = "NVIDIA RTX 4070 Ti Super", Category = "Видеокарта", Price = 72000, Description = "16GB GDDR6X, 285W", PowerConsumption = 285, InStock = true },
                new PCComponent { Id = "gpu-004", Name = "NVIDIA RTX 4070", Category = "Видеокарта", Price = 50000, Description = "12GB GDDR6X, 200W", PowerConsumption = 200, InStock = true },
                new PCComponent { Id = "gpu-005", Name = "AMD RX 7900 XTX", Category = "Видеокарта", Price = 85000, Description = "24GB GDDR6, 355W", PowerConsumption = 355, InStock = true },
                new PCComponent { Id = "gpu-006", Name = "AMD RX 7900 XT", Category = "Видеокарта", Price = 65000, Description = "20GB GDDR6, 315W", PowerConsumption = 315, InStock = true },
                new PCComponent { Id = "gpu-007", Name = "AMD RX 7800 XT", Category = "Видеокарта", Price = 45000, Description = "16GB GDDR6, 263W", PowerConsumption = 263, InStock = true },
                
                // Оперативная память (6 шт)
                new PCComponent { Id = "ram-001", Name = "32GB DDR5 6000MHz", Category = "Оперативная память", Price = 15000, Description = "2x16GB, CL30", MemoryType = "DDR5", InStock = true },
                new PCComponent { Id = "ram-002", Name = "32GB DDR5 5600MHz", Category = "Оперативная память", Price = 12000, Description = "2x16GB, CL36", MemoryType = "DDR5", InStock = true },
                new PCComponent { Id = "ram-003", Name = "16GB DDR5 5600MHz", Category = "Оперативная память", Price = 8000, Description = "2x8GB, CL36", MemoryType = "DDR5", InStock = true },
                new PCComponent { Id = "ram-004", Name = "32GB DDR4 3600MHz", Category = "Оперативная память", Price = 10000, Description = "2x16GB, CL18", MemoryType = "DDR4", InStock = true },
                new PCComponent { Id = "ram-005", Name = "16GB DDR4 3600MHz", Category = "Оперативная память", Price = 6000, Description = "2x8GB, CL18", MemoryType = "DDR4", InStock = true },
                new PCComponent { Id = "ram-006", Name = "64GB DDR5 6000MHz", Category = "Оперативная память", Price = 28000, Description = "2x32GB, CL30", MemoryType = "DDR5", InStock = true },
                
                // Накопители (5 шт)
                new PCComponent { Id = "ssd-001", Name = "SSD 2TB NVMe Gen4", Category = "SSD", Price = 14000, Description = "Чтение 7000MB/s, M.2", InStock = true },
                new PCComponent { Id = "ssd-002", Name = "SSD 1TB NVMe Gen4", Category = "SSD", Price = 8000, Description = "Чтение 7000MB/s, M.2", InStock = true },
                new PCComponent { Id = "ssd-003", Name = "SSD 500GB NVMe Gen4", Category = "SSD", Price = 4500, Description = "Чтение 5000MB/s, M.2", InStock = true },
                new PCComponent { Id = "ssd-004", Name = "SSD 2TB SATA III", Category = "SSD", Price = 12000, Description = "Чтение 560MB/s, 2.5\"", InStock = true },
                new PCComponent { Id = "ssd-005", Name = "SSD 1TB SATA III", Category = "SSD", Price = 7000, Description = "Чтение 560MB/s, 2.5\"", InStock = true },
                
                // Блоки питания (5 шт)
                new PCComponent { Id = "psu-001", Name = "Блок питания 1200W", Category = "Блок питания", Price = 18000, Description = "80+ Platinum, ATX 3.0", RecommendedPSU = 1200, InStock = true },
                new PCComponent { Id = "psu-002", Name = "Блок питания 1000W", Category = "Блок питания", Price = 12000, Description = "80+ Platinum, ATX 3.0", RecommendedPSU = 1000, InStock = true },
                new PCComponent { Id = "psu-003", Name = "Блок питания 850W", Category = "Блок питания", Price = 9000, Description = "80+ Gold, ATX 3.0", RecommendedPSU = 850, InStock = true },
                new PCComponent { Id = "psu-004", Name = "Блок питания 750W", Category = "Блок питания", Price = 7000, Description = "80+ Gold", RecommendedPSU = 750, InStock = true },
                new PCComponent { Id = "psu-005", Name = "Блок питания 650W", Category = "Блок питания", Price = 5500, Description = "80+ Bronze", RecommendedPSU = 650, InStock = true },
                
                // Охлаждение (5 шт)
                new PCComponent { Id = "cool-001", Name = "NZXT Kraken X73", Category = "Охлаждение", Price = 18000, Description = "360мм, RGB, 3x вентилятора", InStock = true },
                new PCComponent { Id = "cool-002", Name = "Corsair iCUE H150i", Category = "Охлаждение", Price = 16000, Description = "360мм, RGB, 3x вентилятора", InStock = true },
                new PCComponent { Id = "cool-003", Name = "Deepcool AK620", Category = "Охлаждение", Price = 7000, Description = "Двухбашенный, 2x120мм", InStock = true },
                new PCComponent { Id = "cool-004", Name = "Noctua NH-D15", Category = "Охлаждение", Price = 8000, Description = "Двухбашенный, 2x140мм", InStock = true },
                new PCComponent { Id = "cool-005", Name = "Cooler Master Hyper 212", Category = "Охлаждение", Price = 3500, Description = "Однобашенный, 120мм", InStock = true },
                
                // Корпуса (5 шт)
                new PCComponent { Id = "case-001", Name = "Corsair 5000D Airflow", Category = "Корпус", Price = 12000, Description = "ATX, Стекло, 3x вентилятора", InStock = true },
                new PCComponent { Id = "case-002", Name = "NZXT H7 Flow", Category = "Корпус", Price = 10000, Description = "ATX, Стекло, 2x вентилятора", InStock = true },
                new PCComponent { Id = "case-003", Name = "Lian Li O11 Dynamic", Category = "Корпус", Price = 14000, Description = "ATX, Стекло, без вентиляторов", InStock = true },
                new PCComponent { Id = "case-004", Name = "Fractal Design Meshify 2", Category = "Корпус", Price = 11000, Description = "ATX, Стекло, 3x вентилятора", InStock = true },
                new PCComponent { Id = "case-005", Name = "Deepcool CH560", Category = "Корпус", Price = 7000, Description = "ATX, Стекло, 4x вентилятора", InStock = true }
            };
        }

        private class ComponentWrapper
        {
            public List<PCComponent> Components { get; set; } = new List<PCComponent>();
        }
    }
}