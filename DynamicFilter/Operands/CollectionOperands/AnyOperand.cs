using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DynamicFilter.Abstract;
using DynamicFilter.Factories;
using DynamicFilter.Abstract.Filters;

namespace DynamicFilter.Operands.CollectionOperands
{
    public class AnyOperand : ICollectionOperand
    {
        public AnyOperand(IFilterData data, IOperand innerOperand)
        {
            Data = data ?? throw new ArgumentNullException(nameof(data));
            InnerOperand = innerOperand;
        }

        public IOperand InnerOperand { get; set; }

        public string Name => $"Какой-либо {Data.Print()}";

        public IFilterData Data { get; set; }

        public LambdaExpression Calculate() => InnerOperand != null ? FilterBuilder.AnyPredicate(Data, InnerOperand.Calculate()) : FilterBuilder.AnyPredicate(Data);

        public IOperand Copy() => new AnyOperand(Data, InnerOperand.Copy());

        public string Print() => $"({Name} {InnerOperand?.Print()})";

        //public string Print(IDictionary<IFilterData, (string DisplayMember, string ValueMember, IEnumerable<object> ValidValues)> validValuesDictionary)
        //    => $"({Name} {InnerOperand?.Print(validValuesDictionary)})";
    }
}
