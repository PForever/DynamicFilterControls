using DynamicFilter.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DynamicFilter.Operands.Grouped
{
    public class NotOperand : IUnoOperand
    {
        public NotOperand(IOperand operand)
        {
            Operand = operand ?? throw new ArgumentNullException(nameof(operand));
        }

        public IOperand Operand { get; set; }

        public string Name => "не";

        public LambdaExpression Calculate()
        {
            var op = Operand.Calculate();
            var param = op.Parameters;
            var body = op.Body;
            body = Expression.Not(op.Body);
            return Expression.Lambda(body, param);
        }

        public IOperand Copy() => new NotOperand(Operand.Copy());

        public string Print() => $"{Name}{Operand.Print()}";
    }
}
