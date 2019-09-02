namespace DynamicFilter.Abstract
{
    public interface IUnoOperand : IOperand
    {
        IOperand Operand { get; set; }
    }
}
