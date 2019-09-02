namespace DynamicFilterControls
{
    partial class OperandBuilder
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
            this.cbProperty = new System.Windows.Forms.ComboBox();
            this.bsProperties = new System.Windows.Forms.BindingSource(this.components);
            this.cbOperations = new System.Windows.Forms.ComboBox();
            this.bsOperations = new System.Windows.Forms.BindingSource(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.tbValue = new System.Windows.Forms.TextBox();
            this.dtpValue = new System.Windows.Forms.DateTimePicker();
            this.nudValue = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btBuild = new System.Windows.Forms.Button();
            this.cbValue = new System.Windows.Forms.ComboBox();
            this.bsValues = new System.Windows.Forms.BindingSource(this.components);
            this.chbIsNull = new System.Windows.Forms.CheckBox();
            this.chbValue = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.bsProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsOperations)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsValues)).BeginInit();
            this.SuspendLayout();
            // 
            // cbProperty
            // 
            this.cbProperty.DataSource = this.bsProperties;
            this.cbProperty.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbProperty.FormattingEnabled = true;
            this.cbProperty.Location = new System.Drawing.Point(3, 19);
            this.cbProperty.Name = "cbProperty";
            this.cbProperty.Size = new System.Drawing.Size(121, 21);
            this.cbProperty.TabIndex = 0;
            // 
            // bsProperties
            // 
            this.bsProperties.PositionChanged += new System.EventHandler(this.OnPropertyChanged);
            // 
            // cbOperations
            // 
            this.cbOperations.DataSource = this.bsOperations;
            this.cbOperations.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbOperations.FormattingEnabled = true;
            this.cbOperations.Location = new System.Drawing.Point(130, 19);
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
            this.label1.Location = new System.Drawing.Point(30, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Свойство";
            // 
            // tbValue
            // 
            this.tbValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbValue.Location = new System.Drawing.Point(257, 20);
            this.tbValue.Name = "tbValue";
            this.tbValue.Size = new System.Drawing.Size(130, 20);
            this.tbValue.TabIndex = 4;
            // 
            // dtpValue
            // 
            this.dtpValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dtpValue.Location = new System.Drawing.Point(257, 20);
            this.dtpValue.Name = "dtpValue";
            this.dtpValue.Size = new System.Drawing.Size(130, 20);
            this.dtpValue.TabIndex = 5;
            // 
            // nudValue
            // 
            this.nudValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.nudValue.Location = new System.Drawing.Point(257, 20);
            this.nudValue.Name = "nudValue";
            this.nudValue.Size = new System.Drawing.Size(130, 20);
            this.nudValue.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(158, 3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Операция";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(285, 3);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Значение";
            // 
            // btBuild
            // 
            this.btBuild.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btBuild.Location = new System.Drawing.Point(312, 46);
            this.btBuild.Name = "btBuild";
            this.btBuild.Size = new System.Drawing.Size(75, 23);
            this.btBuild.TabIndex = 9;
            this.btBuild.Text = "OK";
            this.btBuild.UseVisualStyleBackColor = true;
            this.btBuild.Click += new System.EventHandler(this.OnBuild);
            // 
            // cbValue
            // 
            this.cbValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbValue.DataSource = this.bsValues;
            this.cbValue.DropDownWidth = 127;
            this.cbValue.FormattingEnabled = true;
            this.cbValue.Location = new System.Drawing.Point(257, 20);
            this.cbValue.Name = "cbValue";
            this.cbValue.Size = new System.Drawing.Size(127, 21);
            this.cbValue.TabIndex = 10;
            this.cbValue.TextUpdate += new System.EventHandler(this.OnCbTextUpdate);
            this.cbValue.Format += new System.Windows.Forms.ListControlConvertEventHandler(this.OnValueFormatted);
            this.cbValue.KeyUp += new System.Windows.Forms.KeyEventHandler(this.OnCbKeyUp);
            // 
            // chbIsNull
            // 
            this.chbIsNull.AutoSize = true;
            this.chbIsNull.Location = new System.Drawing.Point(257, 50);
            this.chbIsNull.Name = "chbIsNull";
            this.chbIsNull.Size = new System.Drawing.Size(42, 17);
            this.chbIsNull.TabIndex = 11;
            this.chbIsNull.Text = "null";
            this.chbIsNull.UseVisualStyleBackColor = true;
            this.chbIsNull.CheckedChanged += new System.EventHandler(this.OnCheckedChanged);
            // 
            // chbValue
            // 
            this.chbValue.AutoSize = true;
            this.chbValue.Location = new System.Drawing.Point(303, 23);
            this.chbValue.Name = "chbValue";
            this.chbValue.Size = new System.Drawing.Size(15, 14);
            this.chbValue.TabIndex = 12;
            this.chbValue.UseVisualStyleBackColor = true;
            // 
            // OperandBuilder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.chbValue);
            this.Controls.Add(this.chbIsNull);
            this.Controls.Add(this.cbValue);
            this.Controls.Add(this.btBuild);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.nudValue);
            this.Controls.Add(this.dtpValue);
            this.Controls.Add(this.tbValue);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbOperations);
            this.Controls.Add(this.cbProperty);
            this.Name = "OperandBuilder";
            this.Size = new System.Drawing.Size(390, 74);
            ((System.ComponentModel.ISupportInitialize)(this.bsProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsOperations)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsValues)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cbProperty;
        private System.Windows.Forms.ComboBox cbOperations;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbValue;
        private System.Windows.Forms.DateTimePicker dtpValue;
        private System.Windows.Forms.NumericUpDown nudValue;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.BindingSource bsProperties;
        private System.Windows.Forms.BindingSource bsOperations;
        private System.Windows.Forms.Button btBuild;
        private System.Windows.Forms.ComboBox cbValue;
        private System.Windows.Forms.BindingSource bsValues;
        private System.Windows.Forms.CheckBox chbIsNull;
        private System.Windows.Forms.CheckBox chbValue;
    }
}
