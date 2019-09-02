using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using DynamicFilterControls.Models;
using DynamicFilter.Abstract;
using DynamicFilter.Abstract.Filters;
using DynamicFilter.Help;
using DynamicFilter.Operands.CollectionOperands;
using DynamicFilterControls.Logic;

namespace DynamicFilterControls
{
    internal partial class InnerOperandBuilder : UserControl
    {
        private readonly InnerOperandBuilderLogic _innerOperandBuilderLogic;
        public IOperand InnerOperand => _innerOperandBuilderLogic.InnerOperand;
        public IDictionary<(Type SrcType, string PropertyName), (string DisplayMember, string ValueMember, Func<IEnumerable<ComboContainer>> ValidValues)> ValidValuesDictionary { get => _innerOperandBuilderLogic.ValidValuesDictionary; set => _innerOperandBuilderLogic.ValidValuesDictionary = value; }
        public IDictionary<(Type SrcType, string PropertyName), string> DisplaySource { get => _innerOperandBuilderLogic.DisplaySource; set => _innerOperandBuilderLogic.DisplaySource = value; }
        public ICollectionOperand Result { get => _innerOperandBuilderLogic.Result; set => _innerOperandBuilderLogic.Result = value; }
        public event EventHandler<EventArgs<IOperand>> Built { add => _innerOperandBuilderLogic.Built += value; remove => _innerOperandBuilderLogic.Built -= value; }
        public IFilterData Data { get => _innerOperandBuilderLogic.Data; set => _innerOperandBuilderLogic.Data = value; }
        public InnerOperandBuilder()
        {
            _innerOperandBuilderLogic = new InnerOperandBuilderLogic();
            InitializeComponent();
            bsCollectionOperands.DataSource = _innerOperandBuilderLogic.OperationList;
        }
        public InnerOperandBuilder(IFilterData data, IDictionary<(Type SrcType, string PropertyName), string> displaySource, IDictionary<(Type SrcType, string PropertyName), (string DisplayMember, string ValueMember, Func<IEnumerable<ComboContainer>> ValidValues)> validValuesDictionary)
        {
            _innerOperandBuilderLogic = new InnerOperandBuilderLogic(data, displaySource, validValuesDictionary);
            InitializeComponent();
            bsCollectionOperands.DataSource = _innerOperandBuilderLogic.OperationList;
        }

        public InnerOperandBuilder(IFilterData data, IDictionary<(Type SrcType, string PropertyName), string> displaySource, ICollectionOperand operand, IDictionary<(Type SrcType, string PropertyName), (string DisplayMember, string ValueMember, Func<IEnumerable<ComboContainer>> ValidValues)> validValuesDictionary) : this(data, displaySource, validValuesDictionary) => _innerOperandBuilderLogic = new InnerOperandBuilderLogic(data, displaySource, operand, validValuesDictionary);
        private void OnBuild(object sender, EventArgs e) => _innerOperandBuilderLogic.OnBuild(bsCollectionOperands.Current as NamedOperation);
        private void OnCreate(object sender, EventArgs e) => _innerOperandBuilderLogic.OnCreate();
    }
}
