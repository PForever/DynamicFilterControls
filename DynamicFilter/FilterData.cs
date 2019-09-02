using DynamicFilter.Abstract.Filters;
using DynamicFilter.Help;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DynamicFilter
{
    public class FilterData : IFilterData, IEquatable<IFilterData>
    {
        public FilterData(Type propertyType)
        {
            PropertyType = propertyType ?? throw new ArgumentNullException(nameof(propertyType));
        }

        public FilterData(IFilterData parent, Type propertyType, string propertyName, string displayName) : this(propertyType)
        {
            Parent = parent;
            PropertyName = propertyName;
            DisplayName = displayName;
        }

        public FilterData(IFilterData parent, Type propertyType, string propertyName) : this(propertyType)
        {
            Parent = parent;
            DisplayName = PropertyName = propertyName;
        }

        public IFilterData Parent { get; set; }
        public Type PropertyType { get; set; }
        public string PropertyName { get; set; }
        public string DisplayName { get; set; }
        public bool IsSelection { get; set; }
        public (string DisplayMember, string ValueMember, Func<IEnumerable<ComboContainer>> Values) ValidValues
        {
            get
            {
                return (_validValues.DisplayMember, _validValues.ValueMember, _validValues.Values);
            }
            set
            {
                _validValues = (value.DisplayMember, value.ValueMember, value.Values);
            }
        }

        public bool IsDisplayed { get; set; }
        public bool IsFromSource { get; set; }

        private (string DisplayMember, string ValueMember, Func<IEnumerable<ComboContainer>> Values) _validValues;

        public string Print() => $"{(Parent?.PropertyName != null ? $"{Parent.Print()}." : "")}{DisplayName}";

        public override bool Equals(object obj)
        {
            return Equals(obj as FilterData);
        }

        public bool Equals(IFilterData other)
        {
            return other != null &&
                   EqualityComparer<IFilterData>.Default.Equals(Parent, other.Parent) &&
                   EqualityComparer<Type>.Default.Equals(PropertyType, other.PropertyType) &&
                   PropertyName == other.PropertyName;
        }

        public override int GetHashCode()
        {
            var hashCode = -1879778440;
            hashCode = hashCode * -1521134295 + EqualityComparer<IFilterData>.Default.GetHashCode(Parent);
            hashCode = hashCode * -1521134295 + EqualityComparer<Type>.Default.GetHashCode(PropertyType);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(PropertyName);
            return hashCode;
        }

        public override string ToString() => Print();

        public IFilterData Copy()
        {
            return new FilterData(Parent?.Copy(), PropertyType, PropertyName, DisplayName) { IsSelection = IsSelection, ValidValues = ValidValues, IsDisplayed = IsDisplayed, IsFromSource = IsFromSource };
        }

        public IFilterData AsRoot()
        {
            var collection = PropertyType.GetInterfaces().FirstOrDefault(i => i.Name.Contains("IEnumerable") && i.IsGenericType);
            if (collection != null)
            {
                var root = collection.GetGenericArguments().Single();
                return new FilterData(root);
            }
            return new FilterData(PropertyType);
        }
    }

    public class FilterData<T> : FilterData, IFilterData<T>
    {
        public FilterData() : base(typeof(T))
        {
        }
        public FilterData(IFilterData data) : base(data.Parent, data.PropertyType, data.PropertyName, data.DisplayName)
        {
            if (data.GetGrandFather().PropertyType is var type && type != typeof(T))
                throw new ArgumentException($" Unexcept type of parent for data {nameof(data)}. Except type is {typeof(T)} received type is {type}");
            IsDisplayed = data.IsDisplayed;
            IsFromSource = data.IsFromSource;
            IsSelection = data.IsSelection;
            ValidValues = data.ValidValues;
        }
    }
}