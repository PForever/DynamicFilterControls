using DynamicFilter.Abstract.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DynamicFilter.Help
{
    public static class FilterDataFactory
    {
        public static IFilterData CreateFilterData(IFilterData parent, Type propertyType, string propertyName, string displayName) => new FilterData(parent, propertyType, propertyName, displayName);
        public static IFilterData CreateFilterData(IFilterData parent, Type propertyType, string propertyName) => new FilterData(parent, propertyType, propertyName);
        public static IFilterData CreateFilterData(Type type) => new FilterData(type);
        public static IEnumerable<IFilterData> CreateChildren(Type type)
        {
            var properties = TypeDescriptor.GetProperties(type);
            var root = new FilterData(type);
            foreach (PropertyDescriptor property in properties)
                yield return new FilterData(root, property.PropertyType, property.Name, property.DisplayName);
        }
        public static IEnumerable<IFilterData> CreateChildren(this IFilterData parent)
        {
            var type = parent.PropertyType;
            var properties = TypeDescriptor.GetProperties(type);
            foreach (PropertyDescriptor property in properties)
                yield return new FilterData(parent, property.PropertyType, property.Name, property.DisplayName);
        }

        public static IFilterData CreateFilterData(IFilterData parent, string valueMember)
        {
            var parentType = TypeHelper.GetTypeOfData(parent);
            var propertyInfo = TypeDescriptor.GetProperties(parentType).Cast<PropertyDescriptor>().FirstOrDefault(p => p.Name == valueMember);
            var propertyType = propertyInfo.PropertyType;
            return CreateFilterData(parent, propertyType, valueMember);
        }
        public static IFilterData CreateFilterData(Type parentType, string propertyName)
        {
            var parent = CreateFilterData(parentType);
            var propertyInfo = TypeDescriptor.GetProperties(parentType).Cast<PropertyDescriptor>().FirstOrDefault(p => p.Name == propertyName);
            var propertyType = propertyInfo.PropertyType;
            return CreateFilterData(parent, propertyType, propertyName);
        }
    }
    public static class FilterDataFactoryExtension
    {
        //public static IEnumerable<IFilterData<T>> AddData<T, TProperty>(this IFilterData<T> src, Expression<Func<T, TProperty>> path, string displayName)
        //{
        //    var data = FilterDataFactory<T>.CreateFilterData(path);
        //    data.DisplayName = displayName;
        //    data.IsDisplayed = true;
        //    yield return src;
        //    yield return new FilterData<T>(data);
        //}
        //public static IEnumerable<IFilterData<T>> AddData<T, TProperty, TCollection, TValue>(this IFilterData<T> src, Expression<Func<T, TProperty>> path, IEnumerable<TCollection> validValues, Func<TCollection, string> displayFunc, Func<TCollection, TValue> valueFunc)
        //{
        //    var data = FilterDataFactory<T>.CreateFilterData(path);
        //    data.IsDisplayed = true;
        //    data.ValidValues = (nameof(ComboContainer.DisplayMember), nameof(ComboContainer.ValueMember), validValues.Select(v => new ComboContainer { DisplayMember = displayFunc(v), ValueMember = valueFunc(v) }));
        //    yield return src;
        //    yield return new FilterData<T>(data);
        //}



        public static IEnumerable<IFilterData<T>> AddData<T, TProperty>(this IEnumerable<IFilterData<T>> src, Expression<Func<T, TProperty>> path, string displayName)
        {
            var data = FilterDataFactory<T>.CreateFilterData(path);
            data.DisplayName = displayName;
            data.IsDisplayed = true;
            foreach (var item in src) yield return item;
            yield return new FilterData<T>(data);
        }
        public static IEnumerable<IFilterData<T>> AddData<T, TProperty, TCollection, TValue>(this IEnumerable<IFilterData<T>> src, Expression<Func<T, TProperty>> path, Expression<Func<TProperty, TValue>> valuePropFunc, Func<IEnumerable<TCollection>> validValues, Func<TCollection, string> displayFunc, Func<TCollection, TValue> valueFunc)
        {
            var data = FilterDataFactory<T>.CreateFilterData(path);
            var propValue = FilterDataFactory<T>.CreateFilterData(valuePropFunc);
            data.IsDisplayed = true;
            data.ValidValues = ("", propValue.PropertyName, () => validValues().Select(v => new ComboContainer { DisplayMember = displayFunc(v), ValueMember = valueFunc(v) }));
            foreach (var item in src) yield return item;
            yield return new FilterData<T>(data);
        }
    }
    public class FilterDataFactory<T>
    {

        public static IEnumerable<IFilterData> CreateFilterData(params (LambdaExpression Path, string DisplayName)[] Properties)
        {
            foreach (var (path, displayName) in Properties) yield return CreateFilterData(path, displayName);
        }
        public static IFilterData CreateFilterData<TProperty>(Expression<Func<T, TProperty>> path, string displayName)
        {
            var data = CreateFilterData(path);
            data.DisplayName = displayName;
            data.IsDisplayed = true;
            return data;
        }
        public static IEnumerable<IFilterData<T>> AddData<TProperty>(Expression<Func<T, TProperty>> path, string displayName)
        {
            var data = CreateFilterData(path);
            data.DisplayName = displayName;
            data.IsDisplayed = true;
            yield return new FilterData<T>(data);
        }
        public static IEnumerable<IFilterData<T>> AddData<TProperty, TCollection, TValue>(Expression<Func<T, TProperty>> path, Expression<Func<TProperty, TValue>> valuePropFunc, Func<IEnumerable<TCollection>> getValidValues, Func<TCollection, string> displayFunc, Func<TCollection, TValue> valueFunc)
        {
            var data = CreateFilterData(path);
            var propValue = CreateFilterData(valuePropFunc);
            data.IsDisplayed = true;
            data.ValidValues = ("", propValue.PropertyName, () => getValidValues().Select(v => new ComboContainer { DisplayMember = displayFunc(v), ValueMember = valueFunc(v) }));
            yield return new FilterData<T>(data);
        }

        public static IFilterData CreateFilterData(LambdaExpression path, string displayName)
        {
            var data = CreateFilterData(path);
            data.DisplayName = displayName;
            data.IsDisplayed = true;
            return data;
        }
        public static IFilterData CreateFilterData<TProperty>(Expression<Func<T, TProperty>> path)
        {
            if (path == null) throw new ArgumentNullException(nameof(path));
            if (!TryParsePropertyData(path.Body, out var data) || data == null)
            {
                throw new ArgumentException("path");
            }
            return data;
        }
        public static IFilterData CreateFilterData(LambdaExpression path)
        {
            if (path == null) throw new ArgumentNullException(nameof(path));
            if (!TryParsePropertyData(path.Body, out var data) || data == null)
            {
                throw new ArgumentException("path");
            }
            return data;
        }

        public static bool TryParsePropertyData(Expression expression, out IFilterData data)
        {
            data = null;
            var expression2 = RemoveConvert(expression);
            var memberExpression = expression2 as MemberExpression;
            var methodCallExpression = expression2 as MethodCallExpression;
            if (memberExpression != null)
            {
                var member = memberExpression.Member;

                if (!TryParsePropertyData(memberExpression.Expression, out IFilterData parent))
                {
                    return false;
                }
                data = FilterDataFactory.CreateFilterData(parent, member is PropertyInfo prop ? prop.PropertyType : member.DeclaringType, member.Name);
            }

            else if (methodCallExpression != null)
            {
                if (methodCallExpression.Method.Name == "Select" && methodCallExpression.Arguments.Count == 2)
                {
                    if (!TryParsePropertyData(methodCallExpression.Arguments[0], out IFilterData parent2))
                    {
                        return false;
                    }
                    var lambdaExpression = methodCallExpression.Arguments[1] as LambdaExpression;
                    if (lambdaExpression != null)
                    {
                        if (!TryParsePropertyData(lambdaExpression.Body, out IFilterData data2))
                        {
                            return false;
                        }
                        ChangeParent(data2, parent2);
                        data = data2;
                        return true;
                    }
                }
                return false;
            }
            else data = FilterDataFactory.CreateFilterData(expression.Type);

            return true;
        }

        private static void ChangeParent(IFilterData current, IFilterData newParent)
        {
            var parent = current.Parent;
            while (parent.Parent != null)
            {
                current = parent;
                parent = parent.Parent;
            }
            current.IsSelection = true;
            current.Parent = newParent;
        }

        public static Expression RemoveConvert(Expression expression)
        {
            while (expression.NodeType == ExpressionType.Convert || expression.NodeType == ExpressionType.ConvertChecked)
            {
                expression = ((UnaryExpression)expression).Operand;
            }
            return expression;
        }
    }
}
