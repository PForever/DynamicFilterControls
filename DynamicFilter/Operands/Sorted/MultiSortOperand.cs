using DynamicFilter.Abstract;
using DynamicFilter.Abstract.Sort;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DynamicFilter.Operands.Sorted
{
    public class MultiSortOperand : IMultiSortOperand
    {
        private List<ISortDataOperand> _properties = new List<ISortDataOperand>();

        public MultiSortOperand(ISortDataOperand sortProperties)
        {
            Add(sortProperties);
        }
        public MultiSortOperand(IEnumerable<ISortDataOperand> sortProperties)
        {
            SortProperties = sortProperties ?? throw new ArgumentNullException(nameof(sortProperties));
        }

        public IEnumerable<ISortDataOperand> SortProperties
        {
            get => _properties.AsEnumerable();
            set => _properties = value.ToList();
        }

        public string Name => "Сортировка по";

        public int Count => _properties.Count;

        public IMultiSortOperand Add(ISortDataOperand data)
        {
            _properties.Add(data);
            return this;
        }
        public IMultiSortOperand Insert(ISortDataOperand data, int index)
        {
            _properties.Insert(index, data);
            return this;
        }

        public IEnumerable<(LambdaExpression, SortingDirection)> Calculate()
        {
            foreach (var data in SortProperties) yield return data.Calculate();
        }

        public void Change(ISortDataOperand data, int newIndex)
        {
            int oldIndex = _properties.IndexOf(data);
            _properties[oldIndex] = _properties[newIndex];
            _properties[newIndex] = data;
        }

        public void Change(int oldIndex, int newIndex)
        {
            var temp = _properties[newIndex];
            _properties[newIndex] = _properties[oldIndex];
            _properties[oldIndex] = temp;
        }

        public void Change(ISortDataOperand a, ISortDataOperand b)
        {
            int oldIndex = _properties.IndexOf(a);
            int newIndex = _properties.IndexOf(b);
            _properties[oldIndex] = b;
            _properties[newIndex] = a;
        }

        public IOperand Copy() => throw new NotImplementedException();
        public string Print() => $"{Name} {_properties.Select(p => p.Print()).Aggregate((p, s) => $"{p}, {s}")}";
        public void Remove(ISortDataOperand current) => _properties.Remove(current);
        public int IndexOf(ISortDataOperand operand) => _properties.IndexOf(operand);
    }
}
