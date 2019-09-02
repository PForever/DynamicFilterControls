namespace DynamicFilter.Abstract
{
    public interface IParameterizedOperand : IOperand
    {
        object Value { get; set; }
        string DisplayValue { get; set; }
    }
}
