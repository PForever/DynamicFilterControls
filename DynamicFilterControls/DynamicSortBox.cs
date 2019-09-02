using DynamicFilter.Abstract.Filters;
using DynamicFilter.Abstract.Sort;
using DynamicFilter.Factories;
using DynamicFilter.Help;
using DynamicFilter.Operands;
using DynamicFilterControls.CollectionHelp;
using DynamicFilterControls.Logic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace DynamicFilterControls
{
    internal partial class DynamicSortBox : UserControl
    {
        #region Fields
        private IDictionary<TreeNode, ISortDataOperand> _tree;
        private int _currentRowIndex;
        private readonly DynamicSortBoxLogic _dynamicSortBoxLogic;
        #endregion

        #region Constructors
        public DynamicSortBox() : this(null, DisplaySourceFactory())
        {
        }

        public DynamicSortBox(Type type, IDictionary<(Type Type, string PropertyName), string> displaySource)
        {
            InitializeComponent();
            tvOperands.Nodes.Add(tnEmpty = new TreeNode("Не назначено"));

            _dynamicSortBoxLogic = new DynamicSortBoxLogic(
                type,
                displaySource,
                ChangeFilterPosition,
                ChangeEnambeBackBattons,
                PathUpdate,
                PrintUpdate,
                FiltersChange,
                SetUpEnable,
                SetDownEnable,
                UpdateTree,
                Selected);
        }

        private void SetDownEnable(bool value) => btDownFilter.Enabled = value;
        private void SetUpEnable(bool value) => btUpFilter.Enabled = value;
        private void FiltersChange(IList<IFilterData> filters) => bsFilters.DataSource = filters;
        private void PathUpdate(string text)
        {
            lbPath.Text = text;
            toolTip.SetToolTip(lbPath, text);
        }
        private void PrintUpdate(string text)
        {
            tbPrint.Text = text;
            toolTip.SetToolTip(tbPrint, text);
        }

        private void ChangeEnambeBackBattons(bool value) => btBack.Enabled = value;
        private void ChangeFilterPosition(int index) => bsFilters.Position = index;

        public DynamicSortBox(Type type, DataGridViewColumnCollection columns) : this() => SetByColumns(type, columns);
        #endregion Constructors

        #region Properties

        public Type Root
        {
            get => _dynamicSortBoxLogic.Root;
            set => _dynamicSortBoxLogic.Root = value;
        }

        public IMultiSortOperand Operand
        {
            get => _dynamicSortBoxLogic.Operand;
            set => _dynamicSortBoxLogic.Operand = value;
        }

        public IMultiSortOperand Result => _dynamicSortBoxLogic.Result;

        #endregion

        public event EventHandler<EventArgs<IMultiSortOperand>> ResultBuilt
        {
            add => _dynamicSortBoxLogic.ResultBuilt += value;
            remove => _dynamicSortBoxLogic.ResultBuilt -= value;
        }
        public void SetByColumns(Type type, DataGridViewColumnCollection columns)
        {
            var (sources, display, _) = ControlHelper.CreateDataFromColumns(type, columns);
            _dynamicSortBoxLogic.Source = sources;
            _dynamicSortBoxLogic.DisplaySource = display;
        }

        public void SourceAdd(IFilterData item) => _dynamicSortBoxLogic.SourceAdd(item);

        public void SourceAddRange(IEnumerable<IFilterData> collection)
        {
            foreach (var item in collection) SourceAdd(item);
        }

        internal void DisplaisAddRange(IEnumerable<(Type SrcType, string propertyName, string displayName)> innerDisplayDictionary) => _dynamicSortBoxLogic.DisplaisAddRange(innerDisplayDictionary);

        private ISortDataOperand GetOperandByNode(TreeNode node) => _tree[node];

        private void OnBack(object sender, EventArgs e) => _dynamicSortBoxLogic.OnBack();

        private void OnBuilt(object sender, EventArgs e) => _dynamicSortBoxLogic.OnBuilt();

        private void OnCellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex != tbPropertyType.Index || e.RowIndex == -1)
                return;
            var data = bsFilters[e.RowIndex] as IFilterData;
            e.Value = _dynamicSortBoxLogic.OnCellFormatting(data);
            e.FormattingApplied = true;
        }

        private void OnCellMouseUp(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1) return;
            _currentRowIndex = e.RowIndex;
            //_currentRow = dgvFilters.Rows[e.RowIndex];
        }

        private void OnDeleteOperand(object sender, EventArgs e) => _dynamicSortBoxLogic.DeleteOperand();

        private void OnDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (dgvFilters.Rows[e.RowIndex].DataBoundItem is IFilterData newParent) _dynamicSortBoxLogic.OpenFilter(newParent);
            else throw new DataException();
        }

        private void OnFilterRowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            int last = e.RowIndex + e.RowCount;
            for (int i = e.RowIndex; i < last; i++)
            {
                var row = dgvFilters.Rows[i];
                if (row.DataBoundItem is IFilterData data 
                    && !data.PropertyType.IsPrimitive()
                    && (data.PropertyType.IsComparable())
                    && row.ContextMenuStrip == null)
                {
                    row.ContextMenuStrip = cmsFilterEdit;
                }
            }
        }
        private void OnAdd(object sender, EventArgs e)
        {
            if (bsFilters.Current is IFilterData data) _dynamicSortBoxLogic.CreateOperand(data);
        }
        private void OnHome(object sender, EventArgs e) => _dynamicSortBoxLogic.OnHome();

        private void OnInvertClick(object sender, EventArgs e) => _dynamicSortBoxLogic.InvertSort();

        private ISortDataOperand Selected()
        {
            var node = tvOperands.SelectedNode;
            return (node == null || node == tnEmpty) ? null : GetOperandByNode(node);
        }

        private void OnNodeClicked(object sender, TreeNodeMouseClickEventArgs e)
        {
            var node = e.Node;
            if (node == tnEmpty)
                return;
            switch (e.Button)
            {
                case MouseButtons.Right:
                    tvOperands.SelectedNode = node;
                    break;

                default:
                    break;
            }
        }

        private void OnNodeDoubleClicked(object sender, TreeNodeMouseClickEventArgs e)
        {
            var node = e.Node;
            if (!node.IsSelected || node == tnEmpty)
                return;

            switch (e.Button)
            {
                case MouseButtons.Left:
                    if (node.IsExpanded) node.Collapse();
                    else node.Expand();
                    _dynamicSortBoxLogic.InvertSort();
                    break;

                case MouseButtons.Right:
                    tvOperands.SelectedNode = node;
                    break;

                default:
                    break;
            }
        }

        private void OnOpenedFilterEdit(object sender, EventArgs e) => _dynamicSortBoxLogic.OnOpenedFilterEdit(_currentRowIndex);

        private void OnOperandsKeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Delete:
                    _dynamicSortBoxLogic.DeleteOperand();
                    break;

                case Keys.Enter:
                    _dynamicSortBoxLogic.InvertSort();
                    break;

                default:
                    break;
            }
        }

        #region Dependency

        #endregion Dependency
        private void UpdateTree(IMultiSortOperand operand)
        {
            tvOperands.Nodes.Clear();
            _tree = new Dictionary<TreeNode, ISortDataOperand>();

            if (operand == null)
            {
                tvOperands.Nodes.Add(tnEmpty);

                return;
            }
            foreach (var prop in operand.SortProperties)
            {
                var node = tvOperands.Nodes.Add(prop.Name);
                node.ContextMenuStrip = cmsNodeEdit;
                _tree.Add(node, prop);
            }
            tvOperands.ExpandAll();
        }

        private void OnUpFilter(object sender, EventArgs e) => _dynamicSortBoxLogic.OnUpFilter();

        private void OnDownFilter(object sender, EventArgs e) => _dynamicSortBoxLogic.OnDownFilter();

        private void OnAfterSelect(object sender, TreeViewEventArgs e)
        {
            _dynamicSortBoxLogic.ChechSelected();
        }

        private static IDictionary<(Type SrcType, string PropertyName), string> DisplaySourceFactory() => new Dictionary<(Type SrcType, string PropertyName), string>();
    }
}