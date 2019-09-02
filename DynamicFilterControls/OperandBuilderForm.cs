using DynamicFilter.Abstract;
using DynamicFilter.Abstract.Filters;
using DynamicFilter.Help;
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
    internal partial class OperandBuilderForm : Form
    {
        public IOperand Result { get; private set; }
        public OperandBuilderForm()
        {
            InitializeComponent();
            operandBuilder.OperandBuilt += OnOperandBuilt;
        }
        public OperandBuilderForm(IFilterData property, IEnumerable<IFilterData> allPropertyies, IDictionary<(Type SrcType, string PropertyName), (string DisplayMember, string ValueMember, Func<IEnumerable<ComboContainer>> ValidValues)> validValuesDictionary) : this()
        {
            operandBuilder.ValidValuesDictionary = validValuesDictionary;
            operandBuilder.Properties = allPropertyies;
            operandBuilder.Property = property;
        }
        public OperandBuilderForm(IParameterizedFilterOperand operand, IEnumerable<IFilterData> allPropertyies, IDictionary<(Type SrcType, string PropertyName), (string DisplayMember, string ValueMember, Func<IEnumerable<ComboContainer>> ValidValues)> validValuesDictionary) : this()
        {
            operandBuilder.ValidValuesDictionary = validValuesDictionary;
            operandBuilder.Properties = allPropertyies;
            operandBuilder.Operand = operand;
        }

        private void OnOperandBuilt(object sender, EventArgs<IOperand> e)
        {
            Result = e.Value;
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
