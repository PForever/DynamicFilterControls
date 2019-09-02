//using DynamicFilter.Abstract;
//using DynamicFilter.Abstract.Filters;
//using DynamicFilter.Factories;
//using DynamicFilter.Help;
//using DynamicFilter.Operands.Parametrized;
//using DynamicFilterControls.CollectionHelp;
//using DynamicFilterControls.Models;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace DynamicFilterControls.Logic
//{
//    public class OperandBuilderLogic
//    {
//        #region Props
//        //bsProperties.DataSource = PropertyList;
//        private Action<IList<IFilterData>> SetPropertySource { get; }
//        //bsOperations.DataSource = NullOperationList;
//        private Action<IList<NamedOperation>> SetOperationSource { get; }
//        private Action<object> SetValueObject { get; }
//        private Func<object> GetValueObject { get; }
//        private Action<string> SetValueString { get; }
//        private Func<string> GetValueString { get; }
//        private Action<bool> SetValueBool { get; }
//        private Func<bool> GetValueBool { get; }
//        private Action<byte> SetValueByte { get; }
//        private Func<byte> GetValueByte { get; }
//        private Action<short> SetValueShort { get; }
//        private Func<short> GetValueShort { get; }
//        private Action<int> SetValueInt { get; }
//        private Func<int> GetValueInt { get; }
//        private Action<long> SetValueLong { get; }
//        private Func<long> GetValueLong { get; }
//        private Action<decimal> SetValueDecimal { get; }
//        private Func<decimal> GetValueDecimal { get; }
//        private Action<float> SetValueFloat { get; }
//        private Func<float> GetValueFloat { get; }
//        private Action<double> SetValueDouble { get; }
//        private Func<double> GetValueDouble { get; }
//        private Action<DateTime> SetValueDateTime { get; }
//        private Func<DateTime> GetValueDateTime { get; }
//        //cbValue.SelectedItem == DefaultObject
//        private Action<ComboContainer> SetValueCombo { get; }
//        //cbValue.SelectedValue
//        private Func<ComboContainer> GetValueCombo { get; }
//        private Action<bool> SetNullubleEnable { get; }
//        private Action<bool> SetNullubleChecked{ get; }
//        private Func<bool> GetNullubleChecked{ get; }

//        public event EventHandler<EventArgs<IOperand>> OperandBuilt;
//        private NamedOperation Operation
//        {
//            get => (NamedOperation)bsOperations.Current;
//            set
//            {
//                var newPosition = bsOperations.Cast<NamedOperation>().TakeWhile(o => o.Type != value.Type).Count();
//                if (bsOperations.Position != newPosition) bsOperations.Position = newPosition;
//                else OnOperationChanged();
//            }
//        }
//        public IParameterizedFilterOperand Operand { set => ParsOperand(value); }
//        public object Value
//        {
//            get
//            {
//                var property = Property;
//                if (property.IsFromSource)
//                {
//                    var valueMember = property.ValidValues.ValueMember;
//                    if (cbValue.SelectedItem == DefaultObject) return null;
//                    return cbValue.SelectedValue;
//                }

