using DynamicFilter.Abstract;
using DynamicFilter.Abstract.Filters;
using DynamicFilter.Factories;
using DynamicFilter.Help;
using DynamicFilter.Operands.Grouped;
using FilterLibrary.SortableBindingList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicFilterControls.Logic
{
    public abstract class DynamicBoxLogicBase
    {
        #region Fields
        protected (int level, IFilterData parent) _current;
        protected IDictionary<(Type SrcType, string PropertyName), string> _displaySource;
        protected IList<IDictionary<IFilterData, IList<IFilterData>>> _filterTree;
        protected Type _root;
        protected IFilterData _rootData;
        protected IList<IFilterData> _source;
        #endregion

        #region Actions
        
        #endregion
        protected Action<int> ChangeFilterPosition { get; }
        protected Action<bool> ChangeEnambeBackBattons { get; }
        protected Action<string> PathUpdate { get; }
        protected Action<IList<IFilterData>> FiltersChange { get; }

        #region Constructors

        public DynamicBoxLogicBase(
            Type type,
            IDictionary<(Type Type, string PropertyName), string> displaySource,Action<int> changeFilterPosition,
            Action<bool> changeEnambeBackBattons,
            Action<string> pathUpdate,
            Action<IList<IFilterData>> filtersChange)
        {
            ChangeFilterPosition = changeFilterPosition ?? throw new ArgumentNullException(nameof(changeFilterPosition));
            ChangeEnambeBackBattons = changeEnambeBackBattons ?? throw new ArgumentNullException(nameof(changeEnambeBackBattons));
            PathUpdate = pathUpdate ?? throw new ArgumentNullException(nameof(pathUpdate));
            FiltersChange = filtersChange ?? throw new ArgumentNullException(nameof(filtersChange));
           
            _displaySource = displaySource;
            _filterTree = TreeFactory();
            Root = type;
        }

        #endregion Constructors

        #region Property
        protected IList<IOperand> AllOperands { get; set; }

        protected int Count => _source.Count;

        public IDictionary<(Type SrcType, string PropertyName), string> DisplaySource
        {
            get => _displaySource; set
            {
                _displaySource = value ?? throw new ArgumentNullException(nameof(DisplaySource));
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
                if (value == null) return;

                _source = new List<IFilterData>();
                foreach (var item in value) SourceAdd(item);
                AllOperands = _source.Select(d => OperandFactory.EqualsFilter(d)).Cast<IOperand>().ToSortableList();
                if (_source.Count == 0) throw new ArgumentException(nameof(value));
                OnSourceChanged(_source);
            }
        }

        protected (int level, IFilterData parent) Current
        {
            get => _current;
            set
            {
                _current = value;
                OnChangeCurrent(_current);
            }
        }

        #endregion

        public virtual void SourceAdd(IFilterData item)
        {
            _source.Add(item);
            TreeAdd(item);
        }

        public void SourceAddRange(IEnumerable<IFilterData> collection)
        {
            foreach (var item in collection) SourceAdd(item);
        }

        internal void DisplaisAddRange(IEnumerable<(Type SrcType, string propertyName, string displayName)> innerDisplayDictionary)
        {
            foreach (var (SrcType, propertyName, displayName) in innerDisplayDictionary)
            {
                _displaySource.AddOrUpdate((SrcType, propertyName), displayName);
            }
            UpdateSource();
        }

        protected abstract void Calculate();

        protected void ClearSource()
        {
            _root = null;
            _source = null;
            _filterTree.Clear();
            _current = default;
        }

        protected abstract void ClearTree();

        protected IFilterData GetParent(IList<IFilterData> source)
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

        protected int LinkParent(IFilterData child)
        {
            if (child == null)
                throw new ArgumentNullException(nameof(child));
            if (child.Parent == null)
                return 0;
            int level = LinkParent(child.Parent);
            SetPoint(child, child.Parent, ++level);
            return level;
        }

        public void OnBack()
        {
            var newData = Current.parent;
            var newCurrent = (Level: Current.level - 1, Parent: newData.Parent);
            int position = _filterTree[newCurrent.Level][newCurrent.Parent].IndexOf(newData);
            Current = newCurrent;
            ChangeFilterPosition(position);
        }

        public abstract void OnBuilt();

        public string OnCellFormatting(IFilterData data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            var t = data.PropertyType;

            if (data.IsFromSource)
                return "Выбор из списка";
            else if (TypeHelper.IsIntNumber(t))
                return "Целочисленный";
            else if (TypeHelper.IsDecNumber(t))
                return "Дробный";
            else if (TypeHelper.IsDate(t))
                return "Дата";
            else if (TypeHelper.IsString(t))
                return "Строка";
            else if (TypeHelper.IsBool(t))
                return "Флаг";
            else if (TypeHelper.IsCollection(t))
                return "Коллекция";
            else if (TypeHelper.IsGuid(t))
                return "Ключ";
            else
                return "Пользовательский";
        }

        protected void OnChangeCurrent((int level, IFilterData parent) current)
        {
            if (current.level != 1)
            {
                ChangeEnambeBackBattons(true);
                PathUpdate(current.parent.Print());
            }
            else
            {
                ChangeEnambeBackBattons(false);
                PathUpdate("");
            }
            FiltersChange(_filterTree[current.level][current.parent]);
        }

        public abstract void OpenFilter(IFilterData newParent);

        public void OnHome() => Current = (1, _rootData);

        public void OnOpenedFilterEdit(int position)
        {
            ChangeFilterPosition(position);
        }

        protected void OnSourceChanged(IList<IFilterData> _source)
        {
            _rootData = GetParent(_source);
            _filterTree.GetOrAdd(1, NodeFactory).GetOrAdd(_rootData, PointFactory);
            //PopulateTree(_source);
            Current = (1, _rootData);
        }

        protected void PopulateTree(IList<IFilterData> source)
        {
            foreach (var current in source) TreeAdd(current);
        }

        protected void SetPoint(IFilterData current, IFilterData parent, int level)
        {
            if (!current.IsDisplayed) return;

            var children = _filterTree.GetOrAdd(level, NodeFactory).GetOrAdd(parent, PointFactory);
            if (!children.Contains(current)) children.Add(current);
        }

        protected void TreeAdd(IFilterData current)
        {
            int level = LinkParent(current.Parent);
            SetPoint(current, current.Parent, level + 1);
        }

        protected bool TryChangeCurrent(int level, IFilterData parent)
        {
            if (!TryGetChildren(parent, level, out var children))
                return false;
            Current = (level, parent);
            return true;
        }

        protected bool TryGetChildren(IFilterData parent, int level, out IList<IFilterData> children)
        {
            children = _filterTree.GetOrAdd(level, NodeFactory).GetOrAdd(parent, PointFactory);
            if (children.Count == 0) return TryPopulateChilred(parent);
            return true;
        }

        protected bool TryPopulateChilred(IFilterData current)
        {
            var created = current.CreateChildren();
            if (!created.Any()) return false;
            foreach (var child in created) SourceAdd(child);

            return true;
        }

        protected abstract void UpdateOperandView();
        

        protected void UpdateSource()
        {
            Source = Source;
        }
        #region Dependency

        protected static IList<IFilterData> ActiveSourceFactory() => new List<IFilterData>();

        protected static IDictionary<IFilterData, IList<IFilterData>> NodeFactory() => new Dictionary<IFilterData, IList<IFilterData>>();

        protected static IList<IFilterData> PointFactory() => new SortableBindingList<IFilterData>();
        protected static IList<IDictionary<IFilterData, IList<IFilterData>>> TreeFactory() => new List<IDictionary<IFilterData, IList<IFilterData>>>();
        #endregion Dependency
    }
}
