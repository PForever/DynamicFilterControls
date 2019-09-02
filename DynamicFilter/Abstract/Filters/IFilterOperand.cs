using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DynamicFilter.Abstract.Filters
{
    public interface IFilterOperand : IOperand
    {
        IFilterData Data { get; set; }
    }
}
