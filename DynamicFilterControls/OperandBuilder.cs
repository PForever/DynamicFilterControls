using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using DynamicFilterControls.Models;
using System.ComponentModel;
using DynamicFilterControls.CollectionHelp;
using DynamicFilter.Help;
using DynamicFilter.Factories;
using DynamicFilter.Abstract;
using DynamicFilter.Abstract.Filters;
using DynamicFilter.Operands.Parametrized;
using System.Text.RegularExpressions;
using System.Text;
using FilterLibrary.SortableBindingList;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Threading;

namespace DynamicFilterControls
{
    internal partial class OperandBuilder : UserControl
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
        public IParameterizedFilterOperand Operand { set => ParsOperand(value); }
        public object Value
        {
            get
            {
                var property = Property;
                if (property.IsFromSource)
                {
                    var valueMember = property.ValidValues.ValueMember;
                    if (cbValue.SelectedItem == DefaultObject) return null;
                    return /*String.IsNullOrEmpty(valueMember) ? cbValue.SelectedItem : */cbValue.SelectedValue;
                }
                //if (cbValue.Visible) return cbValue.SelectedItem;

                var propertyType = property.PropertyType;
                if (propertyType == typeof(string)) return tbValue.Text;
                else if (propertyType == typeof(int)) return (int)nudValue.Value;
                else if (propertyType == typeof(int?)) return chbIsNull.Checked ? default(int?) : (int?)nudValue.Value;
                else if (propertyType == typeof(DateTime)) return dtpValue.Value.Date;
                else if (propertyType == typeof(DateTime?)) return chbIsNull.Checked ? default(DateTime?) : dtpValue.Value.Date;
                else if (propertyType == typeof(double)) return (double)nudValue.Value;
                else if (propertyType == typeof(double?)) return chbIsNull.Checked ? default(double?) : (double?)nudValue.Value;
                else if (propertyType == typeof(decimal)) return nudValue.Value;
                else if (propertyType == typeof(decimal?)) return chbIsNull.Checked ? default(decimal?) : (decimal?)nudValue.Value;
                else if (propertyType == typeof(float)) return (float)nudValue.Value;
                else if (propertyType == typeof(float?)) return chbIsNull.Checked ? default(float?) : (float?)nudValue.Value;
                else if (propertyType == typeof(byte)) return (byte)nudValue.Value;
                else if (propertyType == typeof(byte?)) return chbIsNull.Checked ? default(byte?) : (byte?)nudValue.Value;
                else if (propertyType == typeof(long)) return (long)nudValue.Value;
                else if (propertyType == typeof(long?)) return chbIsNull.Checked ? default(long?) : (long?)nudValue.Value;
                else if (propertyType == typeof(short)) return (short)nudValue.Value;
                else if (propertyType == typeof(short?)) return chbIsNull.Checked ? default(short?) : (short?)nudValue.Value;
                else if (propertyType == typeof(bool)) return (bool)chbValue.Checked;
                else if (propertyType == typeof(bool?)) return chbIsNull.Checked ? default(bool?) : (bool?)chbValue.Checked;
                else throw new NotSupportedException(propertyType.FullName);
            }
            set
            {
                switch (value)
                {
                    case string v:
                        SetValue(v);
                        break;
                    case int v:
                        SetValue(v);
                        break;
                    case DateTime v:
                        SetValue(v);
                        break;
                    case double v:
                        SetValue(v);
                        break;
                    case float v:
                        SetValue(v);
                        break;
                    case decimal v:
                        SetValue(v);
                        break;
                    case byte v:
                        SetValue(v);
                        break;
                    case long v:
                        SetValue(v);
                        break;
                    case short v:
                        SetValue(v);
                        break;
                    case bool v:
                        SetValue(v);
                        break;
                    case null:
                        bsOperations.DataSource = NullOperationList;
                        //ParsValueType(Property.PropertyType);
                        break;
                    default: throw new NotSupportedException(value.GetType().FullName);
                }
            }
        }
        public IEnumerable<IFilterData> Properties
        {
            get => PropertyList.AsEnumerable();
            set => SetPropertiesList(value);
        }
        public IFilterData Property
        {
            get => (IFilterData)bsProperties.Current;
            set
            {
                int newPosition = bsProperties.Cast<IFilterData>().TakeWhile(o => o != value).Count();
                if (bsProperties.Position != newPosition) bsProperties.Position = newPosition;
                else OnPropertyChanged();
            }
        }
        private IList<NamedOperation> OperationList = NamedOperationList.NamedOperations.Where(o => o.OperandType == OperandType.Value).ToList();
        private IList<NamedOperation> NullOperationList = NamedOperationList.NamedOperations.Where(o => o.OperandType == OperandType.Value && o.ValueType == Models.ValueType.Equalable).ToList();
        private IList<NamedOperation> CurrentOpearationList { get; set; }
        private IList<IFilterData> PropertyList { get; set; }

