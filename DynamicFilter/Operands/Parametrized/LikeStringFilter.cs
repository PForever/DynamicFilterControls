using DynamicFilter.Abstract;
using DynamicFilter.Abstract.Filters;
using DynamicFilter.Factories;
using System.Linq.Expressions;

namespace DynamicFilter.Operands.Parametrized
{
    public class LikeStringFilter : IParameterizedFilterOperand
    {
        public object Value { get => StringValue; set => StringValue = value?.ToString(); }
        public string StringValue { get; set; }
        public IFilterData Data { get; set; }
        public string DisplayValue { get; set; }

        public string Name => $"{Data.Print()} ⊇ {DisplayValue ?? StringValue ?? "null"}";

        public LikeStringFilter(IFilterData data)
        {
            Data = data;
        }

        public LambdaExpression Calculate()
        {
            return FilterBuilder.LikeStringPredicate(Data, StringValue);
        }
        public string Print() => $"({Name})";
        public IOperand Copy() => new LikeStringFilter(Data) { StringValue = StringValue, DisplayValue = DisplayValue };
    }
}
