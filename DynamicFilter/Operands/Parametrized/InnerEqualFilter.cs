using DynamicFilter.Abstract;
using DynamicFilter.Abstract.Filters;
using DynamicFilter.Factories;
using DynamicFilter.Help;
using System.Linq.Expressions;

namespace DynamicFilter.Operands.Parametrized
{
    public class InnerEqualFilter : IParameterizedFilterOperand
    {
        public object Value { get; set; }
        public IFilterData Data { get; set; }
        public IFilterData InnerProperty { get; set; }

        public string Name => $"{Data.Print()} = {DisplayValue ?? TypeHelper.PrintValue(Value) ?? "null"}";

        public string DisplayValue { get; set; }

        public InnerEqualFilter(IFilterData data)
        {
            Data = data;
        }

        public LambdaExpression Calculate()
        {
            return FilterBuilder.EqualPredicate(InnerProperty, Value);
        }
        public string Print() => $"({Name})";

        public IOperand Copy() => new InnerEqualFilter(Data) { Value = Value, InnerProperty = InnerProperty, DisplayValue = DisplayValue };
    }
}