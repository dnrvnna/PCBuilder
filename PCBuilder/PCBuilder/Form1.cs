using System;
using System.Drawing;
using System.Windows.Forms;
using PCBuilder.Forms;
using PCBuilder.Services;
using Guna.UI2.WinForms;

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

        public void RefreshAllTabs()
        {
            // Очищаем все контролы и пересоздаём вкладки
            this.Controls.Clear();
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

            // === ВКЛАДКА 1: КАТАЛОГ ===
            TabPage catalogTab = new TabPage("📚 Каталог");
            catalogTab.BackColor = Color.FromArgb(30, 30, 35);
            var catalog = new CatalogTab(dataManager);
            catalogTab.Controls.Add(catalog.Create());

            // === ВКЛАДКА 2: СБОРКА ===
            TabPage buildTab = new TabPage("🔧 Сборка");
            buildTab.BackColor = Color.FromArgb(30, 30, 35);
            var build = new BuildTab(dataManager);
            buildTab.Controls.Add(build.Create());

            // === ВКЛАДКА 3: АНАЛИТИКА ===
            TabPage analyticsTab = new TabPage("📊 Аналитика");
            analyticsTab.BackColor = Color.FromArgb(30, 30, 35);
            var analytics = new AnalyticsTab(dataManager);
            analyticsTab.Controls.Add(analytics.Create());

            tabControl.TabPages.Add(catalogTab);
            tabControl.TabPages.Add(buildTab);
            tabControl.TabPages.Add(analyticsTab);

            // === КНОПКА ОБНОВЛЕНИЯ ===
            Guna2Button btnRefresh = new Guna2Button
            {
                Text = "🔄 Обновить данные",
                Size = new Size(140, 38),
                Location = new Point(this.ClientSize.Width - 160, 10),
                BorderRadius = 10,
                FillColor = Color.FromArgb(0, 150, 255),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.White,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            btnRefresh.HoverState.FillColor = Color.FromArgb(0, 170, 255);
            btnRefresh.Click += (s, e) =>
            {
                dataManager.LoadAllData();
                RefreshAllTabs();

                // Показываем уведомление
                Guna2MessageDialog dialog = new Guna2MessageDialog
                {
                    Text = $"✅ Данные обновлены!\nЗагружено: {dataManager.Components.Count} компонентов",
                    Icon = Guna.UI2.WinForms.MessageDialogIcon.Information,
                    Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK,
                    Style = Guna.UI2.WinForms.MessageDialogStyle.Dark
                };
                dialog.Show();
            };

            this.Controls.Add(tabControl);
            this.Controls.Add(btnRefresh);
        }
    }
}