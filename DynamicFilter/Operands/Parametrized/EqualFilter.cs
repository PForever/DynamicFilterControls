using DynamicFilter.Abstract;
using DynamicFilter.Abstract.Filters;
using DynamicFilter.Factories;
using DynamicFilter.Help;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DynamicFilter.Operands.Parametrized
{
    public class EqualFilter : IParameterizedFilterOperand
    {
        public object Value { get; set; }
        public IFilterData Data { get; set; }

        public string Name => $"{Data.Print()} = {DisplayValue ?? TypeHelper.PrintValue(Value) ?? "null"}";

        public string DisplayValue { get; set; }

        public EqualFilter(IFilterData data)
        {
            Data = data;
        }

        public LambdaExpression Calculate()
        {
            return FilterBuilder.EqualPredicate(Data, Value);
        }
        public string Print() => $"({Name})";

        public IOperand Copy() => new EqualFilter(Data) { Value = Value, DisplayValue = DisplayValue };

        //public string Print(IDictionary<IFilterData, (string DisplayMember, string ValueMember, IEnumerable<object> ValidValues)> validValuesDictionary)
        //{
        //    var (DisplayMember, ValueMember, ValidValues) = validValuesDictionary[Data];
        //    if (String.IsNullOrEmpty(ValueMember)) return Print();
        //    var type = ValidValues.First().GetType();
        //    Delegate predicate = TypeHelper.Select(type, ValueMember);
        //    var valueObject = ValidValues.FirstOrDefault()
        //}
    }
}
