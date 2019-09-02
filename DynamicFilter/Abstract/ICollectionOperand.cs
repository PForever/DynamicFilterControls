using DynamicFilter.Abstract.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicFilter.Abstract
{
    public interface ICollectionOperand : IOperand, IFilterOperand
    {
        IOperand InnerOperand { get; set; }
    }
}