//                var propertyType = property.PropertyType;
//                if (propertyType == typeof(string)) return GetValueString();
//                else if (propertyType == typeof(int)) return GetValueInt();
//                else if (propertyType == typeof(int?)) return GetNullubleChecked() ? default(int?) : (int?)GetValueInt();
//                else if (propertyType == typeof(DateTime)) return GetValueDateTime();
//                else if (propertyType == typeof(DateTime?)) return GetNullubleChecked() ? default(DateTime?) : GetValueDateTime();
//                else if (propertyType == typeof(double)) return GetValueDouble();
//                else if (propertyType == typeof(double?)) return GetNullubleChecked() ? default(double?) : (double?) GetValueDouble();
//                else if (propertyType == typeof(decimal)) return GetValueDecimal();
//                else if (propertyType == typeof(decimal?)) return GetNullubleChecked() ? default(decimal?) : (decimal?) GetValueDecimal();
//                else if (propertyType == typeof(float)) return GetValueFloat();
//                else if (propertyType == typeof(float?)) return GetNullubleChecked() ? default(float?) : (float?) GetValueFloat();
//                else if (propertyType == typeof(byte)) return GetValueByte();
//                else if (propertyType == typeof(byte?)) return GetNullubleChecked() ? default(byte?) : (byte?) GetValueByte();
//                else if (propertyType == typeof(long)) return GetValueLong();
//                else if (propertyType == typeof(long?)) return GetNullubleChecked() ? default(long?) : (long?) GetValueLong();
//                else if (propertyType == typeof(short)) return GetValueShort();
//                else if (propertyType == typeof(short?)) return GetNullubleChecked() ? default(short?) : (short?) GetValueShort();
//                else if (propertyType == typeof(bool)) return GetValueBool();
//                else if (propertyType == typeof(bool?)) return GetNullubleChecked() ? default(bool?) : (bool?) GetValueBool();
//                else throw new NotSupportedException(propertyType.FullName);
//            }
//            set
//            {
//                switch (value)
//                {
//                    case string v:
//                        SetValueString(v);
//                        break;
//                    case int v:
//                        SetValueInt(v);
//                        break;
//                    case DateTime v:
//                        SetValueDateTime(v);
//                        break;
//                    case double v:
//                        SetValueDouble(v);
//                        break;
//                    case float v:
//                        SetValueFloat(v);
//                        break;
//                    case decimal v:
//                        SetValueDecimal(v);
//                        break;
//                    case byte v:
//                        SetValueByte(v);
//                        break;
//                    case long v:
//                        SetValueLong(v);
//                        break;
//                    case short v:
//                        SetValueShort(v);
//                        break;
//                    case bool v:
//                        SetValueBool(v);
//                        break;
//                    case null:
//                        SetOperationSource(NullOperationList);
//                        //ParsValueType(Property.PropertyType);
//                        break;
//                    default: throw new NotSupportedException(value.GetType().FullName);
//                }
//            }
//        }
//        public IEnumerable<IFilterData> Properties
//        {
//            get => PropertyList.AsEnumerable();
//            set => SetPropertiesList(value);
//        }
//        public IFilterData Property
//        {
//            get => (IFilterData)bsProperties.Current;
//            set
//            {
//                int newPosition = bsProperties.Cast<IFilterData>().TakeWhile(o => o != value).Count();
//                if (bsProperties.Position != newPosition) bsProperties.Position = newPosition;
//                else OnPropertyChanged();
//            }
//        }
//        private IList<NamedOperation> OperationList = NamedOperationList.NamedOperations.Where(o => o.OperandType == OperandType.Value).ToList();
//        private IList<NamedOperation> NullOperationList = NamedOperationList.NamedOperations.Where(o => o.OperandType == OperandType.Value && o.ValueType == Models.ValueType.Equalable).ToList();
//        private IList<NamedOperation> CurrentOpearationList { get; set; }
//        private IList<IFilterData> PropertyList { get; set; }

//        public IDictionary<(Type SrcType, string PropertyName), (string DisplayMember, string ValueMember, Func<IEnumerable<ComboContainer>> ValidValues)> ValidValuesDictionary { get; set; }

//        private IList<Control> ValueControlList { get; set; }
//        #endregion

//        public OperandBuilderLogic() : this(ValidValuesDictionaryFactory())
//        {
//        }

//        private static IDictionary<(Type SrcType, string PropertyName), (string DisplayMember, string ValueMember, Func<IEnumerable<ComboContainer>> ValidValues)> ValidValuesDictionaryFactory() => new Dictionary<(Type SrcType, string PropertyName), (string DisplayMember, string ValueMember, Func<IEnumerable<ComboContainer>> ValidValues)>();


//        protected OperandBuilderLogic(IDictionary<(Type SrcType, string PropertyName), (string DisplayMember, string ValueMember, Func<IEnumerable<ComboContainer>> ValidValues)> validValuesDictionary)
//        {
//            ValidValuesDictionary = validValuesDictionary;
//        }
//        public OperandBuilderLogic(IFilterData property, IEnumerable<IFilterData> properties, IDictionary<(Type SrcType, string PropertyName), (string DisplayMember, string ValueMember, Func<IEnumerable<ComboContainer>> ValidValues)> validValuesDictionary) : this(validValuesDictionary)
//        {
//            SetPropertiesList(properties);
//            SetProperty(property);
//        }
//        public OperandBuilderLogic(IParameterizedFilterOperand operand, IEnumerable<IFilterData> properties, IDictionary<(Type SrcType, string PropertyName), (string DisplayMember, string ValueMember, Func<IEnumerable<ComboContainer>> ValidValues)> validValuesDictionary) : this(operand.Data, properties, validValuesDictionary)
//        {
//            ParsOperand(operand);
//        }

//        private void SetPropertiesList(IEnumerable<IFilterData> properies)
//        {
//            if (properies == null)
//            {
//                SetPropertySource(null);
//                return;
//            }
//            PropertyList = properies.Distinct().ToList();
//            if (PropertyList.Count == 0) throw new ArgumentException(nameof(properies));
//            SetPropertySource(PropertyList);
//        }
//        private static int _nameCounter;

//        private string IncrementName() => $"A{_nameCounter++}";

