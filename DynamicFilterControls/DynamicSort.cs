using DynamicFilter.Operands;
using DynamicFilter.Operands.Abstract;
using DynamicFilter.Operands.Abstract.Filters;
using DynamicFilter.Operands.Help;
using DynamicFilterControls.CollectionHelp;
using DynamicFilterControls.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace DynamicFilterControls
{
    public partial class DynamicFilterBox : UserControl
    {
        #region Fields
        private static IList<IFilterData> _activeSource;
        private (int level, IFilterData parent) _current;
        private IDictionary<(Type SrcType, string PropertyName), string> _displaySource;
        private IList<IDictionary<IFilterData, IList<IFilterData>>> _filterTree;
        private IOperand _operand;
        private Type _root;
        private IFilterData _rootData;
        private IList<IFilterData> _source;
        private IDictionary<TreeNode, IOperand> _tree;
        private IDictionary<(Type SrcType, string PropertyName), (string DisplayMember, string ValueMember, IEnumerable<object> ValidValues)> _validValuesDictionary;
        #endregion New Region

        #region Constructors
        public DynamicFilterBox() : this(null, null, DisplaySourceFactory())
        {
        }

        public DynamicFilterBox(Type type, IEnumerable<(Type SrcType, string PropertyName, string DisplayMember, string ValueMember, IEnumerable<object> ValidValues)> validValues, IDictionary<(Type Type, string PropertyName), string> displaySource)
        {
            InitializeComponent();
            _validValuesDictionary = ValidValuesDictionaryFactory();
            _displaySource = displaySource;
            tvOperands.Nodes.Add(tnEmpty = new TreeNode("Не назначено"));
            _filterTree = TreeFactory();
            _activeSource = ActiveSourceFactory();
            Root = type;
            if(validValues != null) ValidValuesAddRange(validValues);
        }

        public DynamicFilterBox(Type type, DataGridViewColumnCollection columns) : this()
        {
            SetByColumns(type, columns);
        }
        #endregion Constructors

        public event EventHandler<EventArgs<IOperand>> ResultBuilt;

        public IList<IOperand> AllOperands { get; private set; }
        public int Count => _source.Count;
        public IDictionary<(Type SrcType, string PropertyName), string> DisplaySource
        {
            get => _displaySource; set
            {
                _displaySource = value ?? throw new ArgumentNullException(nameof(DisplaySource));
                UpdateSource();
            }
        }

        private void UpdateSource()
        {
            Source = Source;
        }

        public IOperand Operand
        {
            get => _operand;
            set
            {
                _operand = value;
                UpdateOperandView();
            }
        }

        public IOperand Result
        {
            get; private set;
        }

        public void ValidValuesAddRange(IEnumerable<(Type SrcType, string PropertyName, string DisplayMember, string ValueMember, IEnumerable<object> ValidValues)> validValues)
        {
            foreach (var (SrcType, PropertyName, DisplayMember, ValueMember, ValidValues) in validValues)
            {
                _validValuesDictionary.Add((SrcType: SrcType, PropertyName: PropertyName), (nameof(ComboContainer.DisplayMember), nameof(ComboContainer.ValueMember), 
                    ValidValues.Select(v => new ComboContainer
                    {
                        DisplayMember = v == null ? Helper.DefaultMessage : GetPropertyString(v, DisplayMember),
                        ValueMember = v == null ? v : GetPropertyObject(v, ValueMember)
                    })));
            }
            UpdateSource();
        }

        private static string GetPropertyString(object v, string member) => string.IsNullOrEmpty(member) ? v.ToString() : Helper.GetProperty<string>(v, member);
        private static object GetPropertyObject(object v, string member) => string.IsNullOrEmpty(member) ? v : Helper.GetProperty<object>(v, member);

        internal void DisplaisAddRange(IEnumerable<(Type SrcType, string propertyName, string displayName)> innerDisplayDictionary)
        {
            foreach (var (SrcType, propertyName, displayName) in innerDisplayDictionary)
            {
                _displaySource.Add((SrcType, propertyName), displayName);
                UpdateSource();
            }
        }

        public Type Root
        {
            get { return _root; }
            set
            {
                if (value == null)
                {
                    ClearSource();
                    return;
                }
                _root = value;
                Source = FilterDataFactory.CreateChildren(value);
            }
        }

        public IEnumerable<IFilterData> Source
        {
            get => _source?.AsEnumerable();
            set
            {
                ClearSource();
                if (value == null)
                    return;
                _source = new List<IFilterData>();
                foreach (var item in value) SourceAdd(item);
                AllOperands = _source.Select(Factory.EqualsFilter).Cast<IOperand>().ToList();
                if (_source.Count == 0) throw new ArgumentException(nameof(value));
                OnSourceChanged(_source);
            }
        }

        public void SourceAdd(IFilterData item)
        {
            var type = TypeHelper.GetTypeOfParent(item);
            var key = (type, item.PropertyName);
            if (!item.IsDisplayed && DisplaySource.ContainsKey(key))
            {
                item.DisplayName = DisplaySource[key];
                item.IsDisplayed = true;
            }
            if (!item.IsFromSource && ValidValuesDictionary.ContainsKey(key))
            {
                item.ValidValues = ValidValuesDictionary[key];
                item.IsFromSource = true;
            }
            _source.Add(item);
            TreeAdd(item);
        }
        public void SourceAddRange(IEnumerable<IFilterData> collection)
        {
            foreach (var item in collection) SourceAdd(item);
        }

        private IDictionary<(Type SrcType, string PropertyName), (string DisplayMember, string ValueMember, IEnumerable<object> ValidValues)> ValidValuesDictionary
        {
            get => _validValuesDictionary; set
            {
                _validValuesDictionary = value;
                UpdateSource();
            }
        }
        private (int level, IFilterData parent) Current
        {
            get => _current;
            set
            {
                _current = value;
                OnChangeCurrent(_current);
            }
        }

        public void SetByColumns(Type type, DataGridViewColumnCollection columns)
        {
            var (sources, display, dictionary) = Helper.CreateDataFromColumns(type, columns);
            _validValuesDictionary = dictionary;
            _displaySource = display;
            Source = sources;
        }

        private void Calculate() => Result = Operand;

        private void ChengeInnerValue(IFilterData newCurrent)
        {
            var builder = new InnerOperandBuilderForm(newCurrent, ValidValuesDictionary, DisplaySource);

            if (builder.ShowDialog() != DialogResult.OK)
                return;
            var operand = builder.Result;
            Operand = Operand == null ? Operand = operand : Operand.AndAlso(operand);
        }

        private void ChengeValue(IFilterData newCurrent, (string DisplayMember, string ValueMember, IEnumerable<object> ValidValues)? validValuesData = null)
        {
            if (!_activeSource.Contains(newCurrent))
                _activeSource.Add(newCurrent);
            var builder = new OperandBuilderForm(newCurrent, _activeSource, ValidValuesDictionary);

            if (builder.ShowDialog() != DialogResult.OK)
                return;
            var operand = builder.Result;
            Operand = Operand == null ? Operand = operand : Operand.AndAlso(operand);
        }

        private void ClearSource()
        {
            _root = null;
            _source = null;
            _filterTree.Clear();
            _current = default((int level, IFilterData parent));
        }

        private void ClearTree()
        {
            Operand = null;
        }

        private void DeleteOperand(TreeNode node)
        {
            IOperand current, father = GetOperandByNode(node);

            do
            {
                current = father;
                if (!TryGetParentByNode(ref node, out father))
                {
                    ClearTree();
                    return;
                }
            } while (!(father is IBinaryOperand));

            Action<IOperand> changeChild;

            if (!TryGetParentByNode(ref node, out var grandfather))
                changeChild = ch => Operand = ch;
            else
            {
                void ChangeChild(IOperand newChild)
                {
                    switch (grandfather)
                    {
                        case ICollectionOperand o:
                            o.InnerOperand = newChild;
                            break;

                        case IBinaryOperand o:
                            if (o.Left == father)
                                o.Left = newChild;
                            else if (o.Right == father)
                                o.Right = newChild;
                            else
                                throw new Exception();
                            break;

                        case IUnoOperand o:
                            o.Operand = newChild;
                            break;

                        case null:
                            throw new ArgumentNullException(nameof(grandfather));
                        default:
                            throw new NotSupportedException(grandfather.GetType().FullName);
                    }
                }
                changeChild = ChangeChild;
            }
            var binary = father as IBinaryOperand;

            if (binary.Left == current)
                changeChild(binary.Right);
            else if (binary.Right == current)
                changeChild(binary.Left);
            else
                throw new Exception();

            UpdateOperandView();
        }

        private IOperand GetOperandByNode(TreeNode node) => _tree[node];

        private IFilterData GetParent(IList<IFilterData> source)
        {
            IFilterData GetParentOne(IFilterData current)
            {
                var parent = current.Parent;
                while (parent != null)
                {
                    current = parent;
                    parent = current.Parent;
                }
                return current;
            }
            var rootData = source.Select(GetParentOne).Distinct().Single();
            _root = rootData.PropertyType;
            return rootData;
        }

        private IOperand GetParentByNode(TreeNode node)
        {
            var pnode = node.Parent;
            var parent = pnode == null ? null : _tree[pnode];
            return parent;
        }

        private void InvertOperand(TreeNode node)
        {
            var operand = _tree[node];
            var parent = GetParentByNode(node);
            IOperand oldValue;
            IOperand newValue;
            if (parent is NotOperand _)
            {
                oldValue = parent;
                newValue = operand;
                parent = GetParentByNode(node.Parent);
            }
            else if (operand is NotOperand o)
            {
                oldValue = operand;
                newValue = o.Operand;
            }
            else
            {
                oldValue = operand;
                newValue = operand.Not();
            }
            Update(parent, oldValue, newValue);
            UpdateOperandView();
        }

        private int LinkParent(IFilterData child)
        {
            if (child == null)
                throw new ArgumentNullException(nameof(child));
            if (child.Parent == null)
                return 0;
            int level = LinkParent(child.Parent);
            SetPoint(child, child.Parent, ++level);
            return level;
        }

        private void OnBack(object sender, EventArgs e)
        {
            var newData = Current.parent;
            var newCurrent = (Level: Current.level - 1, Parent:  newData.Parent);
            int position = _filterTree[newCurrent.Level][newCurrent.Parent].IndexOf(newData);
            Current = newCurrent;
            bsFilters.Position = position;
        }

        private void OnBuilt(object sender, EventArgs e)
        {
            Calculate();
            ResultBuilt?.Invoke(this, new EventArgs<IOperand>(Result));
        }

        private void OnCellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex != tbPropertyType.Index || e.RowIndex == -1)
                return;
            if (e.Value is Type t)
            {
                if (bsFilters[e.RowIndex] is IFilterData data && data.IsFromSource)
                    e.Value = "Выбор из списка";
                else if (Helper.IsIntNumber(t))
                    e.Value = "Целочисленный";
                else if (Helper.IsDecNumber(t))
                    e.Value = "Дробный";
                else if (Helper.IsDate(t))
                    e.Value = "Дата";
                else if (Helper.IsString(t))
                    e.Value = "Строка";
                else if (Helper.IsBool(t))
                    e.Value = "Флаг";
                else if (Helper.IsCollection(t))
                    e.Value = "Коллекция";
                else if (Helper.IsGuid(t))
                    e.Value = "Ключ";
                else
                    e.Value = "Пользовательский";
                e.FormattingApplied = true;
            }
        }

        private void OnChangeCurrent((int level, IFilterData parent) current)
        {
            if (current.level != 1)
            {
                btBack.Enabled = btHome.Enabled = true;
                lbPath.Text = current.parent.Print();
            }
            else
            {
                btBack.Enabled = btHome.Enabled = false;
                lbPath.Text = "";
            }
            bsFilters.DataSource = _filterTree[current.level][current.parent];
        }

        private void OnDeleteOperand(object sender, EventArgs e)
        {
            var node = tvOperands.SelectedNode;
            if (node == null || node == tnEmpty)
                return;
            DeleteOperand(node);
        }

        private void OnDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            if (dgvFilters.Rows[e.RowIndex].DataBoundItem is IFilterData newParent)
            {
                int newLevel = Current.level + 1;
                if (newParent.IsFromSource)
                    ChengeValue(newParent, newParent.ValidValues);
                else if (Helper.IsPrimitive(newParent.PropertyType))
                    ChengeValue(newParent);
                else if (Helper.IsCollection(newParent.PropertyType))
                    ChengeInnerValue(newParent);
                else if (!TryChangeCurrent(newLevel, newParent))
                    ChengeValue(newParent);
            }
            else
                throw new DataException();
        }

        private void OnHome(object sender, EventArgs e) => Current = (1, _rootData);

        private void OnInvertClick(object sender, EventArgs e)
        {
            var node = tvOperands.SelectedNode;
            if (node == null || node == tnEmpty)
                return;
            InvertOperand(node);
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
                    UpdateOperand(node);
                    break;

                case MouseButtons.Right:
                    tvOperands.SelectedNode = node;
                    break;

                default:
                    break;
            }
        }

        private void OnOperandsKeyUp(object sender, KeyEventArgs e)
        {
            var node = tvOperands.SelectedNode;
            if (node == null || node == tnEmpty)
                return;

            switch (e.KeyCode)
            {
                case Keys.Delete:
                    DeleteOperand(node);
                    break;

                case Keys.Enter:
                    UpdateOperand(node);
                    break;

                default:
                    break;
            }
        }

        private void OnOperationSetting(object sender, EventArgs e)
        {
            var operands = Helper.GetOperands(Operand);
            var opSetting = new OperationBuilderForm(Operand, operands);
            if (opSetting.ShowDialog() == DialogResult.OK)
            {
            }
        }

        private void OnSourceChanged(IList<IFilterData> _source)
        {
            _rootData = GetParent(_source);
            //PopulateTree(_source);
            Current = (1, _rootData);
        }

        private void OnUpdateOperand(object sender, EventArgs e)
        {
            var node = tvOperands.SelectedNode;
            if (node == null || node == tnEmpty)
                return;
            UpdateOperand(node);
        }

        private void PopulateTree(IList<IFilterData> source)
        {
            foreach (var current in source) TreeAdd(current);
        }

        private void TreeAdd(IFilterData current)
        {
            int level = LinkParent(current.Parent);
            SetPoint(current, current.Parent, level + 1);
        }

        private void SetPoint(IFilterData current, IFilterData parent, int level)
        {
            if (!current.IsDisplayed) return;

            var children = _filterTree.GetOrAdd(level, NodeFactory).GetOrAdd(parent, PointFactory);
            if(!children.Contains(current)) children.Add(current);
        }

        private bool TryChangeCurrent(int level, IFilterData parent)
        {
            if (!TryGetChildren(parent, level, out var children))
                return false;
            Current = (level, parent);
            return true;
        }

        #region Dependency

        private static IList<IFilterData> ActiveSourceFactory() => new List<IFilterData>();

        private static IDictionary<(Type SrcType, string PropertyName), string> DisplaySourceFactory() => new Dictionary<(Type SrcType, string PropertyName), string>();

        private static IDictionary<IFilterData, IList<IFilterData>> NodeFactory() => new Dictionary<IFilterData, IList<IFilterData>>();

        private static IList<IFilterData> PointFactory() => new List<IFilterData>();
        private static IList<IDictionary<IFilterData, IList<IFilterData>>> TreeFactory() => new List<IDictionary<IFilterData, IList<IFilterData>>>();
        private static IDictionary<(Type SrcType, string PropertyName), (string DisplayMember, string ValueMember, IEnumerable<object> ValidValues)> ValidValuesDictionaryFactory() => new Dictionary<(Type SrcType, string PropertyName), (string DisplayMember, string ValueMember, IEnumerable<object> ValidValues)>();
        #endregion Dependency
        private bool TryGetChildren(IFilterData parent, int level, out IList<IFilterData> children)
        {
            children = _filterTree.GetOrAdd(level, NodeFactory).GetOrAdd(parent, PointFactory);
            if (children.Count == 0) return TryPopulateChilred(parent);
            return true;
        }

        private bool TryGetParentByNode(ref TreeNode node, out IOperand parent)
        {
            if ((node = node.Parent) == null)
            {
                parent = null;
                return false;
            }

            parent = GetOperandByNode(node);
            return true;
        }

        private bool TryPopulateChilred(IFilterData current)
        {
            var created = current.CreateChildren();
            if (!created.Any()) return false;
            foreach (var child in created) SourceAdd(child);

            return true;
        }

        private void Update(IOperand parent, IOperand oldValue, IOperand newValue)
        {
            switch (parent)
            {
                case ICollectionOperand p:
                    p.InnerOperand = newValue;
                    UpdateOperandView();
                    break;

                case IUnoOperand p:
                    p.Operand = newValue;
                    UpdateOperandView();
                    break;

                case IBinaryOperand p:
                    if (p.Left == oldValue)
                        p.Left = newValue;
                    else if (p.Right == oldValue)
                        p.Right = newValue;
                    else
                        throw new ArgumentException(nameof(oldValue));
                    UpdateOperandView();
                    break;

                case null:
                    Operand = Operand == oldValue ? newValue : throw new ArgumentException(nameof(oldValue));
                    break;

                default:
                    throw new NotSupportedException(Parent.GetType().Name);
            }
        }

        private void UpdateOperand(TreeNode node)
        {
            var operand = _tree[node];
            var parent = GetParentByNode(node);

            DialogResult dialogResult = default(DialogResult);
            IOperand result = default(IOperand);
            switch (operand)
            {
                case IUnoOperand u:
                case IBinaryOperand b:
                    var operands = Helper.GetOperands(Operand);
                    var operationBuilder = new OperationBuilderForm(operand, operands);
                    dialogResult = operationBuilder.ShowDialog();
                    result = operationBuilder.Result;
                    break;

                case IParameterizedFilterOperand o:
                    _activeSource = Helper.GetOperands(Operand).OfType<IParameterizedFilterOperand>().Select(op => op.Data).Distinct().ToList();
                    var operandBuilder = new OperandBuilderForm(o, _activeSource, ValidValuesDictionary);
                    dialogResult = operandBuilder.ShowDialog();
                    result = operandBuilder.Result;
                    break;

                case ICollectionOperand o:
                    var innerOperand = new InnerOperandBuilderForm(o, ValidValuesDictionary, DisplaySource);
                    dialogResult = innerOperand.ShowDialog();
                    result = innerOperand.Result;
                    break;

                default:
                    throw new NotSupportedException();
            }
            if (dialogResult == DialogResult.OK)
            {
                Update(parent, operand, result);
            }
        }

        private void UpdateOperandView()
        {
            string text = tbPrint.Text = _operand?.Print() ?? "";
            toolTip.SetToolTip(tbPrint, text);
            UpdateTree();
        }

        private void UpdateTree()
        {
            tvOperands.Nodes.Clear();
            _tree = new Dictionary<TreeNode, IOperand>();

            if (Operand == null)
            {
                tvOperands.Nodes.Add(tnEmpty);

                return;
            }

            var node = tvOperands.Nodes.Add(Operand.Name);
            node.ContextMenuStrip = cmsNodeEdit;
            _tree.Add(node, Operand);
            Helper.PopulateTreeByOperand(Operand, node, _tree, cmsNodeEdit);
            tvOperands.ExpandAll();
        }
    }
}