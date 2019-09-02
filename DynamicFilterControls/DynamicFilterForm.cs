using DynamicFilter.Abstract;
using DynamicFilter.Abstract.Filters;
using DynamicFilter.Help;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace DynamicFilterControls
{
    public partial class DynamicFilterForm : Form
    {
        public DynamicFilterForm()
        {
            InitializeComponent();
            ucFilterBox.ResultBuilt += OnResultBuilt;
        }

        public DynamicFilterForm(Type type) : this()
        {
            ucFilterBox.Root = type;
        }

        public DynamicFilterForm(Type type, IOperand operand) : this(type)
        {
            ucFilterBox.Operand = operand;
        }

        public DynamicFilterForm(Type type, IEnumerable<(Type SrcType, string PropertyName, string DisplayMember, string ValueMember, Func<IEnumerable<ComboContainer>> ValidValues)> validValuesDictionary) : this(type)
        {
            ucFilterBox.ValidValuesAddRange(validValuesDictionary);
        }

        public DynamicFilterForm(Type type, IOperand operand, IEnumerable<(Type SrcType, string PropertyName, string DisplayMember, string ValueMember, Func<IEnumerable<ComboContainer>> ValidValues)> validValuesDictionary) : this(type, operand)
        {
            ucFilterBox.ValidValuesAddRange(validValuesDictionary);
        }

        public DynamicFilterForm(Type type, IEnumerable<(Type SrcType, string PropertyName, string DisplayMember, string ValueMember, Func<IEnumerable<ComboContainer>> ValidValues)> validValuesDictionary, IEnumerable<(Type SrcType, string propertyName, string displayName)> displaySource) : this(type, validValuesDictionary)
        {
            ucFilterBox.DisplaisAddRange(displaySource);
        }

        public DynamicFilterForm(Type type, IOperand operand, IEnumerable<(Type SrcType, string PropertyName, string DisplayMember, string ValueMember, Func<IEnumerable<ComboContainer>> ValidValues)> validValuesDictionary, IEnumerable<(Type SrcType, string propertyName, string displayName)> displaySource) : this(type, operand, validValuesDictionary)
        {
            ucFilterBox.DisplaisAddRange(displaySource);
        }

        public DynamicFilterForm(Type type, DataGridViewColumnCollection columns) : this()
        {
            ucFilterBox.SetByColumns(type, columns);
        }

        public DynamicFilterForm(Type type, DataGridViewColumnCollection columns, IOperand operand) : this(type, columns)
        {
            ucFilterBox.Operand = operand;
        }

        public DynamicFilterForm(Type type, IEnumerable<(Type SrcType, string propertyName, string displayName)> displaies) : this(type)
        {
            ucFilterBox.DisplaisAddRange(displaies);
        }

        public DynamicFilterForm(Type type, IEnumerable<(Type SrcType, string propertyName, string displayName)> displaies, IOperand operand) : this(type, displaies)
        {
            ucFilterBox.Operand = operand;
        }

        public IOperand Result { get; set; }
        private void OnResultBuilt(object sender, EventArgs<IOperand> e)
        {
            Result = e.Value;
            DialogResult = DialogResult.OK;
            Close();
        }

        public void DisplaisAddRange(IEnumerable<(Type SrcType, string propertyName, string displayName)> innerDisplayDictionary)
        {
            ucFilterBox.DisplaisAddRange(innerDisplayDictionary);
        }

        public void SourceAddRange(IEnumerable<IFilterData> additionalSource)
        {
            ucFilterBox.SourceAddRange(additionalSource);
        }
        public void ValidValuesAddRange(IEnumerable<(Type SrcType, string PropertyName, string DisplayMember, string ValueMember, Func<IEnumerable<ComboContainer>> ValidValues)> validValues)
        {
            ucFilterBox.ValidValuesAddRange(validValues);
        }
    }
}