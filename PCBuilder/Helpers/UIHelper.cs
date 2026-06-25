using System.Drawing;

namespace PCBuilder.Helpers
{
    public static class UIHelper
    {
        public static Color GetCategoryColor(string category)
        {
            switch (category)
            {
                case "Процессор": return Color.FromArgb(255, 120, 50);
                case "Материнская плата": return Color.FromArgb(100, 150, 255);
                case "Видеокарта": return Color.FromArgb(50, 200, 100);
                case "Оперативная память": return Color.FromArgb(200, 100, 255);
                case "SSD": return Color.FromArgb(0, 200, 255);
                case "Блок питания": return Color.FromArgb(255, 200, 50);
                case "Охлаждение": return Color.FromArgb(100, 200, 200);
                case "Корпус": return Color.FromArgb(200, 150, 100);
                default: return Color.Gray;
            }
        }

        public static string GetCategoryIcon(string category)
        {
            switch (category)
            {
                case "Процессор": return "🔴";
                case "Материнская плата": return "🔵";
                case "Видеокарта": return "🟣";
                case "Оперативная память": return "🟡";
                case "SSD": return "🟢";
                case "Блок питания": return "🟠";
                case "Охлаждение": return "🔷";
                case "Корпус": return "🟫";
                default: return "⚪";
            }
        }
    }
}