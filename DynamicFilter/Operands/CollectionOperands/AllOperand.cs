using DynamicFilter.Abstract;
using DynamicFilter.Abstract.Filters;
using DynamicFilter.Factories;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DynamicFilter.Operands.CollectionOperands
{
    public class AllOperand : ICollectionOperand
    {
        public AllOperand(IFilterData data, IOperand innerOperand)
        {
            Data = data ?? throw new ArgumentNullException(nameof(data));
            InnerOperand = innerOperand ?? throw new ArgumentNullException(nameof(innerOperand));
        }

        public IOperand InnerOperand { get; set; }

        public string Name => $"Все {Data.Print()}";

        public IFilterData Data { get; set; }

        public LambdaExpression Calculate() => FilterBuilder.AllPredicate(Data, InnerOperand.Calculate());

        public IOperand Copy() => new AllOperand(Data, InnerOperand.Copy());

        public string Print() => $"({Name} {InnerOperand.Print()})";

        //public string Print(IDictionary<IFilterData, (string DisplayMember, string ValueMember, IEnumerable<object> ValidValues)> validValuesDictionary)
        //{
        //    return $"({Name} {InnerOperand.Print(validValuesDictionary)})";
        //}
    }
}
