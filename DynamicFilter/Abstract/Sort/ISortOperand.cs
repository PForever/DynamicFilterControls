using DynamicFilter.Abstract.Filters;
using System.Linq.Expressions;

namespace DynamicFilter.Abstract.Sort
{
    public interface ISortDataOperand
    {
        IFilterData Data { get; set; }
        string Name { get; }
        (LambdaExpression, SortingDirection) Calculate();
        string Print();
        ISortDataOperand Copy();
        SortingDirection Direction { get; set; }
    }
}