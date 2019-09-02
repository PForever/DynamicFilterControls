using DynamicFilter.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicFilter.Help
{
    public static class OperandHelper
    {
        public static IEnumerable<IOperand> GetOperands(IOperand operand)
        {
            yield return operand;
            switch (operand)
            {
                case IUnoOperand o:
                    foreach (var item in GetOperands(o.Operand)) yield return item;
                    yield break;
                case ICollectionOperand o:
                    if (o.InnerOperand != null) foreach (var item in GetOperands(o.InnerOperand)) yield return item;
                    yield break;
                case IBinaryOperand o:
                    foreach (var item in GetOperands(o.Left)) yield return item;
                    foreach (var item in GetOperands(o.Right)) yield return item;
                    yield break;
                case IParameterizedOperand o:
                    yield break;
                case null: throw new ArgumentNullException(nameof(operand));
                default: throw new NotSupportedException(operand.GetType().FullName);
            }
        }
    }
}
