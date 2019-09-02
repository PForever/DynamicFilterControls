namespace DynamicFilterControls
{
    partial class LazyGridView
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
            _lazyCollection?.Dispose();
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LazyGridView));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tbSimpleFilter = new System.Windows.Forms.TextBox();
            this.cbSimpleFilter = new System.Windows.Forms.ComboBox();
            this.bsSimpleFilters = new System.Windows.Forms.BindingSource(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.bindingNavigator = new System.Windows.Forms.BindingNavigator(this.components);
            this.bindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.lbCount = new System.Windows.Forms.ToolStripLabel();
            this.btFilter = new System.Windows.Forms.ToolStripSplitButton();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.btSort = new System.Windows.Forms.ToolStripSplitButton();
            this.очиститьToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.btFirst = new System.Windows.Forms.ToolStripButton();
            this.btPrevious = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tbPosition = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.btNext = new System.Windows.Forms.ToolStripButton();
            this.btLast = new System.Windows.Forms.ToolStripButton();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bsSimpleFilters)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator)).BeginInit();
            this.bindingNavigator.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tbSimpleFilter);
            this.splitContainer1.Panel2.Controls.Add(this.cbSimpleFilter);
            this.splitContainer1.Panel2.Controls.Add(this.label1);
            this.splitContainer1.Panel2.Controls.Add(this.bindingNavigator);
            this.splitContainer1.Size = new System.Drawing.Size(737, 393);
            this.splitContainer1.SplitterDistance = 363;
            this.splitContainer1.TabIndex = 0;
            // 
            // tbSimpleFilter
            // 
            this.tbSimpleFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.tbSimpleFilter.Location = new System.Drawing.Point(621, 3);
            this.tbSimpleFilter.Name = "tbSimpleFilter";
            this.tbSimpleFilter.Size = new System.Drawing.Size(113, 20);
            this.tbSimpleFilter.TabIndex = 4;
            this.tbSimpleFilter.TextChanged += new System.EventHandler(this.OnSimpleFilterChanged);
            // 
            // cbSimpleFilter
            // 
            this.cbSimpleFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cbSimpleFilter.DataSource = this.bsSimpleFilters;
            this.cbSimpleFilter.DisplayMember = "DisplayName";
            this.cbSimpleFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSimpleFilter.FormattingEnabled = true;
            this.cbSimpleFilter.Location = new System.Drawing.Point(494, 3);
            this.cbSimpleFilter.MaximumSize = new System.Drawing.Size(121, 0);
            this.cbSimpleFilter.MinimumSize = new System.Drawing.Size(121, 0);
            this.cbSimpleFilter.Name = "cbSimpleFilter";
            this.cbSimpleFilter.Size = new System.Drawing.Size(121, 21);
            this.cbSimpleFilter.TabIndex = 3;
            this.cbSimpleFilter.ValueMember = "FilterData";
            this.cbSimpleFilter.SelectedIndexChanged += new System.EventHandler(this.OnSimpleFilterChanged);
            // 
            // bsSimpleFilters
            // 
            this.bsSimpleFilters.DataSource = typeof(DynamicFilterControls.Models.Alias);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(441, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Фильтр";
            // 
            // bindingNavigator
            // 
            this.bindingNavigator.AddNewItem = null;
            this.bindingNavigator.BindingSource = this.bindingSource;
            this.bindingNavigator.CountItem = this.lbCount;
            this.bindingNavigator.CountItemFormat = "из {0}";
            this.bindingNavigator.DeleteItem = null;
            this.bindingNavigator.Dock = System.Windows.Forms.DockStyle.None;
            this.bindingNavigator.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btFilter,
            this.btSort,
            this.btFirst,
            this.btPrevious,
            this.toolStripSeparator1,
            this.tbPosition,
            this.lbCount,
            this.toolStripSeparator2,
            this.btNext,
            this.btLast});
            this.bindingNavigator.Location = new System.Drawing.Point(0, 1);
            this.bindingNavigator.MoveFirstItem = this.btFirst;
            this.bindingNavigator.MoveLastItem = this.btLast;
            this.bindingNavigator.MoveNextItem = this.btNext;
            this.bindingNavigator.MovePreviousItem = this.btPrevious;
            this.bindingNavigator.Name = "bindingNavigator";
            this.bindingNavigator.PositionItem = this.tbPosition;
            this.bindingNavigator.Size = new System.Drawing.Size(389, 25);
            this.bindingNavigator.TabIndex = 1;
            this.bindingNavigator.Text = "bindingNavigator";
            // 
            // lbCount
            // 
            this.lbCount.Name = "lbCount";
            this.lbCount.Size = new System.Drawing.Size(36, 22);
            this.lbCount.Text = "из {0}";
            this.lbCount.ToolTipText = "Total number of items";
            // 
            // btFilter
            // 
            this.btFilter.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1});
            this.btFilter.Image = global::DynamicFilterControls.Properties.Resources.Settings;
            this.btFilter.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btFilter.Name = "btFilter";
            this.btFilter.Size = new System.Drawing.Size(80, 22);
            this.btFilter.Text = "Фильтр";
            this.btFilter.ButtonClick += new System.EventHandler(this.OnFilter);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(126, 22);
            this.toolStripMenuItem1.Text = "Очистить";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.OnClearFilter);
            // 
            // btSort
            // 
            this.btSort.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.очиститьToolStripMenuItem1});
            this.btSort.Image = global::DynamicFilterControls.Properties.Resources.Settings;
            this.btSort.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btSort.Name = "btSort";
            this.btSort.Size = new System.Drawing.Size(105, 22);
            this.btSort.Text = "Сортировка";
            this.btSort.ButtonClick += new System.EventHandler(this.OnSort);
            // 
            // очиститьToolStripMenuItem1
            // 
            this.очиститьToolStripMenuItem1.Name = "очиститьToolStripMenuItem1";
            this.очиститьToolStripMenuItem1.Size = new System.Drawing.Size(126, 22);
            this.очиститьToolStripMenuItem1.Text = "Очистить";
            this.очиститьToolStripMenuItem1.Click += new System.EventHandler(this.OnClearSort);
            // 
            // btFirst
            // 
            this.btFirst.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btFirst.Image = ((System.Drawing.Image)(resources.GetObject("btFirst.Image")));
            this.btFirst.Name = "btFirst";
            this.btFirst.RightToLeftAutoMirrorImage = true;
            this.btFirst.Size = new System.Drawing.Size(23, 22);
            this.btFirst.Text = "Move first";
            // 
            // btPrevious
            // 
            this.btPrevious.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btPrevious.Image = ((System.Drawing.Image)(resources.GetObject("btPrevious.Image")));
            this.btPrevious.Name = "btPrevious";
            this.btPrevious.RightToLeftAutoMirrorImage = true;
            this.btPrevious.Size = new System.Drawing.Size(23, 22);
            this.btPrevious.Text = "Move previous";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // tbPosition
            // 
            this.tbPosition.AccessibleName = "Position";
            this.tbPosition.AutoSize = false;
            this.tbPosition.Name = "tbPosition";
            this.tbPosition.Size = new System.Drawing.Size(50, 23);
            this.tbPosition.Text = "0";
            this.tbPosition.ToolTipText = "Current position";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // btNext
            // 
            this.btNext.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btNext.Image = ((System.Drawing.Image)(resources.GetObject("btNext.Image")));
            this.btNext.Name = "btNext";
            this.btNext.RightToLeftAutoMirrorImage = true;
            this.btNext.Size = new System.Drawing.Size(23, 22);
            this.btNext.Text = "Move next";
            // 
            // btLast
            // 
            this.btLast.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btLast.Image = ((System.Drawing.Image)(resources.GetObject("btLast.Image")));
            this.btLast.Name = "btLast";
            this.btLast.RightToLeftAutoMirrorImage = true;
            this.btLast.Size = new System.Drawing.Size(23, 22);
            this.btLast.Text = "Move last";
            // 
            // LazyGridView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.MinimumSize = new System.Drawing.Size(737, 393);
            this.Name = "LazyGridView";
            this.Size = new System.Drawing.Size(737, 393);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bsSimpleFilters)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator)).EndInit();
            this.bindingNavigator.ResumeLayout(false);
            this.bindingNavigator.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.BindingSource bindingSource;
        private System.Windows.Forms.BindingNavigator bindingNavigator;
        private System.Windows.Forms.ToolStripSplitButton btSort;
        private System.Windows.Forms.ToolStripMenuItem очиститьToolStripMenuItem1;
        private System.Windows.Forms.ToolStripSplitButton btFilter;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.BindingSource bsSimpleFilters;
        private System.Windows.Forms.ComboBox cbSimpleFilter;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbSimpleFilter;
        private System.Windows.Forms.ToolStripButton btFirst;
        private System.Windows.Forms.ToolStripButton btPrevious;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripTextBox tbPosition;
        private System.Windows.Forms.ToolStripLabel lbCount;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton btNext;
        private System.Windows.Forms.ToolStripButton btLast;
    }
}
