using DynamicFilter.Abstract;
using DynamicFilter.Abstract.Filters;
using DynamicFilter.Factories;
using DynamicFilter.Help;
using DynamicFilter.Operands.Grouped;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DynamicFilterControls.Logic
{
    public class DynamicFilterBoxLogic : DynamicBoxLogicBase
    {
        #region Fields
        protected Func<IOperand, IEnumerable<IOperand>, IOperand> OperationBuilderFormOrDefault { get; }
        protected Func<IParameterizedFilterOperand, IEnumerable<IFilterData>, IDictionary<(Type SrcType, string PropertyName), (string DisplayMember, string ValueMember, Func<IEnumerable<ComboContainer>> ValidValues)>, IOperand> OperandBuilderFormOrDefault { get; }
        protected Func<ICollectionOperand, IDictionary<(Type SrcType, string PropertyName), (string DisplayMember, string ValueMember, Func<IEnumerable<ComboContainer>> ValidValues)>, IDictionary<(Type SrcType, string PropertyName), string>, IOperand> CollectionBuilderFormOrDefault { get; }
        protected Action<IOperand> UpdateTree { get; }
        Action<string> PrintUpdate { get; }
        protected Func<IFilterData, IDictionary<(Type SrcType, string PropertyName), (string DisplayMember, string ValueMember, Func<IEnumerable<ComboContainer>> ValidValues)>, IDictionary<(Type SrcType, string PropertyName), string>, IOperand> InnerOperandFormOrDefault { get; }
        protected Func<IFilterData, IEnumerable<IFilterData>, IDictionary<(Type SrcType, string PropertyName), (string DisplayMember, string ValueMember, Func<IEnumerable<ComboContainer>> ValidValues)>, IOperand> OperandBuilderDataFormOrDefault { get; }

        private static IList<IFilterData> _activeSource = ActiveSourceFactory();
        protected IOperand _operand;
        private IDictionary<(Type SrcType, string PropertyName), (string DisplayMember, string ValueMember, Func<IEnumerable<ComboContainer>> ValidValues)> _validValuesDictionary = ValidValuesDictionaryFactory();
        #endregion
        #region Constructors
        
        public DynamicFilterBoxLogic(
            Type type,
            IEnumerable<(Type SrcType, string PropertyName, string DisplayMember, string ValueMember, Func<IEnumerable<ComboContainer>> ValidValues)> validValues,
            IDictionary<(Type Type, string PropertyName), string> displaySource,
            Func<IFilterData, IDictionary<(Type SrcType, string PropertyName), (string DisplayMember, string ValueMember, Func<IEnumerable<ComboContainer>> ValidValues)>, IDictionary<(Type SrcType, string PropertyName), string>, IOperand> innerOperandFormOrDefault,
            Func<IFilterData, IEnumerable<IFilterData>, IDictionary<(Type SrcType, string PropertyName), (string DisplayMember, string ValueMember, Func<IEnumerable<ComboContainer>> ValidValues)>, IOperand> operandBuilderDataFormOrDefault,
            Action<int> changeFilterPosition,
            Action<bool> changeEnambeBackBattons,
            Action<string> pathUpdate,
            Action<string> printUpdate,
            Action<IList<IFilterData>> filtersChange,
            Func<IOperand, IEnumerable<IOperand>, IOperand> operationBuilderFormOrDefault,
            Func<IParameterizedFilterOperand, IEnumerable<IFilterData>, IDictionary<(Type SrcType, string PropertyName), (string DisplayMember, string ValueMember, Func<IEnumerable<ComboContainer>> ValidValues)>, IOperand> operandBuilderFormOrDefault,
            Func<ICollectionOperand, IDictionary<(Type SrcType, string PropertyName), (string DisplayMember, string ValueMember, Func<IEnumerable<ComboContainer>> ValidValues)>, IDictionary<(Type SrcType, string PropertyName), string>, IOperand> collectionBuilderFormOrDefault,
            Action<IOperand> updateTree) : base(type, displaySource, changeFilterPosition, changeEnambeBackBattons, pathUpdate, filtersChange)
        {
            InnerOperandFormOrDefault = innerOperandFormOrDefault ?? throw new ArgumentNullException(nameof(innerOperandFormOrDefault));
            OperandBuilderDataFormOrDefault = operandBuilderDataFormOrDefault ?? throw new ArgumentNullException(nameof(operandBuilderDataFormOrDefault));
            UpdateTree = updateTree ?? throw new ArgumentNullException(nameof(updateTree));
            PrintUpdate = printUpdate ?? throw new ArgumentNullException(nameof(printUpdate));
            OperationBuilderFormOrDefault = operationBuilderFormOrDefault ?? throw new ArgumentNullException(nameof(operationBuilderFormOrDefault));
            OperandBuilderFormOrDefault = operandBuilderFormOrDefault ?? throw new ArgumentNullException(nameof(operandBuilderFormOrDefault));
            CollectionBuilderFormOrDefault = collectionBuilderFormOrDefault ?? throw new ArgumentNullException(nameof(collectionBuilderFormOrDefault));
            if (validValues != null) ValidValuesAddRange(validValues);
        }

        #endregion Constructors

        #region Property

        public IDictionary<(Type SrcType, string PropertyName), (string DisplayMember, string ValueMember, Func<IEnumerable<ComboContainer>> ValidValues)> ValidValuesDictionary
        {
            get => _validValuesDictionary; set
            {
                _validValuesDictionary = value;
                UpdateSource();
            }
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
            get; protected set;
        }

        public event EventHandler<EventArgs<IOperand>> ResultBuilt;

        #endregion

        public override void SourceAdd(IFilterData item)
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
            base.SourceAdd(item);
        }

        public void ValidValuesAddRange(IEnumerable<(Type SrcType, string PropertyName, string DisplayMember, string ValueMember, Func<IEnumerable<ComboContainer>> ValidValues)> validValues)
        {
            foreach (var (SrcType, PropertyName, DisplayMember, ValueMember, ValidValues) in validValues)
            {
                //var selector = Helper.ContainerSelector(DisplayMember, ValueMember);

                _validValuesDictionary.Add((SrcType: SrcType, PropertyName: PropertyName), (DisplayMember, ValueMember, ValidValues/*.Select(selector)*/));
            }
            UpdateSource();
        }

        public void AddChechOperand(IFilterData newCurrent)
        {
            var operand = OperandFactory.EqualsFilter(newCurrent, null).Not();
            Operand = Operand == null ? Operand = operand : Operand.AndAlso(operand);
        }

        private void ChengeInnerValue(IFilterData newCurrent)
        {
            var operand = InnerOperandFormOrDefault(newCurrent, ValidValuesDictionary, DisplaySource);
            if (operand == null)
                return;
            Operand = Operand == null ? Operand = operand : Operand.AndAlso(operand);
        }

        public override void OpenFilter(IFilterData newParent)
        {
            var type = newParent.PropertyType;
            int newLevel = Current.level + 1;
            if (newParent.IsFromSource)
                ChengeValue(newParent);
            else if (type.IsPrimitive())
                ChengeValue(newParent);
            else if (type.IsCollection() && !type.IsString())
                ChengeInnerValue(newParent);
            else if (!TryChangeCurrent(newLevel, newParent))
                ChengeValue(newParent);
        }
        private void ChengeValue(IFilterData newCurrent)
        {
            if (!_activeSource.Contains(newCurrent))
                _activeSource.Add(newCurrent);
            var operand = OperandBuilderDataFormOrDefault(newCurrent, _activeSource, ValidValuesDictionary);
            if (operand == null) return;
            Operand = Operand == null ? Operand = operand : Operand.AndAlso(operand);
        }

        public void DeleteOperand<T>(T node, Func<T, IOperand> getOperandByNode, Func<T, (bool IsSuccess, T Node, IOperand Father)> tryGetParentByNode)
        {
            IOperand current, father = getOperandByNode(node);
            (bool IsSuccess, T Node, IOperand Father) res;
            do
            {
                current = father;
                res = tryGetParentByNode(node);
                if (!res.IsSuccess)
                {
                    ClearTree();
                    return;
                }
                node = res.Node; father = res.Father;
            } while (!(father is IBinaryOperand));

            Action<IOperand> changeChild;

            res = tryGetParentByNode(node);
            var grandfather = res.Father;
            if (!res.IsSuccess) changeChild = ch => Operand = ch;
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

        public void InvertOperand<T>(T node, Func<T, IOperand> getOperandByNode, Func<T, (bool IsSuccess, T Node, IOperand Father)> tryGetParentByNode)
        {
            var operand = getOperandByNode(node);
            var res = tryGetParentByNode(node);
            var parent = res.Father;
            node = res.Node;
            IOperand oldValue;
            IOperand newValue;
            if (parent is NotOperand _)
            {
                oldValue = parent;
                newValue = operand;
                parent = tryGetParentByNode(node).Father;
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
                    throw new NotSupportedException(parent.GetType().Name);
            }
        }

        public void UpdateOperand(IOperand operand, IOperand parent)
        {
            IOperand result = default;
            switch (operand)
            {
                case IUnoOperand u:
                case IBinaryOperand b:
                    var operands = OperandHelper.GetOperands(Operand);
                    result = OperationBuilderFormOrDefault(operand, operands);
                    break;

                case IParameterizedFilterOperand o:
                    if (IsCustomParametrized(o)) return;

                    _activeSource = OperandHelper.GetOperands(Operand).OfType<IParameterizedFilterOperand>().Where(p => !IsCustomParametrized(p)).Select(op => op.Data).Distinct().ToList();
                    result = OperandBuilderFormOrDefault(o, _activeSource, ValidValuesDictionary);
                    break;

                case ICollectionOperand o:
                    result = CollectionBuilderFormOrDefault(o, ValidValuesDictionary, DisplaySource);
                    break;

                default:
                    throw new NotSupportedException();
            }
            if (result != null)
            {
                Update(parent, operand, result);
            }
        }

        private static bool IsCustomParametrized(IParameterizedFilterOperand o) => o.Data is IFilterData data && !data.IsFromSource && !data.PropertyType.IsPrimitive();

        protected override void ClearTree() => Operand = null;
        protected override void UpdateOperandView()
        {
            string text = Operand?.Print() ?? "";
            PrintUpdate(text);
            UpdateTree(Operand);
        }
        protected override void Calculate() => Result = Operand;
        public override void OnBuilt()
        {
            Calculate();
            ResultBuilt?.Invoke(this, new EventArgs<IOperand>(Result));
        }
        #region Dependency

        private static IDictionary<(Type SrcType, string PropertyName), (string DisplayMember, string ValueMember, Func<IEnumerable<ComboContainer>> ValidValues)> ValidValuesDictionaryFactory() => new Dictionary<(Type SrcType, string PropertyName), (string DisplayMember, string ValueMember, Func<IEnumerable<ComboContainer>> ValidValues)>();
        #endregion Dependency
    }
}
