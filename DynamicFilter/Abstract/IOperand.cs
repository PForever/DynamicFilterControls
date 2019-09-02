using System.Linq.Expressions;

namespace DynamicFilter.Abstract
{
    public interface IOperand
    {
        string Name { get; }
        LambdaExpression Calculate();
        string Print();
        IOperand Copy();
    }
}