        public IDictionary<(Type SrcType, string PropertyName), (string DisplayMember, string ValueMember, Func<IEnumerable<ComboContainer>> ValidValues)> ValidValuesDictionary { get; set; }

        private IList<Control> ValueControlList { get; set; }
        #endregion

        public OperandBuilder() : this(ValidValuesDictionaryFactory())
        {
        }

        private static IDictionary<(Type SrcType, string PropertyName), (string DisplayMember, string ValueMember, Func<IEnumerable<ComboContainer>> ValidValues)> ValidValuesDictionaryFactory() => new Dictionary<(Type SrcType, string PropertyName), (string DisplayMember, string ValueMember, Func<IEnumerable<ComboContainer>> ValidValues)>();


        protected OperandBuilder(IDictionary<(Type SrcType, string PropertyName), (string DisplayMember, string ValueMember, Func<IEnumerable<ComboContainer>> ValidValues)> validValuesDictionary)
        {
            ValidValuesDictionary = validValuesDictionary;
            InitializeComponent();
            CustomInitialize();
        }
        public OperandBuilder(IFilterData property, IEnumerable<IFilterData> properties, IDictionary<(Type SrcType, string PropertyName), (string DisplayMember, string ValueMember, Func<IEnumerable<ComboContainer>> ValidValues)> validValuesDictionary) : this(validValuesDictionary)
        {
            SetPropertiesList(properties);
            SetProperty(property);
        }
        public OperandBuilder(IParameterizedFilterOperand operand, IEnumerable<IFilterData> properties, IDictionary<(Type SrcType, string PropertyName), (string DisplayMember, string ValueMember, Func<IEnumerable<ComboContainer>> ValidValues)> validValuesDictionary) : this(operand.Data, properties, validValuesDictionary)
        {
            ParsOperand(operand);
        }

        private void SetPropertiesList(IEnumerable<IFilterData> properies)
        {
            if (properies == null)
            {
                bsProperties.DataSource = null;
                return;
            }
            PropertyList = properies.Distinct().ToList();
            if (PropertyList.Count == 0) throw new ArgumentException(nameof(properies));
            bsProperties.DataSource = PropertyList;
        }
        private static int _nameCounter;

        private string IncrementName() => $"A{_nameCounter++}";

        private void ParsOperand(IParameterizedFilterOperand operand)
        {
            var property = operand.Data;
            SetProperty(property);
            if (property.IsFromSource)
            {
                var (_, _, ValidValues) = property.ValidValues;
                SetValueInList((nameof(ComboContainer.DisplayMember), nameof(ComboContainer.ValueMember), ValidValues));
                SetValue(operand.Value);
            }
            else Value = operand.Value;
            //switch (operand)
            //{
            //    case IParameterizedWithListFilterOperand o:
            //        SetValueInList((o.DisplayMember, o.ValueMember, o.ValidValues));
            //        SetValue(o.Value);
            //        break;
            //    default:
            //        Value = operand.Value;
            //        break;
            //}
            ParsFilterOperation(operand);
        }

        private void ParsFilterOperation(IParameterizedFilterOperand operand)
        {
            switch (operand)
            {
                case EqualInListFilter _:
                case InnerEqualFilter _:
                case EqualFilter _:
                    SetOperation(FilterType.Equals);
                    break;
                case LessFilter _:
                    SetOperation(FilterType.Less);
                    break;
                case MoreFilter _:
                    SetOperation(FilterType.More);
                    break;
                case LikeFilter _:
                    SetOperation(FilterType.Like);
                    break;
                case null: throw new ArgumentNullException(nameof(operand));
                default: throw new NotSupportedException(operand.GetType().FullName);
            }
        }

