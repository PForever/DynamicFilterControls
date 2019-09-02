using DynamicFilter.Abstract;
using DynamicFilter.Abstract.Filters;
using DynamicFilter.Help;
using DynamicFilter.Operands.CollectionOperands;
using DynamicFilterControls.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicFilterControls.Logic
{
    public class InnerOperandBuilderLogic
    {
        //MessageBox.Show("Для данной операции внутренний фильтр не может быть пустым");
        private Action<string> OperationErrorHandler { get; }
        private Action<string> PrintUpdate { get; }
        //var innerCreator = new DynamicFilterForm(Data.AsRoot().PropertyType, InnerOperand, ValidValuesDictionary.Select(kvp => (kvp.Key.SrcType, kvp.Key.PropertyName, kvp.Value.DisplayMember, kvp.Value.ValueMember, kvp.Value.ValidValues)), DisplaySource.Select(d => (d.Key.SrcType, d.Key.PropertyName, d.Value)));
        private Func<Type, IOperand, IEnumerable<(Type SrcType, string PropertyName, string DisplayMember, string ValueMember, Func<IEnumerable<ComboContainer>> ValidValues)>, IEnumerable<(Type SrcType, string propertyName, string displayName)>,IOperand> DynamicFilterFormOrDefault { get; }
        public IDictionary<(Type SrcType, string PropertyName), (string DisplayMember, string ValueMember, Func<IEnumerable<ComboContainer>> ValidValues)> ValidValuesDictionary { get; set; }
        public IDictionary<(Type SrcType, string PropertyName), string> DisplaySource { get; set; }
        public IList<NamedOperation> OperationList = NamedOperationList.NamedOperations.Where(o => o.OperandType == OperandType.Collection).ToList();
        private FilterType Operation { get; set; }
        private ICollectionOperand _result;
        private IOperand _innerOperand;

        public IOperand InnerOperand
        {
            get => _innerOperand;
            private set
            {
                _innerOperand = value;
                PrintUpdate(value?.Print() ?? "");
            }
        }
        public ICollectionOperand Result
        {
            get => _result;
            set
            {
                _result = value;
                if (value != null)
                {
                    InnerOperand = value.InnerOperand;
                    ParsOperation(value);
                }
            }
        }

        private void ParsOperation(ICollectionOperand value)
        {
            switch (value)
            {
                case AnyOperand o:
                    Operation = FilterType.Any;
                    break;
                case AllOperand o:
                    Operation = FilterType.All;
                    break;
                default: throw new NotSupportedException();
            }
        }

        public event EventHandler<EventArgs<IOperand>> Built;
        public IFilterData Data { get; set; }
        public InnerOperandBuilderLogic() { }
        public InnerOperandBuilderLogic(IFilterData data, IDictionary<(Type SrcType, string PropertyName), string> displaySource, IDictionary<(Type SrcType, string PropertyName), (string DisplayMember, string ValueMember, Func<IEnumerable<ComboContainer>> ValidValues)> validValuesDictionary)
        {
            ValidValuesDictionary = validValuesDictionary;
            DisplaySource = displaySource;
            Data = data;
        }
        public InnerOperandBuilderLogic(IFilterData data, IDictionary<(Type SrcType, string PropertyName), string> displaySource, ICollectionOperand operand, IDictionary<(Type SrcType, string PropertyName), (string DisplayMember, string ValueMember, Func<IEnumerable<ComboContainer>> ValidValues)> validValuesDictionary) : this(data, displaySource, validValuesDictionary)
        {
            Result = operand;
        }

        private void Calculate()
        {
            Built?.Invoke(this, new EventArgs<IOperand>(Result));
        }

        public void OnBuild(NamedOperation current)
        {
            if (current == null) throw new ArgumentNullException(nameof(current));
            if (!TryCreateResult(current.Type, InnerOperand, out var result)) return;
            Result = result;
            Calculate();
        }

        public void OnCreate()
        {
            var innerCreator = DynamicFilterFormOrDefault(Data.AsRoot().PropertyType, InnerOperand, ValidValuesDictionary.Select(kvp => (kvp.Key.SrcType, kvp.Key.PropertyName, kvp.Value.DisplayMember, kvp.Value.ValueMember, kvp.Value.ValidValues)), DisplaySource.Select(d => (d.Key.SrcType, d.Key.PropertyName, d.Value)));
            if (innerCreator != null) InnerOperand = innerCreator;
        }

        private bool TryCreateResult(FilterType current, IOperand innerResult, out ICollectionOperand result)
        {
            switch (current)
            {
                case FilterType.All:
                    if (innerResult == null)
                    {
                        OperationErrorHandler("Для данной операции внутренний фильтр не может быть пустым");
                        result = null;
                        return false;
                    }
                    result = new AllOperand(Data, innerResult);
                    return true;
                case FilterType.Any:
                    result = new AnyOperand(Data, innerResult);
                    return true;
                default: throw new NotSupportedException(current.ToString());
            }
        }
    }
}
