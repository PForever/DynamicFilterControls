using System.Collections;
using System.Collections.Generic;

namespace DynamicFilter.Abstract.Filters
{
    public interface IParameterizedFilterOperand : IParameterizedOperand, IFilterOperand
    {
    }
    public interface IParameterizedWithListFilterOperand : IParameterizedOperand, IFilterOperand
    {
        IEnumerable<object> ValidValues { get; set; }
        string DisplayMember { get; set; }
        string ValueMember { get; set; }
    }
}