        private void ParsValueType(Type propertyType)
        {
            if (propertyType == typeof(string)) EnableVisible(tbValue);
            else if (propertyType == typeof(int))
            {
                EnableVisible(nudValue);
                SetValue((int)0);
            }
            else if (propertyType == typeof(int?)) EnableVisible(nudValue, true);
            else if (propertyType == typeof(DateTime)) EnableVisible(dtpValue);
            else if (propertyType == typeof(DateTime?)) EnableVisible(dtpValue, true);
            else if (propertyType == typeof(double))
            {
                EnableVisible(nudValue);
                SetValue((double)0);
            }
            else if (propertyType == typeof(double?)) EnableVisible(nudValue, true);
            else if (propertyType == typeof(decimal))
            {
                EnableVisible(nudValue);
                SetValue((decimal)0);
            }
            else if (propertyType == typeof(decimal?)) EnableVisible(nudValue, true);
            else if (propertyType == typeof(float))
            {
                EnableVisible(nudValue);
                SetValue((float)0);
            }
            else if (propertyType == typeof(float?)) EnableVisible(nudValue, true);
            else if (propertyType == typeof(byte))
            {
                EnableVisible(nudValue);
                SetValue((byte)0);
            }
            else if (propertyType == typeof(byte?)) EnableVisible(nudValue, true);
            else if (propertyType == typeof(long))
            {
                EnableVisible(nudValue);
                SetValue((long)0);
            }
            else if (propertyType == typeof(long?)) EnableVisible(nudValue, true);
            else if (propertyType == typeof(short))
            {
                EnableVisible(nudValue);
                SetValue((short)0);
            }
            else if (propertyType == typeof(short?)) EnableVisible(nudValue, true);
            else if (propertyType == typeof(bool)) EnableVisible(chbValue);
            else if (propertyType == typeof(bool?)) EnableVisible(chbValue, true);
            else throw new NotSupportedException(propertyType.FullName);
        }
        private IList<ComboContainer> _valueList;
        private void SetValueInList((string DisplayMember, string ValueMember, Func<IEnumerable<ComboContainer>> ValidValues) validValuesData)
        {
            var editable = bsValues as ISupportInitialize;
            editable.BeginInit();
            cbValue.DataSource = null;

            var validValues = validValuesData.ValidValues();

            var type = validValues.Where(v => v != null).FirstOrDefault()?.GetType();

            if (type != null)
            {
                if (!string.IsNullOrEmpty(validValuesData.ValueMember)) cbValue.ValueMember = validValuesData.ValueMember;
                if (!string.IsNullOrEmpty(validValuesData.DisplayMember)) cbValue.DisplayMember = validValuesData.DisplayMember;

                var nullObject = DefaultObject = new ComboContainer();
                var list = _valueList = validValues.Replace(null, nullObject).ToList();
                bsValues.DataSource = list;
                cbValue.DropDownWidth = list.Select(s => s.DisplayMember).GetMaxLenForView();
            }
            else
            {
                cbValue.DisplayMember = nameof(ComboContainer.DisplayMember);
                DefaultObject = new ComboContainer { DisplayMember = ControlHelper.DefaultMessage };
                bsValues.DataSource = DefaultObject;
            }

            cbValue.DataSource = bsValues;
            editable.EndInit();
            EnableVisible(cbValue);
        }

        private object CreateDefault(Type type, string displayMember)
        {
            var defaultObject = TypeHelper.CreateNew(type);
            string defaultMessage = ControlHelper.DefaultMessage;
            TypeHelper.SetProperty(defaultObject, displayMember, defaultMessage);
            return defaultObject;
        }

        #region Setting
        private void SetValue(string v)
        {
            chbIsNull.Checked = false;
            tbValue.Text = v;
        }
        private void SetValue(bool v)
        {
            chbIsNull.Checked = false;
            chbValue.Checked = v;
        }

