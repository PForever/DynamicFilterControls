using DynamicFilter.Abstract;
using DynamicFilter.Abstract.Filters;
using DynamicFilter.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DynamicFilter.Operands.Parametrized
{
    public class EqualInListFilter : IParameterizedWithListFilterOperand
    {
        public object Value { get; set; }
        public IFilterData Data { get; set; }

        public string Name => $"{Data.Print()} = {DisplayValue ?? Value?.ToString() ?? "null"}";
        public string DisplayValue { get; set; }
        private IList<object> _validList;
        public IEnumerable<object> ValidValues
        {
            get => _validList.AsEnumerable();
            set => _validList = value?.ToList();
        }
        public string DisplayMember { get; set; }
        public string ValueMember { get; set; }

        public EqualInListFilter(IFilterData data, IEnumerable<object> validList, string displayMember = null, string valueMember = null)
        {
            _validList = validList?.ToList() ?? throw new ArgumentNullException(nameof(validList));
            Data = data ?? throw new ArgumentNullException(nameof(data));
            DisplayMember = displayMember;
            ValueMember = valueMember;
        }

        public LambdaExpression Calculate()
        {
            return FilterBuilder.EqualPredicate(Data, Value);
        }
        public string Print() => $"({Name})";

        public IOperand Copy() => new EqualFilter(Data) { Value = Value };
    }
}
