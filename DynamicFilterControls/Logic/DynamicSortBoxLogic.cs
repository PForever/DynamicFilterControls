using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynamicFilter.Abstract;
using DynamicFilter.Abstract.Filters;
using DynamicFilter.Abstract.Sort;
using DynamicFilter.Factories;
using DynamicFilter.Help;

namespace DynamicFilterControls.Logic
{
    public class DynamicSortBoxLogic : DynamicBoxLogicBase
    {
        #region Fields
        protected IMultiSortOperand _operand;
        Action<string> PrintUpdater { get; }


        public DynamicSortBoxLogic(
            Type type,
            IDictionary<(Type Type, string PropertyName), string> displaySource,
            Action<int> changeFilterPosition,
            Action<bool> changeEnambeBackBattons,
            Action<string> pathUpdater,
            Action<string> printUpdater,
            Action<IList<IFilterData>> filtersChange,
            Action<bool> setUpEnable,
            Action<bool> setDownEnable,
            Action<IMultiSortOperand> updateTree,
            Func<ISortDataOperand> selected) : base(type, displaySource, changeFilterPosition, changeEnambeBackBattons, pathUpdater, filtersChange)
        {
            SetUpEnable = setUpEnable ?? throw new ArgumentNullException(nameof(setUpEnable));
            SetDownEnable = setDownEnable ?? throw new ArgumentNullException(nameof(setDownEnable));
            UpdateTree = updateTree ?? throw new ArgumentNullException(nameof(updateTree));
            PrintUpdater = printUpdater ?? throw new ArgumentNullException(nameof(printUpdater));
            Selected = selected ?? throw new ArgumentNullException(nameof(selected));
        }

        #endregion

        #region Properties
        private Action<bool> SetUpEnable { get; }
        private Action<bool> SetDownEnable { get; }
        private Action<IMultiSortOperand> UpdateTree { get; }
        private Func<ISortDataOperand> Selected { get; }

        public void SetOperand(IMultiSortOperand operand)
        {
            _operand = operand;
            UpdateOperandView();
        }

        public IMultiSortOperand Operand
        {
            get => _operand;
            set
            {
                _operand = value;
                UpdateOperandView();
            }
        }

        public IMultiSortOperand Result
        {
            get; private set;
        }

        public event EventHandler<EventArgs<IMultiSortOperand>> ResultBuilt;

        #endregion

        public override void SourceAdd(IFilterData item)
        {
            var typeProperty = item.PropertyType;
            if (typeProperty.IsCollection() && !typeProperty.IsString()) return;

            var type = TypeHelper.GetTypeOfParent(item);
            var key = (type, item.PropertyName);
            if (!item.IsDisplayed && DisplaySource.ContainsKey(key))
            {
                item.DisplayName = DisplaySource[key];
                item.IsDisplayed = true;
            }
            base.SourceAdd(item);
        }

        public void CreateOperand(IFilterData newCurrent)
        {
            var operand = OperandFactory.SortOperand(newCurrent, SortingDirection.Asc);
            AddOperand(operand);
        }

        private void AddOperand(ISortDataOperand operand) => Operand = Operand == null ? OperandFactory.MultiSortOperand(operand) : Operand.Add(operand);

        protected override void ClearTree() => Operand = null;

        public void DeleteOperand()
        {
            var selected = Selected();
            if (selected == null) return;
            if (Operand.Count == 1) Operand = null;
            else Operand.Remove(selected);
            UpdateOperandView();
        }

        //private ISortDataOperand GetOperandByNode(TreeNode node) => _tree[node];

        public override void OnBuilt()
        {
            Calculate();
            ResultBuilt?.Invoke(this, new EventArgs<IMultiSortOperand>(Result));
        }

        public override void OpenFilter(IFilterData newParent)
        {
            int newLevel = Current.level + 1;
            if (newParent.PropertyType.IsPrimitive())
                CreateOperand(newParent);
            else if (newParent.PropertyType.IsCollection())
                throw new NotSupportedException("Collection property not supported");
            else if (!TryChangeCurrent(newLevel, newParent))
                CreateOperand(newParent);
        }

        public void InvertSort()
        {
            var selected = Selected();
            if (selected == null) return;

            switch (selected.Direction)
            {
                case SortingDirection.Asc:
                    selected.Direction = SortingDirection.Desc;
                    break;
                case SortingDirection.Desc:
                    selected.Direction = SortingDirection.Asc;
                    break;
                default: throw new NotSupportedException(selected.Direction.ToString());
            }
            UpdateOperandView();
        }

        protected override void UpdateOperandView()
        {
            string text = Operand?.Print() ?? "";
            PrintUpdater(text);
            UpdateTree(Operand);
            ChechSelected();
        }

        public void OnUpFilter()
        {
            var selected = Selected();
            if (selected == null) return;
            var i = Operand.IndexOf(selected);
            Operand.Change(i, i - 1);
            UpdateOperandView();
        }

        public void OnDownFilter()
        {
            var selected = Selected();
            if (selected == null) return;
            var i = Operand.IndexOf(selected);
            Operand.Change(i, i + 1);
            UpdateOperandView();
        }

        public void ChechSelected()
        {
            var selected = Selected();
            if (selected != null)
            {
                var i = Operand.IndexOf(selected);
                SetUpEnable(i > 0);
                SetDownEnable(i < Operand.Count - 1);
            }
            else
            {
                SetUpEnable(false);
                SetDownEnable(false);
            }
        }
        protected override void Calculate() => Result = Operand;
    }
}
