using DynamicFilter.Abstract;
using System;
using System.Linq.Expressions;
using static DynamicFilter.Help.ExpressionLogicOperations;

namespace DynamicFilter.Operands.Grouped
{
    public class OrOperand : IBinaryOperand
    {
        public OrOperand(IOperand left, IOperand right)
        {
            Left = left ?? throw new ArgumentNullException(nameof(left));
            Right = right ?? throw new ArgumentNullException(nameof(right));
        }

        public IOperand Left { get; set; }
        public IOperand Right { get; set; }

        public string Name => "или";

        public LambdaExpression Calculate()
        {
            var l = Left.Calculate();
            var r = Right.Calculate();
            return OrElse(l, r);
        }

        public IOperand Copy() => new OrOperand(Left.Copy(), Right.Copy());

        public string Print() => $"({Left.Print()} {Name} {Right.Print()})";
    }
}
