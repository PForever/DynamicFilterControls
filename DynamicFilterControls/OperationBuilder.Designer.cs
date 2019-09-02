namespace DynamicFilterControls
{
    partial class OperationBuilder
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
            this.cbOperand1 = new System.Windows.Forms.ComboBox();
            this.bsOperands1 = new System.Windows.Forms.BindingSource(this.components);
            this.cbOperations = new System.Windows.Forms.ComboBox();
            this.bsOperations = new System.Windows.Forms.BindingSource(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.cbOperand2 = new System.Windows.Forms.ComboBox();
            this.bsOperands2 = new System.Windows.Forms.BindingSource(this.components);
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btBuild = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.bsOperands1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsOperations)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsOperands2)).BeginInit();
            this.SuspendLayout();
            // 
            // cbOperand1
            // 
            this.cbOperand1.DataSource = this.bsOperands1;
            this.cbOperand1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbOperand1.FormattingEnabled = true;
            this.cbOperand1.ItemHeight = 13;
            this.cbOperand1.Location = new System.Drawing.Point(3, 20);
            this.cbOperand1.Name = "cbOperand1";
            this.cbOperand1.Size = new System.Drawing.Size(121, 21);
            this.cbOperand1.TabIndex = 0;
            // 
            // cbOperations
            // 
            this.cbOperations.DataSource = this.bsOperations;
            this.cbOperations.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbOperations.FormattingEnabled = true;
            this.cbOperations.Location = new System.Drawing.Point(130, 20);
            this.cbOperations.Name = "cbOperations";
            this.cbOperations.Size = new System.Drawing.Size(121, 21);
            this.cbOperations.TabIndex = 1;
            // 
            // bsOperations
            // 
            this.bsOperations.DataSource = typeof(DynamicFilterControls.Models.NamedOperation);
            this.bsOperations.PositionChanged += new System.EventHandler(this.OnOperationChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(30, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Операнд 1";
            // 
            // cbOperand2
            // 
            this.cbOperand2.DataSource = this.bsOperands2;
            this.cbOperand2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbOperand2.FormattingEnabled = true;
            this.cbOperand2.Location = new System.Drawing.Point(257, 20);
            this.cbOperand2.Name = "cbOperand2";
            this.cbOperand2.Size = new System.Drawing.Size(121, 21);
            this.cbOperand2.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(158, 4);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Операция";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(285, 4);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Операнд 2";
            // 
            // btBuild
            // 
            this.btBuild.Location = new System.Drawing.Point(300, 47);
            this.btBuild.Name = "btBuild";
            this.btBuild.Size = new System.Drawing.Size(75, 23);
            this.btBuild.TabIndex = 9;
            this.btBuild.Text = "Построить";
            this.btBuild.UseVisualStyleBackColor = true;
            this.btBuild.Click += new System.EventHandler(this.OnBuilt);
            // 
            // OperationBuilder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btBuild);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cbOperand2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbOperations);
            this.Controls.Add(this.cbOperand1);
            this.Name = "OperationBuilder";
            this.Size = new System.Drawing.Size(382, 72);
            ((System.ComponentModel.ISupportInitialize)(this.bsOperands1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsOperations)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsOperands2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cbOperand1;
        private System.Windows.Forms.ComboBox cbOperations;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbOperand2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.BindingSource bsOperands1;
        private System.Windows.Forms.BindingSource bsOperations;
        private System.Windows.Forms.BindingSource bsOperands2;
        private System.Windows.Forms.Button btBuild;
    }
}
