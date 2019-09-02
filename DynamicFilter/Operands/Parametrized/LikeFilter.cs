using DynamicFilter.Abstract;
using DynamicFilter.Abstract.Filters;
using DynamicFilter.Factories;
using System;
using System.Linq.Expressions;

namespace DynamicFilter.Operands.Parametrized
{
    public class LikeFilter : IParameterizedFilterOperand
    {
        public object Value { get; set; }
        public IFilterData Data { get; set; }
        public string DisplayValue { get; set; }

        public string Name => $"{Data.Print()} ⊇ {DisplayValue ?? Value?.ToString() ?? "null"}";

        public LikeFilter(IFilterData data)
        {
            Data = data;
        }

        public LambdaExpression Calculate()
        {
            return FilterBuilder.LikePredicate(Data, Value);
        }
        public string Print() => $"({Name})";
        public IOperand Copy() => new LikeFilter(Data) { Value = Value, DisplayValue = DisplayValue };
    }
}
