using DynamicFilter.Help;
using System;
using System.Collections.Generic;

namespace DynamicFilter.Abstract.Filters
{
    public interface IFilterData : IEquatable<IFilterData>
    {
        string DisplayName { get; set; }
        IFilterData Parent { get; set; }
        bool IsDisplayed { get; set; }
        bool IsFromSource { get; set; }
        (string DisplayMember, string ValueMember, Func<IEnumerable<ComboContainer>> Values) ValidValues { get; set; }
        string PropertyName { get; set; }
        bool IsSelection { get; set; }
        Type PropertyType { get; set; }
        string Print();
        IFilterData Copy();
        IFilterData AsRoot();
    }
    public interface IFilterData<T> : IFilterData
    {
    }
}