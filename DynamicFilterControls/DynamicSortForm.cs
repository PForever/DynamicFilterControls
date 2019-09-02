using DynamicFilter.Abstract.Filters;
using DynamicFilter.Abstract.Sort;
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
    public partial class DynamicSortForm : Form
    {
        public DynamicSortForm()
        {
            InitializeComponent();
            ucSortBox.ResultBuilt += OnResultBuilt;
        }

        public DynamicSortForm(Type type) : this()
        {
            ucSortBox.Root = type;
        }

        public DynamicSortForm(Type type, IMultiSortOperand operand) : this(type)
        {
            ucSortBox.Operand = operand;
        }

        public DynamicSortForm(Type type, IEnumerable<(Type SrcType, string ValueMember, string DisplayMember)> displaySource) : this(type)
        {
            ucSortBox.DisplaisAddRange(displaySource);
        }

        public DynamicSortForm(Type type, IMultiSortOperand operand, IEnumerable<(Type SrcType, string ValueMember, string DisplayMember)> displaySource) : this(type, operand)
        {
            ucSortBox.DisplaisAddRange(displaySource);
        }

        public DynamicSortForm(Type type, DataGridViewColumnCollection columns) : this()
        {
            ucSortBox.SetByColumns(type, columns);
        }

        public DynamicSortForm(Type type, DataGridViewColumnCollection columns, IMultiSortOperand operand) : this()
        {
            ucSortBox.SetByColumns(type, columns);
            ucSortBox.Operand = operand;
        }

        public IMultiSortOperand Result { get; set; }
        private void OnResultBuilt(object sender, EventArgs<IMultiSortOperand> e)
        {
            Result = e.Value;
            DialogResult = DialogResult.OK;
            Close();
        }

        public void DisplaisAddRange(IEnumerable<(Type SrcType, string propertyName, string displayName)> innerDisplayDictionary)
        {
            ucSortBox.DisplaisAddRange(innerDisplayDictionary);
        }

        public void SourceAddRange(IEnumerable<IFilterData> additionalSource)
        {
            ucSortBox.SourceAddRange(additionalSource);
        }
    }
}
