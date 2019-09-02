namespace DynamicFilterControls
{
    partial class OperandBuilderForm
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
            this.operandBuilder = new DynamicFilterControls.OperandBuilder();
            this.SuspendLayout();
            // 
            // operandBuilder
            // 
            this.operandBuilder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.operandBuilder.Location = new System.Drawing.Point(0, 0);
            this.operandBuilder.Name = "operandBuilder";
            this.operandBuilder.Properties = null;
            this.operandBuilder.Property = null;
            this.operandBuilder.Size = new System.Drawing.Size(388, 74);
            this.operandBuilder.TabIndex = 0;
            // 
            // OperandBuilderForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(388, 74);
            this.Controls.Add(this.operandBuilder);
            this.MaximumSize = new System.Drawing.Size(1000, 113);
            this.MinimumSize = new System.Drawing.Size(404, 113);
            this.Name = "OperandBuilderForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Редактор значения";
            this.ResumeLayout(false);

        }

        #endregion

        private DynamicFilterControls.OperandBuilder operandBuilder;
    }
}