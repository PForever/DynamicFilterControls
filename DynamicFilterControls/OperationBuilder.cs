using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DynamicFilterControls.Models;
using DynamicFilter.Operands;
using DynamicFilter.Abstract;
using DynamicFilter.Operands.Grouped;
using DynamicFilter.Factories;

namespace DynamicFilterControls
{
    internal partial class OperationBuilder : UserControl
    {
        #region Props
        public event EventHandler<EventArgs<IOperand>> OperandBuilt;
        private NamedOperation Operation
        {
            get => (NamedOperation)bsOperations.Current;
            set
            {
                var newPosition = bsOperations.Cast<NamedOperation>().TakeWhile(o => o.Type != value.Type).Count();
                if (bsOperations.Position != newPosition) bsOperations.Position = newPosition;
                else OnOperationChanged();
            }
        }
        private IOperand Operand1
        {
            get => ((NamedOperand)bsOperands1.Current).Value;
            set => bsOperands1.Position = bsOperands1.Cast<NamedOperand>().TakeWhile(o => o.Value != value).Count();
        }
        private IOperand Operand2
        {
            get => ((NamedOperand)bsOperands2.Current).Value;
            set => bsOperands2.Position = bsOperands2.Cast<NamedOperand>().TakeWhile(o => o.Value != value).Count();
        }
        private IList<NamedOperation> OperationList = NamedOperationList.NamedOperations.Where(o => o.OperandType == OperandType.Binary || o.OperandType == OperandType.Uno).ToList();
        private IList<NamedOperand> OperandList { get; set; }
        #endregion

        public OperationBuilder()
        {
            InitializeComponent();
            CustomInitialize();
        }
        public OperationBuilder(IEnumerable<IOperand> operands) : this()
        {
            SetOperandList(operands);
            SetOperationList();
        }
        public OperationBuilder(IOperand operand, IEnumerable<IOperand> operands) : this()
        {
            SetOperandList(operands);
            SetOperationList();
            SetOperand(operand);
        }

        private void SetOperand(IOperand operand)
        {
            switch (operand)
            {
                case IBinaryOperand o:
                    SetOperand1(o.Left);
                    SetOperand2(o.Right);
                    ParsBinaryOperation(o);
                    break;
                case IUnoOperand o:
                    SetOperand1(o.Operand);
                    ParsUnoOperation(o);
                    break;
                case null: throw new ArgumentNullException(nameof(operand));
                default: throw new NotSupportedException(operand.GetType().FullName);
            }
        }

        private void ParsBinaryOperation(IBinaryOperand o)
        {
            switch (o)
            {
                case AndOperand a:
                    SetOperation(FilterType.And);
                    break;
                case OrOperand a:
                    SetOperation(FilterType.Or);
                    break;
                default: throw new NotSupportedException(o.GetType().FullName);
            }
        }

        private void ParsUnoOperation(IUnoOperand o)
        {
            switch (o)
            {
                case NotOperand a:
                    SetOperation(FilterType.Not);
                    break;
                default: throw new NotSupportedException(o.GetType().FullName);
            }
        }

        private void SetOperation(FilterType type) => Operation = OperationList.Single(o => o.Type == type);
        private void SetOperand1(IOperand operand) => Operand1 = OperandList.Single(o => o.Value == operand).Value;
        private void SetOperand2(IOperand operand) => Operand2 = OperandList.Single(o => o.Value == operand).Value;
        private void SetOperationList() => bsOperations.DataSource = OperationList;

        private void SetOperandList(IEnumerable<IOperand> operands)
        {
            if (operands == null) throw new ArgumentNullException(nameof(operands));
            OperandList = operands.Select(o => new NamedOperand { DisplayName = /*IncrementName()*/o.Print(), Value = o }).ToList();
            if (OperandList.Count == 0) throw new ArgumentException(nameof(operands));
            bsOperands1.DataSource = 
            bsOperands2.DataSource = OperandList;
        }
        //private int _nameCounter;
        //private string IncrementName() => $"A{_nameCounter++}";

        void CalculateResult()
        {
            switch (Operation.Type)
            {
                case FilterType.And:
                    Result = OperandFactory.AndAlso(Operand1.Copy(), Operand2.Copy());
                    break;
                case FilterType.Or:
                    Result = OperandFactory.OrElse(Operand1.Copy(), Operand2.Copy());
                    break;
                case FilterType.Not:
                    Result = OperandFactory.Not(Operand1.Copy());
                    break;
                default: throw new NotSupportedException();
            }
        }

        public IOperand Result { get; private set; }
        private void CustomInitialize()
        {
            bsOperands1.DataSource = typeof(NamedOperand);
            bsOperands2.DataSource = typeof(NamedOperand);

            cbOperations.DisplayMember = nameof(NamedOperation.DisplayName);
            cbOperations.ValueMember = nameof(NamedOperation.Type);

            bsOperations.DataSource = typeof(NamedOperation);

            cbOperand1.DisplayMember = nameof(NamedOperand.DisplayName);
            cbOperand1.ValueMember = nameof(NamedOperand.Value);
            cbOperand2.DisplayMember = nameof(NamedOperand.DisplayName);
            cbOperand2.ValueMember = nameof(NamedOperand.Value);
        }

        private void OnOperationChanged(object sender, EventArgs e) => OnOperationChanged();
        private void OnOperationChanged()
        {
            if (bsOperations.Current is NamedOperation operation)
            {
                switch (operation.Type)
                {
                    case FilterType.Not:
                        cbOperand2.Visible = false;
                        break;
                    case FilterType.Or:
                    case FilterType.And:
                        cbOperand2.Visible = true;
                        break;
                    default: break;
                }
            }
            else throw new ArgumentException(nameof(bsOperations.Current));
        }

        private void OnBuilt(object sender, EventArgs e)
        {
            CalculateResult();
            OperandBuilt?.Invoke(this, new EventArgs<IOperand>(Result));
        }
    }
}
