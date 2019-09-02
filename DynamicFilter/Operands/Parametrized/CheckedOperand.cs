using DynamicFilter.Abstract;
using DynamicFilter.Abstract.Filters;
using DynamicFilter.Factories;
using DynamicFilter.Help;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DynamicFilter.Operands.Parametrized
{
    public class CheckedOperand : IParameterizedFilterOperand
    {
        public CheckedOperand(IOperand check, IParameterizedFilterOperand operand)
        {
            Check = check ?? throw new ArgumentNullException(nameof(check));
            Operand = operand ?? throw new ArgumentNullException(nameof(operand));
        }

        public IOperand Check { get; set; }
        public IParameterizedFilterOperand Operand { get; set; }
        public object Value { get => Operand.Value; set => Operand.Value = value; }
        public IFilterData Data { get => Operand.Data; set => Operand.Data = value; }

        public string Name => "и";

        public string DisplayValue { get => Operand.DisplayValue; set => Operand.DisplayValue = value; }

        public LambdaExpression Calculate()
        {
            var l = Check.Calculate();
            var r = Operand.Calculate();
            return ExpressionLogicOperations.AndAlso(l, r);
        }
        IOperand IOperand.Copy() => new CheckedOperand(Check.Copy(), Operand.Copy() as IParameterizedFilterOperand);
        public string Print() => $"({Check.Print()} {Name} {Operand.Print()})";

        //public string Print(IDictionary<IFilterData, (string DisplayMember, string ValueMember, IEnumerable<object> ValidValues)> validValuesDictionary)
        //    => $"({Check.Print()} {Name} {Operand.Print(validValuesDictionary)})";
    }
}
