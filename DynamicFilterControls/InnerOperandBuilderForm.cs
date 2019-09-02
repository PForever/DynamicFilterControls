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
    internal partial class InnerOperandBuilderForm : Form
    {
        public IOperand Result;

        public InnerOperandBuilderForm()
        {
            InitializeComponent();
        }
        public InnerOperandBuilderForm(IFilterData data, IDictionary<(Type SrcType, string PropertyName), (string DisplayMember, string ValueMember, Func<IEnumerable<ComboContainer>> ValidValues)> validValuesDictionary, IDictionary<(Type SrcType, string PropertyName), string> displaySource) : this()
        {
            innerOperandBuilder.Data = data ?? throw new ArgumentNullException(nameof(data));
            innerOperandBuilder.Built += OnBuilt;
            innerOperandBuilder.ValidValuesDictionary = validValuesDictionary ?? throw new ArgumentNullException(nameof(validValuesDictionary));
            innerOperandBuilder.DisplaySource = displaySource;
        }
        public InnerOperandBuilderForm(ICollectionOperand operand, IDictionary<(Type SrcType, string PropertyName), (string DisplayMember, string ValueMember, Func<IEnumerable<ComboContainer>> ValidValues)> validValuesDictionary, IDictionary<(Type SrcType, string PropertyName), string> displaySource) : this(operand?.Data ?? throw new ArgumentNullException(nameof(operand)), validValuesDictionary, displaySource)
        {
            innerOperandBuilder.Result = operand;
        }
        private void OnBuilt(object sender, EventArgs<IOperand> e)
        {
            Result = e.Value;
            DialogResult = DialogResult.OK;
            Close();
        }

    }
}
