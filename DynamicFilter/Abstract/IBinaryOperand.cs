namespace DynamicFilter.Abstract
{
    public interface IBinaryOperand : IOperand
    {
        IOperand Left { get; set; }
        IOperand Right { get; set; }
    }
}
