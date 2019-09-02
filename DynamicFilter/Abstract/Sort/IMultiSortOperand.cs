using System.Collections.Generic;
using System.Linq.Expressions;

namespace DynamicFilter.Abstract.Sort
{
    public interface IMultiSortOperand
    {
        string Name { get; }
        string Print();
        IOperand Copy();
        IEnumerable<ISortDataOperand> SortProperties { get; set; }
        int Count { get; }
        IMultiSortOperand Add(ISortDataOperand data);
        IMultiSortOperand Insert(ISortDataOperand data, int index);
        void Change(ISortDataOperand data, int newIndex);
        void Change(int oldIndex, int newIndex);
        void Change(ISortDataOperand a, ISortDataOperand b);
        IEnumerable<(LambdaExpression, SortingDirection)> Calculate();
        void Remove(ISortDataOperand current);
        int IndexOf(ISortDataOperand operand);
    }
}
