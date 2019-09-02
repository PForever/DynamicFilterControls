using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using DynamicFilter;
using DynamicFilter.Abstract;
using DynamicFilter.Abstract.Filters;
using DynamicFilter.Abstract.Sort;
using DynamicFilter.Help;

namespace DynamicFilterControls.CollectionHelp
{
    public static class ControlHelper
    {
        public static readonly string DefaultMessage = "---";

        public static async Task SetSourcesExt<TFiltered, TOrdered, TSelected, TProperty>(this LazyGridView lazyGridView,
            TSelected defaultElement,
            Expression<Func<TOrdered, TProperty>> defaultSort,
            Func<(IQueryable<TFiltered> Source, IDisposable Context)> filteredSourceFactory,
            Func<IQueryable<TFiltered>, IQueryable<TOrdered>> orderedSourceFactory,
            Func<IQueryable<TOrdered>, IQueryable<TSelected>> selectedSourceFactory)
        {
            await lazyGridView.SetSource(defaultElement, new[] { (defaultSort as LambdaExpression, SortingDirection.Asc) }, () => filteredSourceFactory() is var r ? (r.Source as IQueryable, r.Context) : default, s => orderedSourceFactory((IQueryable<TFiltered>)s), s => selectedSourceFactory((IQueryable<TOrdered>)s), typeof(TFiltered), typeof(TOrdered), typeof(TSelected));
        }
        public static async Task SetSourcesExt<TFiltered, TOrdered, TSelected, TProperty>(this LazyGridView lazyGridView,
            TSelected defaultElement,
            IEnumerable<(Expression<Func<TOrdered, TProperty>> Source, SortingDirection Direction)> defaultSort,
            Func<(IQueryable<TFiltered> Source, IDisposable Context)> filteredSourceFactory,
            Func<IQueryable<TFiltered>, IQueryable<TOrdered>> orderedSourceFactory,
            Func<IQueryable<TOrdered>, IQueryable<TSelected>> selectedSourceFactory)
        {
            await lazyGridView.SetSource(defaultElement, defaultSort.Select(s => (s.Source as LambdaExpression, s.Direction)), () => filteredSourceFactory() is var r ? (r.Source as IQueryable, r.Context) : default, s => orderedSourceFactory((IQueryable<TFiltered>)s), s => selectedSourceFactory((IQueryable<TOrdered>)s), typeof(TFiltered), typeof(TOrdered), typeof(TSelected));
        }

        internal static void PopulateTreeByOperand(IOperand operand, TreeNode parent, IDictionary<TreeNode, IOperand> tree, ContextMenuStrip menu)
        {
            
            switch (operand)
            {
                case IUnoOperand o:
                    var childOper = o.Operand;
                    var childNode = parent.Nodes.Add(childOper.Name);
                    childNode.ContextMenuStrip = menu;
                    tree.Add(childNode, childOper);
                    PopulateTreeByOperand(childOper, childNode, tree, menu);
                    break;
                case IBinaryOperand o:
                    var childOper1 = o.Left;
                    var childNode1 = parent.Nodes.Add(childOper1.Name);
                    childNode1.ContextMenuStrip = menu;
                    tree.Add(childNode1, childOper1);
                    PopulateTreeByOperand(childOper1, childNode1, tree, menu);

                    var childOper2 = o.Right;
                    var childNode2 = parent.Nodes.Add(childOper2.Name);
                    childNode2.ContextMenuStrip = menu;
                    tree.Add(childNode2, childOper2);
                    PopulateTreeByOperand(childOper2, childNode2, tree, menu);
                    break;
                case ICollectionOperand o:
                    var innerOper = o.InnerOperand;
                    if (innerOper == null) break;
                    var innerNode = parent.Nodes.Add(innerOper.Name);
                    innerNode.ContextMenuStrip = menu;
                    tree.Add(innerNode, innerOper);
                    PopulateTreeByOperand(innerOper, innerNode, tree, menu);
                    break;
                case IParameterizedOperand o:
                    break;
                case null: throw new ArgumentNullException(nameof(operand));
                default: throw new NotSupportedException(operand.GetType().FullName);
            }
        }

        public static (
            IEnumerable<IFilterData> source,
            IDictionary<(Type SrcType, string PropertyName), string> displayDictionary,
            IDictionary<(Type SrcType, string PropertyName), (string DisplayMember, string ValueMember, Func<IEnumerable<ComboContainer>> ValidValues)> validValuesDictionary)
            CreateDataFromColumns(Type type, DataGridViewColumnCollection columns)
        {
            var displayDictionary = new Dictionary<(Type SrcType, string PropertyName), string>(columns.Count);
            var source = new List<IFilterData>(columns.Count);
            var validValuesDictionary = new Dictionary<(Type SrcType, string PropertyName), (string DisplayMember, string ValueMember, Func<IEnumerable<ComboContainer>> ValidValues)>();
            var parent = new FilterData(type);
            foreach (DataGridViewColumn column in columns)
            {
                if (!column.IsDataBound && string.IsNullOrEmpty(column.DataPropertyName)) continue;
                var data = new FilterData(parent, column.ValueType, column.DataPropertyName, column.HeaderText);
                source.Add(data);
                var key = (type, data.PropertyName);
                displayDictionary.Add(key, data.DisplayName);
                if(column is DataGridViewComboBoxColumn cb && cb.DataSource is IEnumerable src)
                {
                    Func<IEnumerable<ComboContainer>> validValues;
                    if (src is IEnumerable<ComboContainer> s)
                    {
                        validValues = () => s;
                    }
                    else
                    {
                        var selector = ContainerSelector(cb.DisplayMember, cb.ValueMember);
                        validValues = () => src.Cast<object>().Select(selector).ToList();
                    }

                    validValuesDictionary.Add(key, (cb.DisplayMember, cb.ValueMember, validValues));
                }
            }
            return (source, displayDictionary, validValuesDictionary);
        }

        public static IEnumerable<(Type SrcType, string PropertyName, string DisplayName)> ToDisplayData<T>(this DataGridViewColumnCollection columns)
        {
            var type = typeof(T);
            foreach (DataGridViewColumn column in columns)
            {
                if (!column.IsDataBound && string.IsNullOrEmpty(column.DataPropertyName)) continue;
                yield return (type, column.DataPropertyName, column.HeaderText);
            }
        }

        public static Func<object, ComboContainer> ContainerSelector(string displayMember, string valueMember) => v => new ComboContainer
        {
            DisplayMember = v == null ? DefaultMessage : TypeHelper.GetPropertyString(v, displayMember),
            ValueMember = v == null ? v : TypeHelper.GetPropertyObject(v, valueMember)
        };

        internal static Models.ValueType WhatIsType(Type propertyType)
        {
            if (propertyType.IsString()) return Models.ValueType.Container | Models.ValueType.Equalable;
            else if(propertyType.IsLineSpace()) return Models.ValueType.Comparable | Models.ValueType.Equalable;
            else if (propertyType.IsBool()) return Models.ValueType.Equalable;
            else return Models.ValueType.Equalable;
        }


        private const int ScrollBarSpace = 12;
        private const int CharacterSize = 6;
        public static int GetMaxLenForView(this IEnumerable<string> displayValues) => displayValues.DefaultIfEmpty().Replace(default, string.Empty).Max(p => p.Length) * CharacterSize + ScrollBarSpace;
    }
}
