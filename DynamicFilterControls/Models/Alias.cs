using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynamicFilter.Abstract.Filters;

namespace DynamicFilterControls.Models
{
    class Alias
    {
        public string DisplayName { get; set; }
        public IFilterData FilterData { get; set; }
    }
}
