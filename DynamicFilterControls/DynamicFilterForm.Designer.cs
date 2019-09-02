namespace DynamicFilterControls
{
    partial class DynamicFilterForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DynamicFilterForm));
            this.ucFilterBox = new DynamicFilterControls.DynamicFilterBox();
            this.SuspendLayout();
            // 
            // ucFilterBox
            // 
            this.ucFilterBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucFilterBox.Location = new System.Drawing.Point(0, 0);
            this.ucFilterBox.Name = "ucFilterBox";
            this.ucFilterBox.Operand = null;
            this.ucFilterBox.Root = null;
            this.ucFilterBox.Size = new System.Drawing.Size(714, 310);
            this.ucFilterBox.TabIndex = 0;
            // 
            // DynamicFilterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(714, 310);
            this.Controls.Add(this.ucFilterBox);
            this.Name = "DynamicFilterForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Фильтр";
            this.ResumeLayout(false);

        }

        #endregion

        private DynamicFilterControls.DynamicFilterBox ucFilterBox;
    }
}

