using DynamicFilter.Abstract;
using DynamicFilter.Abstract.Filters;
using DynamicFilter.Help;
using DynamicFilterControls.CollectionHelp;
using DynamicFilterControls.Logic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace DynamicFilterControls
{
    internal partial class DynamicFilterBox : UserControl
    {
        #region Fields
        private int _currentRowIndex;
        private IDictionary<TreeNode, IOperand> _tree;
        private readonly DynamicFilterBoxLogic _dynamicFilterBoxLogic;
        #endregion

        #region Constructors
        public DynamicFilterBox() : this(null, null, DisplaySourceFactory())
        {
        }

        public DynamicFilterBox(Type type, IEnumerable<(Type SrcType, string PropertyName, string DisplayMember, string ValueMember, Func<IEnumerable<ComboContainer>> ValidValues)> validValues, IDictionary<(Type Type, string PropertyName), string> displaySource)
        {
            InitializeComponent();

            tvOperands.Nodes.Add(tnEmpty = new TreeNode("Не назначено"));
            _dynamicFilterBoxLogic = new DynamicFilterBoxLogic(
                type, 
                validValues,
                displaySource, 
                SetInnerOperandOrDefault,
                OperandBuilderDataFormOrDefault,
                ChangeFilterPosition,
                ChangeEnambeBackBattons,
                PathUpdate,
                PrintUpdate,
                FiltersChange,
                OperationBuilderFormOrDefault,
                OperandBuilderFormOrDefault,
                InnerBuilderFormOrDefault,
                UpdateTree);

            _dynamicFilterBoxLogic.

            Root = type;
            if(validValues != null) ValidValuesAddRange(validValues);
        }

        private IOperand InnerBuilderFormOrDefault(ICollectionOperand newCurrent,
            IDictionary<(Type SrcType, string PropertyName), (string DisplayMember, string ValueMember, Func<IEnumerable<ComboContainer>> ValidValues)> ValidValuesDictionary,
            IDictionary<(Type SrcType, string PropertyName), string> DisplaySource)
        {
            var form = new InnerOperandBuilderForm(newCurrent, ValidValuesDictionary, DisplaySource);
            return form.ShowDialog() == DialogResult.OK ? form.Result : null;
        }

        private IOperand OperandBuilderFormOrDefault(IParameterizedFilterOperand newCurrent,
            IEnumerable<IFilterData> activeSource,
            IDictionary<(Type SrcType, string PropertyName), (string DisplayMember, string ValueMember, Func<IEnumerable<ComboContainer>> ValidValues)> validValuesDictionary)
        {
            var form = new OperandBuilderForm(newCurrent, activeSource, validValuesDictionary);
            return form.ShowDialog() == DialogResult.OK ? form.Result : null;
        }

        private void ChangeFilterPosition(int position) => bsFilters.Position = position;
        private void ChangeEnambeBackBattons(bool value) => btBack.Enabled = btHome.Enabled = value;
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
        private void FiltersChange(IList<IFilterData> list) => bsFilters.DataSource = list;

        private IOperand OperationBuilderFormOrDefault(IOperand operand, IEnumerable<IOperand> operands)
        {
            var form = new OperationBuilderForm(operand, operands);
            return form.ShowDialog() == DialogResult.OK ? form.Result : null;
        }

        private IOperand OperandBuilderDataFormOrDefault(IFilterData data, IEnumerable<IFilterData> activeSource, IDictionary<(Type SrcType, string PropertyName), (string DisplayMember, string ValueMember, Func<IEnumerable<ComboContainer>> ValidValues)> ValidValuesDictionary)
        {
            var form = new OperandBuilderForm(data, activeSource, ValidValuesDictionary);
            return form.ShowDialog() == DialogResult.OK ? form.Result : null;
        }

        private IOperand SetInnerOperandOrDefault(IFilterData data, IDictionary<(Type SrcType, string PropertyName), (string DisplayMember, string ValueMember, Func<IEnumerable<ComboContainer>> ValidValues)> validValuesDictionary, IDictionary<(Type SrcType, string PropertyName), string> displayDictionary)
        {
            var form = new InnerOperandBuilderForm(data, validValuesDictionary, displayDictionary);
            return form.ShowDialog() == DialogResult.OK ? form.Result : null;
        }

        public DynamicFilterBox(Type type, DataGridViewColumnCollection columns) : this() => SetByColumns(type, columns);
        #endregion Constructors

        #region Property

        public IOperand Operand
        {
            get => _dynamicFilterBoxLogic.Operand; set => _dynamicFilterBoxLogic.Operand = value;
        }

        public IOperand Result => _dynamicFilterBoxLogic.Result;

        public Type Root
        {
            get => _dynamicFilterBoxLogic.Root; set => _dynamicFilterBoxLogic.Root = value;
        }

        #endregion

        public event EventHandler<EventArgs<IOperand>> ResultBuilt { add => _dynamicFilterBoxLogic.ResultBuilt += value; remove => _dynamicFilterBoxLogic.ResultBuilt -= value; }
        public void SetByColumns(Type type, DataGridViewColumnCollection columns)
        {
            var (sources, display, dictionary) = ControlHelper.CreateDataFromColumns(type, columns);
            _dynamicFilterBoxLogic.Source = sources;
            _dynamicFilterBoxLogic.DisplaySource = display;
            _dynamicFilterBoxLogic.ValidValuesDictionary = dictionary;
        }

        public void SourceAdd(IFilterData item) => _dynamicFilterBoxLogic.SourceAdd(item);

        public void SourceAddRange(IEnumerable<IFilterData> collection) => _dynamicFilterBoxLogic.SourceAddRange(collection);

        public void ValidValuesAddRange(IEnumerable<(Type SrcType, string PropertyName, string DisplayMember, string ValueMember, Func<IEnumerable<ComboContainer>> ValidValues)> validValues) => _dynamicFilterBoxLogic.ValidValuesAddRange(validValues);

        internal void DisplaisAddRange(IEnumerable<(Type SrcType, string propertyName, string displayName)> innerDisplayDictionary) => _dynamicFilterBoxLogic.DisplaisAddRange(innerDisplayDictionary);

        private void DeleteOperand(TreeNode node) => _dynamicFilterBoxLogic.DeleteOperand(node, GetOperandByNode, TryGetParentByNode);

        private IOperand GetOperandByNode(TreeNode node) => _tree[node];

        private IOperand GetParentByNode(TreeNode node)
        {
            var pnode = node.Parent;
            var parent = pnode == null ? null : _tree[pnode];
            return parent;
        }

        private void OnBack(object sender, EventArgs e) => _dynamicFilterBoxLogic.OnBack();

        private void OnBuilt(object sender, EventArgs e) => _dynamicFilterBoxLogic.OnBuilt();

        private void OnCellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex != tbPropertyType.Index || e.RowIndex == -1)
                return;
            if (e.Value is Type t)
            {
                e.Value = _dynamicFilterBoxLogic.OnCellFormatting(bsFilters[e.RowIndex] as IFilterData);
                e.FormattingApplied = true;
            }
        }

        private void OnCellMouseUp(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1) return;
            _currentRowIndex = e.RowIndex;
        }

        private void OnCheck(object sender, EventArgs e)
        {
            if (bsFilters.Current is IFilterData data) _dynamicFilterBoxLogic.AddChechOperand(data);
        }

        private void OnDeleteOperand(object sender, EventArgs e)
        {
            var node = tvOperands.SelectedNode;
            if (node == null || node == tnEmpty) return;
            DeleteOperand(node);
        }

        private void OnDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            if (dgvFilters.Rows[e.RowIndex].DataBoundItem is IFilterData newParent) _dynamicFilterBoxLogic.OpenFilter(newParent);
            else throw new DataException();
        }

        private void OnFilterRowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            int last = e.RowIndex + e.RowCount;
            for (int i = e.RowIndex; i < last; i++)
            {
                var row = dgvFilters.Rows[i];
                if (row.DataBoundItem is IFilterData data && (data.PropertyType.IsNulluble() || !data.PropertyType.IsPrimitive()) && row.ContextMenuStrip == null)
                    row.ContextMenuStrip = cmsFilterEdit;
            }
        }

        private void OnHome(object sender, EventArgs e) => _dynamicFilterBoxLogic.OnHome();

        private void OnInvertClick(object sender, EventArgs e)
        {
            var node = tvOperands.SelectedNode;
            if (node == null || node == tnEmpty)
                return;
            _dynamicFilterBoxLogic.InvertOperand(node, GetOperandByNode, TryGetParentByNode);
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
                    if (node.IsExpanded)
                        node.Collapse();
                    else
                        node.Expand();
                    _dynamicFilterBoxLogic.UpdateOperand(GetOperandByNode(node), GetParentByNode(node));
                    break;

                case MouseButtons.Right:
                    tvOperands.SelectedNode = node;
                    break;

                default:
                    break;
            }
        }

        private void OnOpenedFilterEdit(object sender, EventArgs e) => _dynamicFilterBoxLogic.OnOpenedFilterEdit(_currentRowIndex);

        private void OnOperandsKeyUp(object sender, KeyEventArgs e)
        {
            var node = tvOperands.SelectedNode;
            if (node == null || node == tnEmpty)
                return;

            switch (e.KeyCode)
            {
                case Keys.Delete:
                    _dynamicFilterBoxLogic.DeleteOperand(node, GetOperandByNode, TryGetParentByNode);
                    break;

                case Keys.Enter:
                    _dynamicFilterBoxLogic.UpdateOperand(GetOperandByNode(node), GetParentByNode(node));
                    break;

                default:
                    break;
            }
        }

        private void OnUpdateOperand(object sender, EventArgs e)
        {
            var node = tvOperands.SelectedNode;
            if (node == null || node == tnEmpty) return;
            _dynamicFilterBoxLogic.UpdateOperand(GetOperandByNode(node), GetParentByNode(node));
        }

        private (bool IsSuccess, TreeNode node, IOperand Father) TryGetParentByNode(TreeNode node)
        {
            IOperand parent;
            if ((node = node.Parent) == null)
            {
                parent = null;
                return (false, node, null);
            }

            parent = GetOperandByNode(node);
            return (true, node, parent);
        }

        private void UpdateTree(IOperand operand)
        {
            tvOperands.Nodes.Clear();
            _tree = new Dictionary<TreeNode, IOperand>();

            if (Operand == null)
            {
                tvOperands.Nodes.Add(tnEmpty);

                return;
            }

            var node = tvOperands.Nodes.Add(operand.Name);
            node.ContextMenuStrip = cmsNodeEdit;
            _tree.Add(node, operand);
            ControlHelper.PopulateTreeByOperand(operand, node, _tree, cmsNodeEdit);
            tvOperands.ExpandAll();
        }
        #region Dependency
        private static IDictionary<(Type SrcType, string PropertyName), string> DisplaySourceFactory() => new Dictionary<(Type SrcType, string PropertyName), string>();
        #endregion Dependency
    }
}