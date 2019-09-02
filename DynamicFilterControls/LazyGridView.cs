using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LazyLists;
using DynamicFilterControls.CollectionHelp;
using System.Linq.Expressions;
using DynamicFilter.Operands;
using System.IO;
using System.Runtime.InteropServices;
using DynamicFilterControls.Models;
using System.Data.Entity;
using DynamicFilter.Abstract.Sort;
using DynamicFilter.Abstract;
using DynamicFilter.Abstract.Filters;
using DynamicFilter.Help;
using DynamicFilter.Factories;

namespace DynamicFilterControls
{
    [Docking(DockingBehavior.Ask)]
    [Designer(typeof(MyUserControlDesigner))]
    public partial class LazyGridView : UserControl
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Panel Panel => splitContainer1?.Panel1;
        //[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        private DataGridView Dgv;// => stylishedDataGridView1.Grid;

        //[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        //public BindingSource DataSource => bindingSource;

        //public Func<(IQueryable Source, IDisposable Context)> SourceFactory { get; private set; }
        public event EventHandler<EventArgs<(int Index, object Item)>> Loaded;
        public event EventHandlerAsync<EventArgs<(int Index, object Item)>> LoadedAsync;
        public Func<(IQueryable Source, IDisposable Context)> FilteredSourceFactory { get; private set; }
        public Func<IQueryable, IQueryable> OrderedSourceFactory { get; private set; }
        public Func<IQueryable, IQueryable> SelectedSourceFactory { get; private set; }
        public Type FilteredSourceType { get; private set; }
        public Type OrderedSourceType { get; private set; }
        public Type SelectedSourceType { get; private set; }
        private IEnumerable<(LambdaExpression, SortingDirection)> _defaultSort;
        public object DefaultElement
        {
            get => OrderedCashedCollection?.DefaultElement;
            set
            {
                var col = OrderedCashedCollection;
                if (col == null) OrderedCashedCollection = new OrderedCashedCollection(value, 0);
                else col.DefaultElement = value;
            }
        }

        public async Task SetSource(Type type, object defaultElement, Func<(IQueryable, IDisposable)> sourceFactory, LambdaExpression defaultSort)
        {
            await SetSource(type, defaultElement, sourceFactory, new[] { (defaultSort, SortingDirection.Asc) });
        }
        public async Task SetSource(Type type, object defaultElement, Func<(IQueryable, IDisposable)> sourceFactory, IEnumerable<(LambdaExpression, SortingDirection)> defaultSort)
        {
            SelectedSourceType = OrderedSourceType = FilteredSourceType = type ?? throw new ArgumentNullException(nameof(type));
            OrderedSourceFactory = src => src;
            SelectedSourceFactory = src => src;
            FilteredSourceFactory = sourceFactory ?? throw new ArgumentNullException(nameof(sourceFactory));
            DefaultElement = defaultElement ?? throw new ArgumentNullException(nameof(defaultSort));
            _sortSelectors = _defaultSort = defaultSort;
            //if (FastFilterData == null) FastFilterData = Helper.CreateDataFromColumns(type, Dgv.Columns).source.ToList();

            await UpdateData();
        }
        public async Task SetSource(object defaultElement, IEnumerable<(LambdaExpression, SortingDirection)> defaultSort, Func<(IQueryable Source, IDisposable Context)> filteredSourceFactory, Func<IQueryable, IQueryable> orderedSourceFactory, Func<IQueryable, IQueryable> selectedSourceFactory, Type filteredSourceType, Type orderedSourceType, Type selectedSourceType)
        {
            FilteredSourceType = filteredSourceType ?? throw new ArgumentNullException(nameof(filteredSourceType));
            OrderedSourceType = orderedSourceType ?? throw new ArgumentNullException(nameof(orderedSourceType));
            SelectedSourceType = selectedSourceType ?? throw new ArgumentNullException(nameof(selectedSourceType));
            FilteredSourceFactory = filteredSourceFactory ?? throw new ArgumentNullException(nameof(filteredSourceFactory));
            OrderedSourceFactory = orderedSourceFactory ?? throw new ArgumentNullException(nameof(orderedSourceFactory));
            SelectedSourceFactory = selectedSourceFactory ?? throw new ArgumentNullException(nameof(selectedSourceFactory));
            DefaultElement = defaultElement ?? throw new ArgumentNullException(nameof(defaultSort));
            _sortSelectors = _defaultSort = defaultSort;
            //if(FastFilterData == null && filteredSourceType == selectedSourceType) FastFilterData = Helper.CreateDataFromColumns(selectedSourceType, Dgv.Columns).source.ToList();

            await UpdateData();
        }

