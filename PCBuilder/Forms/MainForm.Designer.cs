using System.Windows.Forms;

namespace PCBuilder.Forms
{
    partial class MainForm  // ← ДОБАВИТЬ partial
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1200, 700);
            this.Text = "PCBuilder - Сборка ПК";
            this.StartPosition = FormStartPosition.CenterScreen;
        }
    }
}