using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using PCBuilder.Models;
using PCBuilder.Helpers;

namespace PCBuilder.Forms.Controls
{
    public class CategoryGroup : Panel
    {
        private readonly string categoryName;
        private readonly List<ComponentBase> components;  // ← ComponentBase
        private readonly Action<ComponentBase> onAddClick;  // ← ComponentBase
        private bool isExpanded = true;
        private Panel headerPanel;
        private FlowLayoutPanel contentPanel;
        private Guna2Button btnToggle;
        private Label lblTitle;

        public CategoryGroup(string categoryName, List<ComponentBase> components, Action<ComponentBase> onAddClick)
        {
            this.categoryName = categoryName;
            this.components = components;
            this.onAddClick = onAddClick;

            this.Width = 100;
            this.Height = 45;
            this.BackColor = Color.Transparent;
            this.Margin = new Padding(0, 5, 0, 5);
            this.Padding = new Padding(0);
            this.AutoSize = false;

            InitializeHeader();
            InitializeContent();
            ToggleExpand();
        }

        private void InitializeHeader()
        {
            headerPanel = new Panel
            {
                Width = this.Width - 10,
                Height = 35,
                BackColor = Color.FromArgb(45, 45, 50),
                Cursor = Cursors.Hand,
                Margin = new Padding(0),
                Padding = new Padding(0),
                Dock = DockStyle.Top
            };
            headerPanel.Click += (s, e) => ToggleExpand();

            // Кнопка сворачивания/разворачивания
            btnToggle = new Guna2Button
            {
                Text = "▼",
                Size = new Size(28, 28),
                Location = new Point(5, 3),
                BorderRadius = 14,
                FillColor = Color.FromArgb(55, 55, 60),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.White,
                Cursor = Cursors.Hand,
                Tag = this
            };
            btnToggle.Click += (s, e) => ToggleExpand();

            // Заголовок
            lblTitle = new Label
            {
                Text = $"📌 {categoryName} ({components.Count} шт.)",
                Location = new Point(40, 5),
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.FromArgb(100, 200, 255),
                AutoSize = true
            };

            // Линия
            Panel line = new Panel
            {
                Location = new Point(40, 28),
                Width = headerPanel.Width - 50,
                Height = 2,
                BackColor = Color.FromArgb(60, 60, 65)
            };

            headerPanel.Controls.AddRange(new Control[] { btnToggle, lblTitle, line });
            this.Controls.Add(headerPanel);
        }

        private void InitializeContent()
        {
            contentPanel = new FlowLayoutPanel
            {
                Width = this.Width - 10,
                Height = 0,
                BackColor = Color.Transparent,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = true,
                Padding = new Padding(5, 5, 5, 5),
                Margin = new Padding(0),
                AutoSize = false,
                Dock = DockStyle.Top
            };

            foreach (var component in components)
            {
                var card = CreateCard(component);
                contentPanel.Controls.Add(card);
            }

            this.Controls.Add(contentPanel);
        }

        private Panel CreateCard(ComponentBase component)
        {
            Panel card = new Panel
            {
                Width = 260,
                Height = 140,
                BackColor = Color.FromArgb(45, 45, 50),
                Cursor = Cursors.Hand,
                Padding = new Padding(10),
                Margin = new Padding(4),
                Tag = component
            };

            card.MouseEnter += (s, e) => card.BackColor = Color.FromArgb(55, 55, 60);
            card.MouseLeave += (s, e) => card.BackColor = Color.FromArgb(45, 45, 50);

            // Название
            Label lblName = new Label
            {
                Text = component.Name,
                Location = new Point(8, 6),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = false,
                Width = 225,
                Height = 38,
                TextAlign = ContentAlignment.TopLeft
            };

            // Описание
            Label lblDesc = new Label
            {
                Text = component.Description ?? "",
                Location = new Point(8, 48),
                Font = new Font("Segoe UI", 8),
                ForeColor = Color.Gray,
                AutoSize = false,
                Width = 180,
                Height = 18
            };

            // Характеристики (из Specs)
            string specsText = GetSpecsText(component);
            Label lblSpecs = new Label
            {
                Text = specsText,
                Location = new Point(8, 70),
                Font = new Font("Segoe UI", 7.5f),
                ForeColor = Color.FromArgb(180, 180, 200),
                AutoSize = false,
                Width = 180,
                Height = 30
            };

            // Цена
            Label lblPrice = new Label
            {
                Text = $"{component.Price:N0} ₽",
                Location = new Point(8, 104),
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.FromArgb(100, 200, 255),
                AutoSize = true
            };

            // Кнопка "Добавить"
            Guna2Button btnAdd = new Guna2Button
            {
                Text = "+",
                Size = new Size(32, 32),
                Location = new Point(215, 95),
                BorderRadius = 16,
                FillColor = Color.FromArgb(0, 120, 255),
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Color.White,
                Tag = component
            };
            btnAdd.HoverState.FillColor = Color.FromArgb(0, 150, 255);
            btnAdd.Click += (s, e) => onAddClick?.Invoke(component);

            card.Controls.AddRange(new Control[] { lblName, lblDesc, lblSpecs, lblPrice, btnAdd });
            card.Click += (s, e) => onAddClick?.Invoke(component);

            return card;
        }