//        private void ParsOperand(IParameterizedFilterOperand operand)
//        {
//            var property = operand.Data;
//            SetProperty(property);
//            if (property.IsFromSource)
//            {
//                var (_, _, ValidValues) = property.ValidValues;
//                SetValueInList((nameof(ComboContainer.DisplayMember), nameof(ComboContainer.ValueMember), ValidValues));
//                SetValueObject(operand.Value);
//            }
//            else Value = operand.Value;
//            //switch (operand)
//            //{
//            //    case IParameterizedWithListFilterOperand o:
//            //        SetValueInList((o.DisplayMember, o.ValueMember, o.ValidValues));
//            //        SetValue(o.Value);
//            //        break;
//            //    default:
//            //        Value = operand.Value;
//            //        break;
//            //}
//            ParsFilterOperation(operand);
//        }

//        private void ParsFilterOperation(IParameterizedFilterOperand operand)
//        {
//            switch (operand)
//            {
//                case EqualInListFilter _:
//                case InnerEqualFilter _:
//                case EqualFilter _:
//                    SetOperation(FilterType.Equals);
//                    break;
//                case LessFilter _:
//                    SetOperation(FilterType.Less);
//                    break;
//                case MoreFilter _:
//                    SetOperation(FilterType.More);
//                    break;
//                case LikeFilter _:
//                    SetOperation(FilterType.Like);
//                    break;
//                case null: throw new ArgumentNullException(nameof(operand));
//                default: throw new NotSupportedException(operand.GetType().FullName);
//            }
//        }

//        private void ParsValueType(Type propertyType)
//        {
//            if (propertyType == typeof(string)) EnableVisible(tbValue);
//            else if (propertyType == typeof(int))
//            {
//                EnableVisible(nudValue);
//                SetValue((int)0);
//            }
//            else if (propertyType == typeof(int?)) EnableVisible(nudValue, true);
//            else if (propertyType == typeof(DateTime)) EnableVisible(dtpValue);
//            else if (propertyType == typeof(DateTime?)) EnableVisible(dtpValue, true);
//            else if (propertyType == typeof(double))
//            {
//                EnableVisible(nudValue);
//                SetValue((double)0);
//            }
//            else if (propertyType == typeof(double?)) EnableVisible(nudValue, true);
//            else if (propertyType == typeof(decimal))
//            {
//                EnableVisible(nudValue);
//                SetValue((decimal)0);
//            }
//            else if (propertyType == typeof(decimal?)) EnableVisible(nudValue, true);
//            else if (propertyType == typeof(float))
//            {
//                EnableVisible(nudValue);
//                SetValue((float)0);
//            }
//            else if (propertyType == typeof(float?)) EnableVisible(nudValue, true);
//            else if (propertyType == typeof(byte))
//            {
//                EnableVisible(nudValue);
//                SetValue((byte)0);
//            }
//            else if (propertyType == typeof(byte?)) EnableVisible(nudValue, true);
//            else if (propertyType == typeof(long))
//            {
//                EnableVisible(nudValue);
//                SetValue((long)0);
//            }
//            else if (propertyType == typeof(long?)) EnableVisible(nudValue, true);
//            else if (propertyType == typeof(short))
//            {
//                EnableVisible(nudValue);
//                SetValue((short)0);
//            }
//            else if (propertyType == typeof(short?)) EnableVisible(nudValue, true);
//            else if (propertyType == typeof(bool)) EnableVisible(chbValue);
//            else if (propertyType == typeof(bool?)) EnableVisible(chbValue, true);
//            else throw new NotSupportedException(propertyType.FullName);
//        }

//        private void SetValueInList((string DisplayMember, string ValueMember, Func<IEnumerable<ComboContainer>> ValidValues) validValuesData)
//        {
//            var editable = bsValues as ISupportInitialize;
//            editable.BeginInit();
//            cbValue.DataSource = null;
//            var type = validValuesData.ValidValues.Where(v => v != null).FirstOrDefault()?.GetType();

//            if (type != null)
//            {
//                if (!string.IsNullOrEmpty(validValuesData.ValueMember)) cbValue.ValueMember = validValuesData.ValueMember;
//                if (!string.IsNullOrEmpty(validValuesData.DisplayMember)) cbValue.DisplayMember = validValuesData.DisplayMember;

//                var nullObject = DefaultObject = new ComboContainer();//CreateDefault(type, validValuesData.DisplayMember);
//                var list = validValuesData.ValidValues.Replace(null, nullObject).ToList();
//                bsValues.DataSource = list;
//                //var selector = string.IsNullOrEmpty(validValuesData.DisplayMember) ? (Func<object, string>) ((object v) => v.ToString()) : (object v) => Helper.GetProperty<string>(v, validValuesData.DisplayMember);
//                cbValue.DropDownWidth = list.Select(s => s.DisplayMember).GetMaxLenForView();
//            }
//            else
//            {
//                cbValue.DisplayMember = nameof(ComboContainer.DisplayMember);
//                DefaultObject = new ComboContainer { DisplayMember = ControlHelper.DefaultMessage };
//                bsValues.DataSource = DefaultObject;
//            }

