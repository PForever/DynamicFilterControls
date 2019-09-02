using DynamicFilter.Abstract;
using DynamicFilter.Abstract.Filters;
using DynamicFilter.Factories;
using System;
using System.Linq.Expressions;

namespace DynamicFilter.Operands.Parametrized
{
    public class MoreFilter : IParameterizedFilterOperand
    {
        public object Value { get; set; }
        public IFilterData Data { get; set; }

        public string Name => $"{Data.Print()} > {DisplayValue ?? Value?.ToString() ?? "null"}";

        public string DisplayValue { get; set; }
        public MoreFilter(IFilterData data)
        {
            Data = data;
        }

        public LambdaExpression Calculate()
        {
            return FilterBuilder.MorePredicate(Data, Value);
        }
        public string Print() => $"({Name})";
        public IOperand Copy() => new MoreFilter(Data) { Value = Value, DisplayValue = DisplayValue };
    }
}
