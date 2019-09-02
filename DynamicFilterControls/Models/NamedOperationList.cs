using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicFilterControls.Models
{
    public static class NamedOperationList
    {
        public static IList<NamedOperation> NamedOperations { get; } = GenerateNamedOperands(
            (FilterType.Equals, "Равно", OperandType.Value, ValueType.Equalable), 
            (FilterType.And, "И", OperandType.Binary, ValueType.None), 
            (FilterType.Or, "Или", OperandType.Binary, ValueType.None), 
            (FilterType.Not, "Не", OperandType.Uno, ValueType.None), 
            (FilterType.Like, "Содержит", OperandType.Value, ValueType.Container), 
            (FilterType.More, "Больше", OperandType.Value, ValueType.Comparable), 
            (FilterType.Less, "Меньше", OperandType.Value, ValueType.Comparable), 
            (FilterType.Any, "Какой-либо", OperandType.Collection, ValueType.None), 
            (FilterType.All, "Все", OperandType.Collection, ValueType.None));

        private static IList<NamedOperation> GenerateNamedOperands(params (FilterType Type, string DisplayName, OperandType OperandType, ValueType ValueType)[] names)
        {
            IEnumerable<NamedOperation> GenerateNamedOperandsEnum()
            {
                foreach (var name in names) yield return new NamedOperation(name.Type, name.DisplayName, name.OperandType, name.ValueType);
            }
            return GenerateNamedOperandsEnum().ToList();
        }
    }

    public enum FilterType
    {
        Equals, And, Or, Not, Like, More, Less,
        All,
        Any
    }
    public enum OperandType
    {
        Value, Uno, Binary,
        Collection
    }
    [Flags]
    public enum ValueType
    {
        None, Equalable, Container, Comparable = 4,
    }
}