        private string GetSpecsText(ComponentBase component)
        {
            if (component.Specs == null || component.Specs.Count == 0)
                return "";

            var specs = component.Specs;
            List<string> parts = new List<string>();

            // Приоритетные характеристики
            if (specs.ContainsKey("Ядра")) parts.Add($"Ядер: {specs["Ядра"]}");
            if (specs.ContainsKey("Потоки")) parts.Add($"Потоков: {specs["Потоки"]}");
            if (specs.ContainsKey("VRAM")) parts.Add($"VRAM: {specs["VRAM"]}");
            if (specs.ContainsKey("Ёмкость")) parts.Add($"Ёмкость: {specs["Ёмкость"]}");
            if (specs.ContainsKey("Частота")) parts.Add($"Частота: {specs["Частота"]}");
            if (specs.ContainsKey("Мощность")) parts.Add($"Мощность: {specs["Мощность"]}");
            if (specs.ContainsKey("Форм-фактор")) parts.Add($"Форм-фактор: {specs["Форм-фактор"]}");
            if (specs.ContainsKey("Тип")) parts.Add($"Тип: {specs["Тип"]}");
            if (specs.ContainsKey("Чтение")) parts.Add($"Чтение: {specs["Чтение"]}");
            if (specs.ContainsKey("Запись")) parts.Add($"Запись: {specs["Запись"]}");
            if (specs.ContainsKey("Макс. частота")) parts.Add($"Макс: {specs["Макс. частота"]}");
            if (specs.ContainsKey("CUDA ядра")) parts.Add($"CUDA: {specs["CUDA ядра"]}");
            if (specs.ContainsKey("Эффективность")) parts.Add($"Эффективность: {specs["Эффективность"]}");
            if (specs.ContainsKey("Модульность")) parts.Add($"Модульность: {specs["Модульность"]}");

            // Если ничего не нашли — берём первые 2 любые характеристики
            if (parts.Count == 0)
            {
                foreach (var kvp in specs.Take(2))
                {
                    parts.Add($"{kvp.Key}: {kvp.Value}");
                }
            }

            return string.Join(" | ", parts.Take(3));
        }

        private void ToggleExpand()
        {
            isExpanded = !isExpanded;

            btnToggle.Text = isExpanded ? "▼" : "▶";

            if (isExpanded)
            {
                contentPanel.Visible = true;
                contentPanel.Height = 0;
                contentPanel.AutoSize = true;
                this.Height = headerPanel.Height + contentPanel.Height + 5;
            }
            else
            {
                contentPanel.Visible = false;
                contentPanel.AutoSize = false;
                contentPanel.Height = 0;
                this.Height = headerPanel.Height + 5;
            }
        }

        public void UpdateWidth(int newWidth)
        {
            this.Width = newWidth - 10;
            headerPanel.Width = this.Width - 10;

            foreach (Control control in headerPanel.Controls)
            {
                if (control is Panel line)
                    line.Width = headerPanel.Width - 50;
            }

            contentPanel.Width = this.Width - 10;
        }
    }
}