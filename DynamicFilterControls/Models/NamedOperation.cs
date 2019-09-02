using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicFilterControls.Models
{
    public class NamedOperation
    {
        public NamedOperation(FilterType type, string displayName, OperandType operandType, ValueType valueType)
        {
            DisplayName = displayName ?? throw new ArgumentNullException(nameof(displayName));
            Type = type;
            OperandType = operandType;
            ValueType = valueType;
        }

        public string DisplayName { get; }
        public FilterType Type { get; }
        public OperandType OperandType { get; }
        public ValueType ValueType { get; set; }
    }
}
