namespace DynamicFilterControls
{
    partial class OperationBuilderForm
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
            this.SuspendLayout();
            // 
            // operationBuilder
            // 
            this.operationBuilder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.operationBuilder.Location = new System.Drawing.Point(0, 0);
            this.operationBuilder.Name = "operationBuilder";
            this.operationBuilder.Size = new System.Drawing.Size(380, 77);
            this.operationBuilder.TabIndex = 0;
            // 
            // OperationBuilderForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(380, 77);
            this.Controls.Add(this.operationBuilder);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Name = "OperationBuilderForm";
            this.Text = "Редактор операций";
            this.ResumeLayout(false);

        }

        #endregion

        private OperationBuilder operationBuilder;
    }
}