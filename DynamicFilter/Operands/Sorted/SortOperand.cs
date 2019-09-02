using DynamicFilter.Abstract.Filters;
using DynamicFilter.Abstract.Sort;
using DynamicFilter.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DynamicFilter.Operands.Sorted
{
    public class SortOperand : ISortDataOperand
    {
        public SortOperand(SortingDirection direction, IFilterData data)
        {
            Direction = direction;
            Data = data ?? throw new ArgumentNullException(nameof(data));
        }

        public SortingDirection Direction { get; set; }
        public IFilterData Data { get; set; }

        public string Name => Print();

        public (LambdaExpression, SortingDirection) Calculate() => (FilterBuilder.SelectorDelegate(Data), Direction);
        public ISortDataOperand Copy() => new SortOperand(Direction, Data);
        public string Print() => $"{Data.Print()} {Show(Direction)}";
        private string Show(SortingDirection direction)
        {
            switch (direction)
            {
                case SortingDirection.None:
                    return "";
                case SortingDirection.Asc:
                    return "↑";
                case SortingDirection.Desc:
                    return "↓";
                default:
                    throw new NotSupportedException();
            }
        }
    }
}
