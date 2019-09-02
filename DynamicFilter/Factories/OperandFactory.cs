using DynamicFilter.Abstract;
using DynamicFilter.Abstract.Filters;
using DynamicFilter.Abstract.Sort;
using DynamicFilter.Operands.Grouped;
using DynamicFilter.Operands.Parametrized;
using DynamicFilter.Operands.Sorted;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicFilter.Factories
{
    public static class OperandFactory
    {
        public static CheckedOperand AndCheck(this IParameterizedFilterOperand operand, IOperand check) => new CheckedOperand(check, operand);
        public static AndOperand AndAlso(this IOperand left, IOperand right) => new AndOperand(left, right);
        public static NotOperand Not(this IOperand operand) => new NotOperand(operand);
        public static OrOperand OrElse(this IOperand left, IOperand right) => new OrOperand(left, right);


        public static EqualFilter EqualsFilter(IFilterData data) => new EqualFilter(data);
        public static EqualFilter EqualsFilter(IFilterData data, object value) => new EqualFilter(data) { Value = value };
        public static InnerEqualFilter InnerEqualsFilter(IFilterData data, IFilterData innerProperty, object value) => new InnerEqualFilter(data) { Value = value, InnerProperty = innerProperty };
        public static InnerEqualFilter InnerEqualsFilter(IFilterData data, IFilterData innerProperty, object value, string display) => new InnerEqualFilter(data) { Value = value, InnerProperty = innerProperty, DisplayValue = display };
        public static EqualFilter EqualsFilter(IFilterData data, object value, string display) => new EqualFilter(data) { Value = value, DisplayValue = display };
        public static LikeFilter LikeFilter(IFilterData data) => new LikeFilter(data);
        public static LikeFilter LikeFilter(IFilterData data, object value) => new LikeFilter(data) { Value = value };
        public static LikeStringFilter LikeStringFilter(IFilterData data) => new LikeStringFilter(data);
        public static LikeStringFilter LikeStringFilter(IFilterData data, object value) => new LikeStringFilter(data) { Value = value };
        public static MoreFilter MoreFilter(IFilterData data) => new MoreFilter(data);
        public static MoreFilter MoreFilter(IFilterData data, object value) => new MoreFilter(data) { Value = value };
        public static LessFilter LessFilter(IFilterData data) => new LessFilter(data);
        public static LessFilter LessFilter(IFilterData data, object value) => new LessFilter(data) { Value = value };


        public static ISortDataOperand SortOperand(IFilterData data, SortingDirection direction) => new SortOperand(direction, data);

        public static IMultiSortOperand MultiSortOperand(ISortDataOperand operand) => new MultiSortOperand(operand);


        public static IOperand CreateFullFilter(IEnumerable<IFilterData> fastFilterData, string value)
        {
            IOperand operand = LikeStringFilter(fastFilterData.First(), value);
            foreach (var item in fastFilterData.Skip(1))
            {
                operand = operand.OrElse(LikeStringFilter(item, value));
            }
            return operand;
        }

    }
}