        private void SetValue(byte v)
        {
            chbIsNull.Checked = false;
            nudValue.DecimalPlaces = 0;
            nudValue.Minimum = byte.MinValue;
            nudValue.Maximum = byte.MaxValue;
            nudValue.Value = v;
        }
        private void SetValue(short v)
        {
            chbIsNull.Checked = false;
            nudValue.DecimalPlaces = 0;
            nudValue.Minimum = short.MinValue;
            nudValue.Maximum = short.MaxValue;
            nudValue.Value = v;
        }
        private void SetValue(int v)
        {
            chbIsNull.Checked = false;
            nudValue.DecimalPlaces = 0;
            nudValue.Minimum = int.MinValue;
            nudValue.Maximum = int.MaxValue;
            nudValue.Value = v;
        }
        private void SetValue(long v)
        {
            chbIsNull.Checked = false;
            nudValue.DecimalPlaces = 0;
            nudValue.Minimum = long.MinValue;
            nudValue.Maximum = long.MaxValue;
            nudValue.Value = v;
        }
        private void SetValue(decimal v)
        {
            chbIsNull.Checked = false;
            nudValue.DecimalPlaces = 1;
            nudValue.Minimum = decimal.MinValue;
            nudValue.Maximum = decimal.MaxValue;
            nudValue.Value = v;
        }
        private void SetValue(float v) => SetValue((decimal)v);
        private void SetValue(double v) => SetValue((decimal)v);
        private void SetValue(DateTime v)
        {
            chbIsNull.Checked = false;
            dtpValue.Value = v;
        }
        private void SetValue(object value)
        {
            chbIsNull.Checked = false;
            var (DisplayMember, ValueMember, ValidValues) = Property.ValidValues;
            if (String.IsNullOrEmpty(ValueMember)) cbValue.SelectedItem = value;
            else cbValue.SelectedValue = value;
        }

        private void SetOperation(FilterType type) => Operation = OperationList.Single(o => o.Type == type);
        private void SetProperty(IFilterData data) => Property = PropertyList.Single(o => o == data);

        private void EnableVisible(Control control, bool nullable = false)
        {
            if (control == null) foreach (var valueControl in ValueControlList) valueControl.Visible = false;
            chbIsNull.Enabled = nullable;
            chbIsNull.Checked = false;
            foreach (var valueControl in ValueControlList) valueControl.Visible = valueControl.Name == control.Name;
        }
        #endregion

        void CalculateResult()
        {
            switch (Operation.Type)
            {
                case FilterType.Equals:
                    var value = Value;
                    var property = Property;
                    if (property.IsFromSource)
                    {
                        if (!String.IsNullOrEmpty(property.ValidValues.ValueMember))
                        {
                            var innerProperty = FilterDataFactory.CreateFilterData(property, property.ValidValues.ValueMember);
                            Result = value == null ? OperandFactory.InnerEqualsFilter(property, innerProperty, value) : OperandFactory.InnerEqualsFilter(property, innerProperty, value, cbValue.Text);
                        }
                        else Result = value == null ? OperandFactory.EqualsFilter(property, value) : OperandFactory.EqualsFilter(property, value, cbValue.Text);
                    }
                    else Result = OperandFactory.EqualsFilter(Property, value);
                    break;
                case FilterType.Like:
                    value = Value;
                    Result = OperandFactory.LikeFilter(Property, value);
                    break;
                case FilterType.More:
                    value = Value;
                    Result = OperandFactory.MoreFilter(Property, value);
                    break;
                case FilterType.Less:
                    value = Value;
                    Result = OperandFactory.LessFilter(Property, value);
                    break;
                //case FilterType.Between:
                //    throw new NotImplementedException();
                default: throw new NotSupportedException();
            }
        }

        public IOperand Result { get; private set; }
        private ComboContainer DefaultObject { get; set; }
        private const string ManyAnySymbols = ".*";

        private void CustomInitialize()
        {
            ValueControlList = new Control[] { tbValue, nudValue, dtpValue, cbValue, chbValue };

            bsProperties.DataSource = typeof(IFilterData);

            cbOperations.DisplayMember = nameof(NamedOperation.DisplayName);
            cbOperations.ValueMember = nameof(NamedOperation.Type);

            bsOperations.DataSource = typeof(NamedOperation);

            //cbProperty.DisplayMember = nameof(IFilterData.DisplayName);
        }