        private IOperand _filter;
        private LambdaExpression _predicate;
        private IOperand _simpleFilter;
        private IMultiSortOperand _sort;
        private (LambdaExpression Selector, SortingDirection Direction)? _simpleSortSelectors;
        private IEnumerable<(LambdaExpression Selector, SortingDirection Direction)> _sortSelectors;

        private LazyCollection _lazyCollection;
        private OrderedCashedCollection _orderedCashedCollection;
        private IList<IFilterData> _fastFilterData;

        private OrderedCashedCollection OrderedCashedCollection
        {
            get => _orderedCashedCollection;
            set
            {
                _orderedCashedCollection = value;
                value.SortAppledAsync += OnSortAppledAsync;
                bindingSource.DataSource = value;
            }
        }

        public LazyGridView()
        {
            base.SetStyle(ControlStyles.Selectable | ControlStyles.AllPaintingInWmPaint, false);

            InitializeComponent();
            if (LicenseManager.UsageMode == LicenseUsageMode.Runtime)
            {
                splitContainer1.Panel1.ControlAdded += new ControlEventHandler(OnControlAdded);
                splitContainer1.Panel1.ControlRemoved += new ControlEventHandler(OnControlRemoved);
            }
            //InitializeDgv(Dgv);
            //if (LicenseManager.UsageMode == LicenseUsageMode.Designtime) return;

            _lazyCollection = new LazyCollection
            {
                BufferCount = 100,
                UploadCount = 20
            };
            OrderedCashedCollection = new OrderedCashedCollection(DefaultElement, 0);
        }

        async Task OnSortAppledAsync(SortEventArgs arg)
        {
            await SetSinglOrder(arg.Order);
        }

