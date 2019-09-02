using DynamicFilter.Abstract;

namespace DynamicFilterControls.Models
{
    public class NamedOperand
    {
        public string DisplayName { get; set; }
        public IOperand Value { get; set; }
    }
}