        private void OnPropertyChanged(object sender, EventArgs e) => OnPropertyChanged();
        private void OnPropertyChanged()
        {
            if (bsProperties.Current is IFilterData data)
            {
                if (data.IsFromSource)
                {
                    bsOperations.DataSource = CurrentOpearationList = OperationList.Where(o => o.Type == FilterType.Equals).ToList();
                    SetValueInList((nameof(ComboContainer.DisplayMember), nameof(ComboContainer.ValueMember), data.ValidValues.Values));
                }
                else
                {
                    Models.ValueType type = ControlHelper.WhatIsType(data.PropertyType);
                    CurrentOpearationList = OperationList.Where(o => (o.ValueType & type) != 0).ToList();
                    bsOperations.DataSource = chbIsNull.Checked ? NullOperationList : CurrentOpearationList;
                    ParsValueType(data.PropertyType);
                }
            }
            else throw new ArgumentException(nameof(bsProperties.Current));
        }

        private void OnOperationChanged(object sender, EventArgs e) => OnOperationChanged();
        private void OnOperationChanged()
        {
            if (bsOperations.Current is NamedOperation operation)
            {
                switch (operation.Type)
                {
                    //case FilterType.Between:
                    //EnableVisible(cbOperand2);
                    default: break;
                }
            }
            else throw new ArgumentException(nameof(bsOperations.Current));
        }

        private void OnBuild(object sender, EventArgs e)
        {
            CalculateResult();
            OperandBuilt.Invoke(this, new EventArgs<IOperand>(Result));
        }

        private void OnCheckedChanged(object sender, EventArgs e)
        {
            bool enabled = !chbIsNull.Checked;
            foreach (var control in ValueControlList) control.Enabled = enabled;
            bsOperations.DataSource = enabled ? CurrentOpearationList : NullOperationList;
        }

        private void OnValueFormatted(object sender, ListControlConvertEventArgs e)
        {

        }

        private string GetRgexpPattern(string text)
        {
            var s = new StringBuilder(text.Length * 3 + 2);
            s.Append(ManyAnySymbols);
            foreach (var c in text)
            {
                s.Append(c).Append(ManyAnySymbols);
            }
            return s.ToString();
        }

        private void OnCbKeyUp(object sender, KeyEventArgs e)
        {
            //if(e.KeyCode > Keys.A && e.KeyCode < Keys.Z && !e.Control && !e.Alt || e.Control && (e.KeyCode == Keys.V || e.KeyCode == Keys.X) || e.KeyCode == Keys.Back || e.KeyCode == Keys.Delete)
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Escape) return;
            cbValue.DroppedDown = true;
        }
        private string _previouslyText;
        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        private CancellationTokenSource CancellationTokenSource
        {
            get => _cancellationTokenSource;
            set
            {
                _cancellationTokenSource.Cancel();
                _cancellationTokenSource = value;
            }
        }
        private async void OnCbTextUpdate(object sender, EventArgs e)
        {
            try
            {
                if (cbValue.SelectionLength != 0) return;
                var text = cbValue.Text;
                if (text == _previouslyText) return;
                var src = CancellationTokenSource = new CancellationTokenSource();
                _previouslyText = text;
                int selectionStart = cbValue.SelectionStart;
                var regex = new Regex(GetRgexpPattern(text));
                var list = String.IsNullOrEmpty(text) ? _valueList : await Task.Run(() => _valueList.AsParallel().Where(s => regex.IsMatch(s.DisplayMember)).ToList(), src.Token);
                //bsValues.Filter = String.IsNullOrEmpty(text) ? null : $"@0(it.{nameof(ComboContainer.DisplayMember)}, \"{GetRgexpPattern(text)}\")";
                if (src.IsCancellationRequested) return;

                bsValues.DataSource = list;
                cbValue.Text = text;

                cbValue.Select(selectionStart, 0);
            }
            catch (TaskCanceledException) {}
        }
    }
}