        private void InitializeDgv(DataGridView dgv)
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime) return;
            dgv.RowPrePaint += OnRowPrePaint;
            dgv.CellFormatting += OnCellFormatting;
            dgv.DataSourceChanged += OnDataSpourceChanged;
            ChangeBs();
        }
        private void ClearDgv(DataGridView dgv)
        {
            dgv.RowPrePaint -= OnRowPrePaint;
            dgv.CellFormatting -= OnCellFormatting;
            dgv.DataSourceChanged -= OnDataSpourceChanged;
            if(dgv.DataSource == bindingSource) dgv.DataSource = null;
        }

        private async Task SetSinglOrder((string property, ListSortDirection direction) order)
        {
            if (order.property == null)
            {
                //_sortSelectors = _sort?.Calculate() ?? _defaultSort;
                _simpleSortSelectors = null;
            }
            else
            {
                var data = FilterDataFactory.CreateFilterData(SelectedSourceType, order.property);
                var dr = SortingDirection.None;
                switch (order.direction)
                {
                    case ListSortDirection.Ascending:
                        dr = SortingDirection.Asc;
                        break;
                    case ListSortDirection.Descending:
                        dr = SortingDirection.Desc;
                        break;
                }
                var operand = OperandFactory.SortOperand(data, dr);
                /*_sortSelectors*/
                _simpleSortSelectors = OperandFactory.MultiSortOperand(operand).SortProperties.Single().Calculate();
            }
            await UpdateData();
        }

        private async void OnFilter(object sender, EventArgs e)
        {
            var filterDialog = FilteredSourceType == SelectedSourceType
                ? _filter == null ? new DynamicFilterForm(FilteredSourceType, Dgv.Columns) : new DynamicFilterForm(FilteredSourceType, Dgv.Columns, _filter)
                : _filter == null ? new DynamicFilterForm(FilteredSourceType) : new DynamicFilterForm(FilteredSourceType, _filter);
            AddValidValuesDictionary(filterDialog);
            AddPropertyData(filterDialog);
            AddDisplayDictionary(filterDialog);
            if (filterDialog.ShowDialog() == DialogResult.OK)
            {
                _filter = filterDialog.Result;
                var exp = (_simpleFilter?.AndAlso(_filter) ?? _filter).Calculate();
                if (exp.CanReduce) exp = (LambdaExpression)exp.Reduce();

                _predicate = exp;
                await UpdateData();
            }
        }

        private void AddDisplayDictionary(DynamicFilterForm filterDialog)
        {
            var dictionary = InnerDisplayDictionary;
            if (dictionary != null) filterDialog.DisplaisAddRange(dictionary);
        }
        private void AddValidValuesDictionary(DynamicFilterForm filterDialog)
        {
            var dictionary = InnerValidValuesDictionary;
            if (dictionary != null) filterDialog.ValidValuesAddRange(dictionary);
        }

        private void AddPropertyData(DynamicFilterForm filterDialog)
        {
            var additionalProperty = AdditionalProperty;
            if (additionalProperty != null) filterDialog.SourceAddRange(additionalProperty);
        }
        private void AddDisplayDictionary(DynamicSortForm filterDialog)
        {
            var dictionary = InnerDisplayDictionary;
            if (dictionary != null) filterDialog.DisplaisAddRange(dictionary);
        }

        private void AddPropertyData(DynamicSortForm filterDialog)
        {
            var additionalProperty = AdditionalProperty;
            if (additionalProperty != null) filterDialog.SourceAddRange(additionalProperty);
        }

        private void OnSort(object sender, EventArgs e)
        {
            var sortDialog = _sort == null ? new DynamicSortForm(OrderedSourceType, InnerDisplayDictionary) : new DynamicSortForm(OrderedSourceType, _sort, InnerDisplayDictionary);

            AddPropertyData(sortDialog);
            AddDisplayDictionary(sortDialog);

            if (sortDialog.ShowDialog() == DialogResult.OK)
            {
                _sort = sortDialog.Result;
                var exp = _sort.Calculate().Select(s => (s.Item1.CanReduce ? (LambdaExpression)s.Item1.Reduce() : s.Item1, s.Item2)).ToArray();

                _sortSelectors = exp;

                OrderedCashedCollection.RemoveSort();
                //await UpdateData();
            }
        }

        private void OnClearSort(object sender, EventArgs e)
        {
            _sort = null;
            _sortSelectors = _defaultSort;
            OrderedCashedCollection.RemoveSort();
            //await UpdateData();
        }

        private async void OnClearFilter(object sender, EventArgs e)
        {
            _filter = null;
            _predicate = _simpleFilter?.Calculate() ?? null;
            await UpdateData();
        }
        public IEnumerable<(Type SrcType, string propertyName, string displayName)> InnerDisplayDictionary { get; set; }
        public IEnumerable<(Type SrcType, string PropertyName, string DisplayMember, string ValueMember, Func<IEnumerable<ComboContainer>> ValidValues)> InnerValidValuesDictionary { get; set; }
        public IEnumerable<IFilterData> AdditionalProperty { get; set; }
        public IEnumerable<IFilterData> FastFilterData
        {
            get => _fastFilterData;
            set
            {
                if (value != null)
                {
                    _fastFilterData = value.ToList();
                    var bsList = value.Select(i => new Alias { DisplayName = i.DisplayName, FilterData = i }).ToList();
                    bsList.Insert(0, new Alias { DisplayName = "Все" });
                    cbSimpleFilter.DropDownWidth = bsList.Select(s => s.DisplayName).GetMaxLenForView();

                    bsSimpleFilters.DataSource = bsList;
                }
                else
                {
                    _fastFilterData = null;
                    if(bsSimpleFilters is BindingSource bs) bs.DataSource = null;
                }

            }
        }
        private async Task UpdateData()
        {
            var (source, context) = GetFilteredSource();
            var result = await _lazyCollection.TrySet(SelectedSourceType, source, () => context.Dispose());
            if (result.IsFailed) return;
            OrderedCashedCollection.Reset(_lazyCollection.Count);
            bindingSource.DataSource = null;
            bindingSource.DataSource = OrderedCashedCollection;
            //bindingSource.ResetBindings(false);
            //Dgv.DataSource = null;
            //Dgv.DataSource = bindingSource;
        }

        private (IQueryable Source, IDisposable Context) GetFilteredSource()
        {
            var (source, context) = FilteredSourceFactory();
            if(_predicate != null) source = source.Where(FilteredSourceType, _predicate);
            source = OrderedSourceFactory(source);
            if (_sortSelectors == null) throw new Exception();
            if(_simpleSortSelectors == null) source = source.OrderBy(OrderedSourceType, _sortSelectors);
            source = SelectedSourceFactory(source);
            if(_simpleSortSelectors != null && _simpleSortSelectors.Value is var s) source = source.OrderBy(SelectedSourceType, s.Selector, s.Direction);
            return (source, context);
        }

        private async void OnCellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime) return;

            if (e.RowIndex < 0 || bindingSource.Count <= e.RowIndex) return;
            if (bindingSource[e.RowIndex] == DefaultElement)
                Dgv.InvalidateCell(e.ColumnIndex, e.RowIndex);
        }

        private async void OnRowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime) return;

            int index = e.RowIndex;

            if (index < 0) return;

            var result = await _lazyCollection.TryGetItem(index);
            if (result.IsFailed)
            {
                if (Dgv.RowCount > index) Dgv.InvalidateRow(index);
                return;
            }
            if (index >= Dgv.RowCount || Dgv.Rows[index].DataBoundItem == result.Value) return;

            bindingSource[index] = result.Value;

            var arg = new EventArgs<(int Index, object Item)>((index, result.Value));
            await OnLoaded(arg);
        }

        private async Task OnLoaded(EventArgs<(int Index, object Item)> arg)
        {
            Loaded?.Invoke(this, arg);
            var loadedAsync = LoadedAsync;
            if (loadedAsync != null) await loadedAsync.Invoke(this, arg);
        }

        private async void OnSimpleFilterChanged(object sender, EventArgs e)
        {
            string text = tbSimpleFilter.Text;
            if (string.IsNullOrEmpty(text))
            {
                _simpleFilter = null;
                _predicate = _filter?.Calculate();
            }
            else
            {
                bsSimpleFilters.Position = cbSimpleFilter.SelectedIndex;
                if (bsSimpleFilters.Current is Alias alias)
                {
                    _simpleFilter = alias.FilterData is IFilterData data ? OperandFactory.LikeStringFilter(data, text) : OperandFactory.CreateFullFilter(FastFilterData, text);
                    _predicate = (_filter?.AndAlso(_simpleFilter) ?? _simpleFilter).Calculate();
                }
                else return;
            }
            await UpdateData();
        }

        private void OnDataSpourceChanged(object sender, EventArgs e)
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime) return;
            ChangeBs();
        }

        private void ChangeBs()
        {
            if (Dgv.DataSource == bindingSource) return;
            if (Dgv.DataSource is BindingSource bs)
            {
                CleanBs(bindingSource);
                bindingSource = bs;
                InitializeBs(bs);
            }
            else
            {
                bindingSource.DataSource = Dgv.DataSource;
                Dgv.DataSource = bindingSource;
            }
        }

        private void InitializeBs(BindingSource bs)
        {
            bindingNavigator.BindingSource = bs;
        }

        private void CleanBs(BindingSource bindingSource)
        {
            bindingNavigator.BindingSource = null;
        }

        private void OnControlAdded(object sender, ControlEventArgs e)
        {
            if(e.Control is DataGridView dgv)
            {
                if (Dgv != null) Controls.Remove(dgv);
                else
                {
                    Dgv = dgv;
                    InitializeDgv(dgv);
                }
            }
        }

        private void OnControlRemoved(object sender, ControlEventArgs e)
        {
            if (e.Control is DataGridView dgv)
            {
                if (Dgv != null)
                {
                    ClearDgv(Dgv);
                    Dgv = null;
                }
            }
        }
    }

    public class MyUserControlDesigner : System.Windows.Forms.Design.ControlDesigner
    {

        public override void Initialize(IComponent comp)
        {
            base.Initialize(comp);
            var uc = (LazyGridView)comp;
            EnableDesignMode(uc.Panel, "Panel");
            //EnableDesignMode(uc.Dgv, "Grid");
        }

    }

}
