namespace DynamicFilterControls
{
    partial class InnerOperandBuilder
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.cbCollectionOperand = new System.Windows.Forms.ComboBox();
            this.bsCollectionOperands = new System.Windows.Forms.BindingSource(this.components);
            this.btCreate = new System.Windows.Forms.Button();
            this.btBuilt = new System.Windows.Forms.Button();
            this.tbPrint = new System.Windows.Forms.TextBox();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.bsCollectionOperands)).BeginInit();
            this.SuspendLayout();
            // 
            // cbCollectionOperand
            // 
            this.cbCollectionOperand.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbCollectionOperand.DataSource = this.bsCollectionOperands;
            this.cbCollectionOperand.DisplayMember = "DisplayName";
            this.cbCollectionOperand.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCollectionOperand.FormattingEnabled = true;
            this.cbCollectionOperand.Location = new System.Drawing.Point(3, 5);
            this.cbCollectionOperand.Name = "cbCollectionOperand";
            this.cbCollectionOperand.Size = new System.Drawing.Size(121, 21);
            this.cbCollectionOperand.TabIndex = 0;
            this.cbCollectionOperand.ValueMember = "Type";
            // 
            // bsCollectionOperands
            // 
            this.bsCollectionOperands.DataSource = typeof(DynamicFilterControls.Models.NamedOperation);
            // 
            // btCreate
            // 
            this.btCreate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btCreate.Location = new System.Drawing.Point(130, 5);
            this.btCreate.Name = "btCreate";
            this.btCreate.Size = new System.Drawing.Size(75, 21);
            this.btCreate.TabIndex = 1;
            this.btCreate.Text = "Настроить";
            this.btCreate.UseVisualStyleBackColor = true;
            this.btCreate.Click += new System.EventHandler(this.OnCreate);
            // 
            // btBuilt
            // 
            this.btBuilt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btBuilt.Location = new System.Drawing.Point(130, 32);
            this.btBuilt.Name = "btBuilt";
            this.btBuilt.Size = new System.Drawing.Size(75, 20);
            this.btBuilt.TabIndex = 2;
            this.btBuilt.Text = "Построить";
            this.btBuilt.UseVisualStyleBackColor = true;
            this.btBuilt.Click += new System.EventHandler(this.OnBuild);
            // 
            // tbPrint
            // 
            this.tbPrint.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbPrint.Location = new System.Drawing.Point(3, 32);
            this.tbPrint.Name = "tbPrint";
            this.tbPrint.ReadOnly = true;
            this.tbPrint.Size = new System.Drawing.Size(121, 20);
            this.tbPrint.TabIndex = 3;
            // 
            // toolTip
            // 
            this.toolTip.AutoPopDelay = 10000;
            this.toolTip.InitialDelay = 500;
            this.toolTip.ReshowDelay = 100;
            // 
            // InnerOperandBuilder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tbPrint);
            this.Controls.Add(this.btBuilt);
            this.Controls.Add(this.btCreate);
            this.Controls.Add(this.cbCollectionOperand);
            this.Name = "InnerOperandBuilder";
            this.Size = new System.Drawing.Size(208, 59);
            ((System.ComponentModel.ISupportInitialize)(this.bsCollectionOperands)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.BindingSource bsCollectionOperands;
        private System.Windows.Forms.ComboBox cbCollectionOperand;
        private System.Windows.Forms.Button btCreate;
        private System.Windows.Forms.Button btBuilt;
        private System.Windows.Forms.TextBox tbPrint;
        private System.Windows.Forms.ToolTip toolTip;
    }
}
