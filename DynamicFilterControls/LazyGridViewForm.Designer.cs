namespace DynamicFilterControls
{
    partial class LazyGridViewForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.lazyGridView1 = new DynamicFilterControls.LazyGridView();
            this.lazyGridView1.Panel.SuspendLayout();
            //((System.ComponentModel.ISupportInitialize)(this.stylishedDataGridView1.Grid)).BeginInit();
            this.SuspendLayout();
            // 
            // lazyGridView1
            // 
            this.lazyGridView1.AdditionalProperty = null;
            this.lazyGridView1.DefaultElement = null;
            this.lazyGridView1.FastFilterData = null;
            this.lazyGridView1.InnerDisplayDictionary = null;
            this.lazyGridView1.InnerValidValuesDictionary = null;
            this.lazyGridView1.Location = new System.Drawing.Point(26, 35);
            this.lazyGridView1.MinimumSize = new System.Drawing.Size(694, 393);
            this.lazyGridView1.Name = "lazyGridView1";
            // 
            // lazyGridView1.Panel
            // 
            this.lazyGridView1.Size = new System.Drawing.Size(694, 393);
            this.lazyGridView1.TabIndex = 0;
            // 
            // stylishedDataGridView1.Grid
            // 
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.Yellow;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Times New Roman", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // LazyGridViewForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.lazyGridView1);
            this.Name = "LazyGridViewForm";
            this.Text = "LazyGridViewForm";
            this.lazyGridView1.Panel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private LazyGridView lazyGridView1;
    }
}