namespace DynamicFilterControls
{
    partial class InnerOperandBuilderForm
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
            this.components = new System.ComponentModel.Container();
            this.bsCollectionOperands = new System.Windows.Forms.BindingSource(this.components);
            this.innerOperandBuilder = new DynamicFilterControls.InnerOperandBuilder();
            ((System.ComponentModel.ISupportInitialize)(this.bsCollectionOperands)).BeginInit();
            this.SuspendLayout();
            // 
            // innerOperandBuilder
            // 
            this.innerOperandBuilder.Data = null;
            this.innerOperandBuilder.DisplaySource = null;
            this.innerOperandBuilder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.innerOperandBuilder.Location = new System.Drawing.Point(0, 0);
            this.innerOperandBuilder.Name = "innerOperandBuilder";
            this.innerOperandBuilder.Result = null;
            this.innerOperandBuilder.Size = new System.Drawing.Size(219, 59);
            this.innerOperandBuilder.TabIndex = 0;
            this.innerOperandBuilder.ValidValuesDictionary = null;
            // 
            // InnerOperandBuilderForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(219, 59);
            this.Controls.Add(this.innerOperandBuilder);
            this.MaximumSize = new System.Drawing.Size(235, 98);
            this.MinimumSize = new System.Drawing.Size(235, 98);
            this.Name = "InnerOperandBuilderForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Внутренний фильтр";
            ((System.ComponentModel.ISupportInitialize)(this.bsCollectionOperands)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.BindingSource bsCollectionOperands;
        private InnerOperandBuilder innerOperandBuilder;
    }
}