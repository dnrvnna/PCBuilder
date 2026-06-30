using PCBuilder.Forms.Tabs;
using PCBuilder.Models;
using PCBuilder.Services;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace PCBuilder.Forms
{
    public partial class MainForm : Form
    {
        private readonly IDataManager dataManager;
        private readonly IBuildManager buildManager;
        private readonly IBuildStorage buildStorage;
        private TabControl tabControl;

        // ✅ СОХРАНЯЕМ ССЫЛКУ НА ВКЛАДКУ
        private BuildTab buildTab;

        public MainForm()
        {
            InitializeComponent();

            dataManager = new DataManager();
            buildManager = new BuildManager();
            buildStorage = new BuildStorage();

            InitializeUI();
        }

        private void InitializeUI()
        {
            this.WindowState = FormWindowState.Maximized;
            this.BackColor = Color.FromArgb(30, 30, 35);
            this.Text = "🖥️ PCBuilder - Сборка ПК";
            this.MinimumSize = new Size(1024, 600);

            tabControl = new TabControl
            {
                Dock = DockStyle.Fill,
                ItemSize = new Size(160, 45),
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Padding = new Point(15, 8),
                BackColor = Color.FromArgb(30, 30, 35),
                SizeMode = TabSizeMode.Fixed
            };

            tabControl.DrawMode = TabDrawMode.OwnerDrawFixed;
            tabControl.DrawItem += DrawTabItem;

            // ✅ СОЗДАЁМ ВКЛАДКИ И СОХРАНЯЕМ ССЫЛКИ
            AddTab(new CatalogTab(dataManager));

            // Сохраняем BuildTab
            buildTab = new BuildTab(dataManager, buildManager, buildStorage);
            AddTab(buildTab);

            AddTab(new AnalyticsTab(dataManager, buildStorage));
            AddTab(new SavedBuildsTab(dataManager, buildStorage));

            this.Controls.Add(tabControl);
        }

        private void AddTab(BaseTab tab)
        {
            var page = new TabPage(tab.TabName);
            page.BackColor = Color.FromArgb(30, 30, 35);
            page.Controls.Add(tab.Create());
            tabControl.TabPages.Add(page);
        }

        private void DrawTabItem(object sender, DrawItemEventArgs e)
        {
            var control = sender as TabControl;
            var page = control.TabPages[e.Index];
            bool isSelected = control.SelectedIndex == e.Index;

            Color bgColor = isSelected ? Color.FromArgb(55, 55, 65) : Color.FromArgb(35, 35, 40);
            using (var brush = new SolidBrush(bgColor))
                e.Graphics.FillRectangle(brush, e.Bounds);

            using (var sf = new StringFormat())
            {
                sf.Alignment = StringAlignment.Center;
                sf.LineAlignment = StringAlignment.Center;
                e.Graphics.DrawString(page.Text, page.Font, Brushes.White,
                    new Rectangle(e.Bounds.X, e.Bounds.Y + 2, e.Bounds.Width, e.Bounds.Height), sf);
            }

            if (isSelected)
            {
                using (var pen = new Pen(Color.FromArgb(0, 150, 255), 3))
                    e.Graphics.DrawLine(pen, e.Bounds.X + 20, e.Bounds.Bottom - 3,
                        e.Bounds.Right - 20, e.Bounds.Bottom - 3);
            }
        }

        // ✅ ТЕПЕРЬ ПРОСТО ИСПОЛЬЗУЕМ СОХРАНЁННУЮ ССЫЛКУ
        public void LoadSavedBuild(Build build)
        {
            tabControl.SelectedIndex = 1;  // Вкладка "Сборка"
            buildTab?.LoadSavedBuild(build);
        }
    }
}