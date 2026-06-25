using System;
using System.Drawing;
using System.Windows.Forms;
using PCBuilder.Forms;
using PCBuilder.Services;

namespace PCBuilder
{
    public partial class Form1 : Form
    {
        private DataManager dataManager;

        public Form1()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            this.BackColor = Color.FromArgb(30, 30, 35);
            this.Text = "🖥️ PCBuilder - Сборка ПК";
            this.MinimumSize = new Size(1024, 600);

            dataManager = new DataManager();
            InitializeTabs();
        }

        private void InitializeTabs()
        {
            TabControl tabControl = new TabControl
            {
                Dock = DockStyle.Fill,
                ItemSize = new Size(160, 45),
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Padding = new Point(15, 8),
                BackColor = Color.FromArgb(30, 30, 35),
                SizeMode = TabSizeMode.Fixed
            };

            // Стилизация вкладок
            tabControl.DrawMode = TabDrawMode.OwnerDrawFixed;
            tabControl.DrawItem += (sender, e) =>
            {
                TabPage page = tabControl.TabPages[e.Index];
                bool isSelected = tabControl.SelectedIndex == e.Index;

                Color bgColor = isSelected ? Color.FromArgb(55, 55, 65) : Color.FromArgb(35, 35, 40);
                using (SolidBrush brush = new SolidBrush(bgColor))
                    e.Graphics.FillRectangle(brush, e.Bounds);

                using (StringFormat sf = new StringFormat())
                {
                    sf.Alignment = StringAlignment.Center;
                    sf.LineAlignment = StringAlignment.Center;
                    e.Graphics.DrawString(page.Text, page.Font, Brushes.White,
                        new Rectangle(e.Bounds.X, e.Bounds.Y + 2, e.Bounds.Width, e.Bounds.Height), sf);
                }

                if (isSelected)
                {
                    using (Pen pen = new Pen(Color.FromArgb(0, 150, 255), 3))
                        e.Graphics.DrawLine(pen, e.Bounds.X + 20, e.Bounds.Bottom - 3,
                            e.Bounds.Right - 20, e.Bounds.Bottom - 3);
                }
            };

            // Создаём вкладки через отдельные классы
            TabPage catalogTab = new TabPage("📚 Каталог");
            catalogTab.BackColor = Color.FromArgb(30, 30, 35);
            catalogTab.Controls.Add(new CatalogTab(dataManager).Create());

            TabPage buildTab = new TabPage("🔧 Сборка");
            buildTab.BackColor = Color.FromArgb(30, 30, 35);
            buildTab.Controls.Add(new BuildTab(dataManager).Create());

            TabPage analyticsTab = new TabPage("📊 Аналитика");
            analyticsTab.BackColor = Color.FromArgb(30, 30, 35);
            analyticsTab.Controls.Add(new AnalyticsTab(dataManager).Create());

            tabControl.TabPages.Add(catalogTab);
            tabControl.TabPages.Add(buildTab);
            tabControl.TabPages.Add(analyticsTab);

            this.Controls.Add(tabControl);
        }
    }
}