using DynamicFilter.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static DynamicFilter.Help.ExpressionLogicOperations;

namespace DynamicFilter.Operands.Grouped
{
    public class AndOperand : IBinaryOperand
    {
        public AndOperand(IOperand left, IOperand right)
        {
            Left = left ?? throw new ArgumentNullException(nameof(left));
            Right = right ?? throw new ArgumentNullException(nameof(right));
        }

        public IOperand Left { get; set; }
        public IOperand Right { get; set; }

        public string Name => "и";

        public LambdaExpression Calculate()
        {
            var l = Left.Calculate();
            var r = Right.Calculate();
            return AndAlso(l, r);
        }
        public IOperand Copy() => new AndOperand(Left.Copy(), Right.Copy());
        public string Print() => $"({Left.Print()} {Name} {Right.Print()})";

        //public string Print(IDictionary<IFilterData, (string DisplayMember, string ValueMember, IEnumerable<object> ValidValues)> validValuesDictionary)
        //{
        //    return  $"({Left.Print(validValuesDictionary)} {Name} {Right.Print(validValuesDictionary)})";
        //}
    }
}
