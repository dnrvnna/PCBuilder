using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using PCBuilder.Models;
using PCBuilder.Services;

namespace PCBuilder.Forms.Tabs
{
    public class SavedBuildsTab : BaseTab
    {
        private readonly IBuildStorage buildStorage;
        private FlowLayoutPanel buildsFlow;
        private Guna2TextBox txtSearch;

        public SavedBuildsTab(IDataManager dataManager, IBuildStorage buildStorage)  // ← IDataManager
            : base(dataManager)
        {
            this.buildStorage = buildStorage;
        }

        public override string TabName => "💾 Мои сборки";

        public override Control Create()
        {
            TableLayoutPanel mainPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(30, 30, 35),
                RowCount = 2,
                ColumnCount = 1,
                Padding = new Padding(15)
            };
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 70));
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            // Верхняя панель
            Panel topPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(40, 40, 45),
                Padding = new Padding(15)
            };

            Label lblTitle = new Label
            {
                Text = $"💾 Сохранённые сборки ({buildStorage.Builds.Count})",
                Location = new Point(15, 20),
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true
            };

            // Поиск
            txtSearch = new Guna2TextBox
            {
                Size = new Size(250, 36),
                Location = new Point(300, 17),
                PlaceholderText = "🔍 Поиск сборки...",
                BorderRadius = 8,
                FillColor = Color.FromArgb(50, 50, 55),
                ForeColor = Color.White,
                PlaceholderForeColor = Color.Gray,
                Font = new Font("Segoe UI", 10)
            };
            txtSearch.TextChanged += (s, e) => FilterBuilds();

            // Кнопка обновления
            Guna2Button btnRefresh = new Guna2Button
            {
                Text = "🔄 Обновить",
                Size = new Size(120, 36),
                Location = new Point(570, 17),
                BorderRadius = 8,
                FillColor = Color.FromArgb(0, 150, 255),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.White
            };
            btnRefresh.Click += (s, e) => RefreshBuilds();

            topPanel.Controls.AddRange(new Control[] { lblTitle, txtSearch, btnRefresh });

            // Панель с карточками
            buildsFlow = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                AutoScroll = true,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = true,
                Padding = new Padding(10)
            };

            LoadBuilds();

            mainPanel.Controls.Add(topPanel, 0, 0);
            mainPanel.Controls.Add(buildsFlow, 0, 1);

            return mainPanel;
        }

        private void LoadBuilds()
        {
            buildsFlow.Controls.Clear();

            var builds = buildStorage.Builds.OrderByDescending(b => b.CreatedDate);

            if (!builds.Any())
            {
                Label lblEmpty = new Label
                {
                    Text = "📭 У вас пока нет сохранённых сборок\n\nПерейдите на вкладку 'Сборка' и создайте свою первую сборку!",
                    Location = new Point(20, 50),
                    Font = new Font("Segoe UI", 14),
                    ForeColor = Color.Gray,
                    AutoSize = true,
                    TextAlign = ContentAlignment.MiddleCenter
                };
                buildsFlow.Controls.Add(lblEmpty);
                return;
            }

            foreach (var build in builds)
            {
                var card = CreateBuildCard(build);
                buildsFlow.Controls.Add(card);
            }
        }

        private void FilterBuilds()
        {
            string searchText = txtSearch.Text.ToLower();

            buildsFlow.Controls.Clear();

            var filtered = buildStorage.Builds
                .Where(b => string.IsNullOrEmpty(searchText) ||
                           b.Name.ToLower().Contains(searchText) ||
                           b.CategorySummary.ToLower().Contains(searchText))
                .OrderByDescending(b => b.CreatedDate);

            if (!filtered.Any())
            {
                Label lblEmpty = new Label
                {
                    Text = "🔍 Сборки не найдены",
                    Location = new Point(20, 50),
                    Font = new Font("Segoe UI", 14),
                    ForeColor = Color.Gray,
                    AutoSize = true
                };
                buildsFlow.Controls.Add(lblEmpty);
                return;
            }

            foreach (var build in filtered)
            {
                var card = CreateBuildCard(build);
                buildsFlow.Controls.Add(card);
            }
        }

        private Panel CreateBuildCard(Build build)
        {
            Panel card = new Panel
            {
                Width = 320,
                Height = 200,
                BackColor = Color.FromArgb(45, 45, 50),
                Margin = new Padding(8),
                Padding = new Padding(15),
                BorderStyle = BorderStyle.FixedSingle
            };

            // Название
            Label lblName = new Label
            {
                Text = build.Name,
                Location = new Point(12, 12),
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = false,
                Width = 280,
                Height = 25
            };

            // Дата
            Label lblDate = new Label
            {
                Text = $"📅 {build.CreatedDate:dd.MM.yyyy HH:mm}",
                Location = new Point(12, 42),
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.Gray,
                AutoSize = true
            };

            // Количество компонентов
            Label lblComponents = new Label
            {
                Text = $"📦 {build.ComponentCount} компонентов",
                Location = new Point(12, 68),
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.FromArgb(180, 180, 180),
                AutoSize = true
            };

            // Категории
            Label lblCategories = new Label
            {
                Text = build.CategorySummary,
                Location = new Point(12, 92),
                Font = new Font("Segoe UI", 8),
                ForeColor = Color.Gray,
                AutoSize = true,
                MaximumSize = new Size(290, 0)
            };

            // Цена
            Label lblPrice = new Label
            {
                Text = $"💰 {build.TotalPrice:N0} ₽",
                Location = new Point(12, 120),
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = build.IsComplete ? Color.FromArgb(0, 220, 100) : Color.FromArgb(255, 200, 50),
                AutoSize = true
            };

            // Статус
            Label lblStatus = new Label
            {
                Text = build.IsComplete ? "✅ Готова" : "⚠️ Неполная",
                Location = new Point(12, 148),
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = build.IsComplete ? Color.Green : Color.Orange,
                AutoSize = true
            };

            // Кнопка "Открыть"
            Guna2Button btnOpen = new Guna2Button
            {
                Text = "📂 Открыть",
                Size = new Size(90, 32),
                Location = new Point(200, 148),
                BorderRadius = 8,
                FillColor = Color.FromArgb(0, 120, 255),
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = Color.White,
                Tag = build
            };
            btnOpen.Click += (s, e) => OpenBuild(build);

            // Кнопка "Удалить"
            Guna2Button btnDelete = new Guna2Button
            {
                Text = "🗑️",
                Size = new Size(32, 32),
                Location = new Point(270, 10),
                BorderRadius = 16,
                FillColor = Color.FromArgb(200, 50, 50),
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.White,
                Tag = build
            };
            btnDelete.Click += (s, e) => DeleteBuild(build);

            card.Controls.AddRange(new Control[] {
                lblName, lblDate, lblComponents, lblCategories, lblPrice, lblStatus, btnOpen, btnDelete
            });

            return card;
        }

        private void OpenBuild(Build build)
        {
            // Ищем MainForm через Application.OpenForms
            var mainForm = Application.OpenForms["MainForm"] as MainForm;
            if (mainForm != null)
            {
                mainForm.LoadSavedBuild(build);
            }
        }

        private void DeleteBuild(Build build)
        {
            var result = MessageBox.Show($"Удалить сборку '{build.Name}'?", "Подтверждение",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                buildStorage.DeleteBuild(build.Id);
                RefreshBuilds();
            }
        }

        private void RefreshBuilds()
        {
            buildStorage.LoadBuilds();
            LoadBuilds();
        }
    }
}