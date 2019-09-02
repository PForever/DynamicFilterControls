using DynamicFilter.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DynamicFilterControls
{
    internal partial class OperationBuilderForm : Form
    {
        public IOperand Result { get; set; }
        public OperationBuilderForm(IOperand operand, IEnumerable<IOperand> operands)
        {
            this.operationBuilder = new OperationBuilder(operand, operands);
            operationBuilder.OperandBuilt += OnOperandBuilt;
            InitializeComponent();
        }

        private void OnOperandBuilt(object sender, EventArgs<IOperand> e)
        {
            Result = e.Value;
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