//            cbValue.DataSource = bsValues;
//            editable.EndInit();
//            EnableVisible(cbValue);
//        }

//        private object CreateDefault(Type type, string displayMember)
//        {
//            var defaultObject = TypeHelper.CreateNew(type);
//            string defaultMessage = ControlHelper.DefaultMessage;
//            TypeHelper.SetProperty(defaultObject, displayMember, defaultMessage);
//            return defaultObject;
//        }

//        #region Setting
     
//        private void SetOperation(FilterType type) => Operation = OperationList.Single(o => o.Type == type);
//        private void SetProperty(IFilterData data) => Property = PropertyList.Single(o => o == data);

//        private void EnableVisible(Control control, bool nullable = false)
//        {
//            if (control == null) foreach (var valueControl in ValueControlList) valueControl.Visible = false;
//            chbIsNull.Enabled = nullable;
//            chbIsNull.Checked = false;
//            foreach (var valueControl in ValueControlList) valueControl.Visible = valueControl.Name == control.Name;
//        }
//        #endregion

//        void CalculateResult()
//        {
//            switch (Operation.Type)
//            {
//                case FilterType.Equals:
//                    var value = Value;
//                    var property = Property;
//                    if (property.IsFromSource)
//                    {
//                        if (!String.IsNullOrEmpty(property.ValidValues.ValueMember))
//                        {
//                            var innerProperty = FilterDataFactory.CreateFilterData(property, property.ValidValues.ValueMember);
//                            Result = value == null ? OperandFactory.InnerEqualsFilter(property, innerProperty, value) : OperandFactory.InnerEqualsFilter(property, innerProperty, value, cbValue.Text);
//                        }
//                        else Result = value == null ? OperandFactory.EqualsFilter(property, value) : OperandFactory.EqualsFilter(property, value, cbValue.Text);
//                    }
//                    else Result = OperandFactory.EqualsFilter(Property, value);
//                    break;
//                case FilterType.Like:
//                    value = Value;
//                    Result = OperandFactory.LikeFilter(Property, value);
//                    break;
//                case FilterType.More:
//                    value = Value;
//                    Result = OperandFactory.MoreFilter(Property, value);
//                    break;
//                case FilterType.Less:
//                    value = Value;
//                    Result = OperandFactory.LessFilter(Property, value);
//                    break;
//                //case FilterType.Between:
//                //    throw new NotImplementedException();
//                default: throw new NotSupportedException();
//            }
//        }

//        public IOperand Result { get; private set; }
//        private ComboContainer DefaultObject { get; set; }

//        private void OnPropertyChanged(object sender, EventArgs e) => OnPropertyChanged();
//        private void OnPropertyChanged()
//        {
//            if (bsProperties.Current is IFilterData data)
//            {
//                if (data.IsFromSource)
//                {
//                    SetOperationSource(CurrentOpearationList = OperationList.Where(o => o.Type == FilterType.Equals).ToList());
//                    SetValueInList((nameof(ComboContainer.DisplayMember), nameof(ComboContainer.ValueMember), data.ValidValues.Values));
//                }
//                else
//                {
//                    var type = ControlHelper.WhatIsType(data.PropertyType);
//                    CurrentOpearationList = OperationList.Where(o => (o.ValueType & type) != 0).ToList();
//                    SetOperationSource(chbIsNull.Checked ? NullOperationList : CurrentOpearationList);
//                    ParsValueType(data.PropertyType);
//                }
//            }
//            else throw new ArgumentException(nameof(bsProperties.Current));
//        }

//        private void OnOperationChanged(object sender, EventArgs e) => OnOperationChanged();
//        private void OnOperationChanged()
//        {
//            if (bsOperations.Current is NamedOperation operation)
//            {
//                switch (operation.Type)
//                {
//                    //case FilterType.Between:
//                    //EnableVisible(cbOperand2);
//                    default: break;
//                }
//            }
//            else throw new ArgumentException(nameof(bsOperations.Current));
//        }

//        private void OnBuild(object sender, EventArgs e)
//        {
//            CalculateResult();
//            OperandBuilt.Invoke(this, new EventArgs<IOperand>(Result));
//        }

//        private void OnCheckedChanged(object sender, EventArgs e)
//        {
//            bool enabled = !chbIsNull.Checked;
//            foreach (var control in ValueControlList) control.Enabled = enabled;
//            SetOperationSource(enabled ? CurrentOpearationList : NullOperationList);
//        }
//    }
//}
