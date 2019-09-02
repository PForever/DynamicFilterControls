using DynamicFilter.Abstract.Filters;
using DynamicFilter.Help;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace DynamicFilter.Factories
{
    public class FilterBuilder
    {

        internal static LambdaExpression EqualPredicate(IFilterData data, object value)
        {
            CreateConvertedParameters(data, value, out var parameterExp, out var constExp, out var propertyConvertedExp);

            var equalExp = Expression.Equal(propertyConvertedExp, constExp);
            return Expression.Lambda(equalExp, parameterExp);
        }
        internal static LambdaExpression LikePredicate(IFilterData data, object value)
        {
            CreateConvertedParameters(data, value, out var parameterExp, out var constExp, out var propertyConvertedExp);
            //var containsInfo = typeof(String).GetMethod(nameof(String.Contains));
            //var equalExp = Expression.Call(propertyConvertedExp, containsInfo, constExp);

            var containsInfo = typeof(DbFunctions).GetMethod(nameof(DbFunctions.Like), new[] { typeof(string), typeof(string), typeof(string) });
            var equalExp = Expression.Call(null, containsInfo, propertyConvertedExp, ToLikeExp(value?.ToString() ?? ""), Expression.Constant(FilterDataHelper.LikeScreening.ToString()));

            return Expression.Lambda(equalExp, parameterExp);
        }
        internal static LambdaExpression LikeStringPredicate(IFilterData data, string value)
        {
            CreateParameters(data, value, out var parameterExp, out var constExp, out var propertyExp);

            var propertyType = propertyExp.Type;

            Expression ConvertOrFalse<T>(bool isConvarted, T convarted) => 
                !isConvarted ? Expression.Constant(false) : (Expression)Expression.Equal(propertyExp, Expression.Constant(convarted, propertyExp.Type));

            Expression body;
            if (propertyType.IsString())
            {
                var containsInfo = typeof(DbFunctions).GetMethod(nameof(DbFunctions.Like), new[] { typeof(string), typeof(string), typeof(string) });
                body = Expression.Call(null, containsInfo, propertyExp, ToLikeExp(value?.ToString() ?? ""), Expression.Constant(FilterDataHelper.LikeScreening.ToString()));
            }
            else if (propertyType == typeof(int) || propertyType == typeof(int?)) body = ConvertOrFalse(int.TryParse(value, out int res), res);
            else if (propertyType == typeof(DateTime) || propertyType == typeof(DateTime?)) body = ConvertOrFalse(DateTime.TryParse(value, out var res), res);
            else if (propertyType == typeof(double) || propertyType == typeof(double?)) body = ConvertOrFalse(double.TryParse(value, out var res), res);
            else if (propertyType == typeof(decimal) || propertyType == typeof(decimal?)) body = ConvertOrFalse(decimal.TryParse(value, out var res), res);
            else if (propertyType == typeof(float) || propertyType == typeof(float?)) body = ConvertOrFalse(float.TryParse(value, out var res), res);
            else if (propertyType == typeof(byte) || propertyType == typeof(byte?)) body = ConvertOrFalse(byte.TryParse(value, out var res), res);
            else if (propertyType == typeof(long) || propertyType == typeof(long?)) body = ConvertOrFalse(long.TryParse(value, out var res), res);
            else if (propertyType == typeof(short) || propertyType == typeof(short?)) body = ConvertOrFalse(short.TryParse(value, out var res), res);
            else if (propertyType == typeof(bool) || propertyType == typeof(bool?))
            {
                if (FilterDataHelper.IsTrue(value)) body = Expression.Equal(propertyExp, Expression.Constant(true, propertyExp.Type));
                else if (FilterDataHelper.IsFalse(value)) body = Expression.Equal(propertyExp, Expression.Constant(false, propertyExp.Type));
                else body = ConvertOrFalse(bool.TryParse(value, out var res), res);
            }
            else body = Expression.Constant(false);

            return Expression.Lambda(body, parameterExp);
        }

        private static ConstantExpression ToLikeExp(string value) => Expression.Constant(FilterDataHelper.ToLikeString(value));

        internal static LambdaExpression SelectorDelegate(IFilterData data)
        {
            CreateConvertedParameters(data, null, out var parameterExp, out var _, out var propertyConvertedExp);
            return Expression.Lambda(propertyConvertedExp, parameterExp);
        }

        internal static LambdaExpression AnyPredicate(IFilterData data)
        {
            CreateConvertedParameters(data, null, out var parameterExp, out var _, out var propertyConvertedExp);

            var any = CreateAny(propertyConvertedExp);
            return Expression.Lambda(any, parameterExp);
        }

        internal static LambdaExpression AnyPredicate(IFilterData data, LambdaExpression lambdaExpression)
        {
            CreateConvertedParameters(data, null, out var parameterExp, out var _, out var propertyConvertedExp);

            var any = CreateAny(propertyConvertedExp, lambdaExpression);
            return Expression.Lambda(any, parameterExp);
        }

        internal static LambdaExpression AllPredicate(IFilterData data, LambdaExpression lambdaExpression)
        {
            CreateConvertedParameters(data, null, out var parameterExp, out var _, out var propertyConvertedExp);

            var all = CreateAll(propertyConvertedExp, lambdaExpression);
            return Expression.Lambda(all, parameterExp);
        }

        internal static LambdaExpression MorePredicate(IFilterData data, object value)
        {
            CreateConvertedParameters(data, value, out var parameterExp, out var constExp, out var propertyConvertedExp);

            var equalExp = Expression.GreaterThan(propertyConvertedExp, constExp);
            return Expression.Lambda(equalExp, parameterExp);
        }
        internal static LambdaExpression LessPredicate(IFilterData data, object value)
        {
            CreateConvertedParameters(data, value, out var parameterExp, out var constExp, out var propertyConvertedExp);

            var equalExp = Expression.LessThan(propertyConvertedExp, constExp);
            return Expression.Lambda(equalExp, parameterExp);
        }
        public static LambdaExpression PropertySelector(IFilterData data)
        {
            GetSelection(data, out var parameterExp, out var propertyExp);

            return Expression.Lambda(propertyExp, parameterExp);
        }

        private static void GetSelection(IFilterData data, out ParameterExpression parameterExp, out Expression propertyExp)
        {
            var childTree = FilterDataHelper.CreateChildTree(data);
            var parentData = GetParent(childTree);
            parameterExp = Expression.Variable(parentData.PropertyType);
            propertyExp = CreatePropertyOrSelect(childTree, parameterExp, data);
        }

        private static void CreateConvertedParameters(IFilterData data, object value, out ParameterExpression parameterExp, out ConstantExpression constExp, out Expression propertyConvertedExp)
        {
            CreateParameters(data, value, out parameterExp, out constExp, out var propertyExp);
            propertyConvertedExp = value != null ? Expression.Convert(propertyExp, value.GetType()) : propertyExp;
        }

        private static void CreateParameters(IFilterData data, object value, out ParameterExpression parameterExp, out ConstantExpression constExp, out Expression propertyExp)
        {
            var childTree = FilterDataHelper.CreateChildTree(data);
            var parentData = GetParent(childTree);
            parameterExp = Expression.Variable(parentData.PropertyType);
            constExp = Expression.Constant(value);
            propertyExp = CreatePropertyExp(childTree, parameterExp);
        }

        private static IFilterData GetParent(List<IFilterData> childTree) => childTree[childTree.Count - 1];

        private static Expression CreatePropertyExp(List<IFilterData> childTree, ParameterExpression parentExp)
        {
            Expression propertyExp = parentExp;
            for (int i = childTree.Count - 2; i >= 0; i--) propertyExp = Expression.Property(propertyExp, childTree[i].PropertyName);

            return propertyExp;
        }
        private static Expression CreatePropertyOrSelect(List<IFilterData> childTree, ParameterExpression parentExp, IFilterData data)
        {
            int count = childTree.Count;
            Expression propertyExp = parentExp;
            for (int i = count - 2; i >= 0; i--)
            {
                var current = childTree[i];
                if (current.IsSelection) return CreateSelect(propertyExp, current, data);
                propertyExp = Expression.Property(propertyExp, current.PropertyName);
            }

            return propertyExp;
        }

        private static Expression CreateSelect(Expression parentExp, IFilterData propertyInfo, IFilterData data)
        {
            var newData = data.Copy();
            var inner = FilterDataHelper.ChangeInnerParent(propertyInfo.Parent, newData);
            var innerLambda = PropertySelector(newData);
            var collectionType = parentExp.Type.GetInterface(nameof(IQueryable)) != null? typeof(Queryable) : parentExp.Type.GetInterface(nameof(IEnumerable)) != null ? typeof(Enumerable) : throw new ArgumentException(parentExp.Type.Name);
            var select = collectionType.GetMethods(BindingFlags.Static | BindingFlags.Public).First(m => m.Name == "Select" && m.GetParameters().Length == 2).MakeGenericMethod(new[] { inner.PropertyType, innerLambda.ReturnType });
            return Expression.Call(select, parentExp, innerLambda);
        }
        private static Expression CreateAll(Expression parentExp, LambdaExpression innerLambda)
        {
            var collectionType = parentExp.Type.GetInterface(nameof(IQueryable)) != null? typeof(Queryable) : parentExp.Type.GetInterface(nameof(IEnumerable)) != null ? typeof(Enumerable) : throw new ArgumentException(parentExp.Type.Name);
            var all = collectionType.GetMethods(BindingFlags.Static | BindingFlags.Public).First(m => m.Name == "All" && m.GetParameters().Length == 2).MakeGenericMethod(new[] { innerLambda.Parameters[0].Type });
            return Expression.Call(all, parentExp, innerLambda);
        }
        private static Expression CreateAny(Expression parentExp, LambdaExpression innerLambda)
        {
            var collectionType = parentExp.Type.GetInterface(nameof(IQueryable)) != null? typeof(Queryable) : parentExp.Type.GetInterface(nameof(IEnumerable)) != null ? typeof(Enumerable) : throw new ArgumentException(parentExp.Type.Name);
            var any = collectionType.GetMethods(BindingFlags.Static | BindingFlags.Public).First(m => m.Name == "Any" && m.GetParameters().Length == 2).MakeGenericMethod(new[] { innerLambda.Parameters[0].Type });
            return Expression.Call(any, parentExp, innerLambda);
        }
        private static Expression CreateAny(Expression parentExp)
        {
            var collectionType = parentExp.Type.GetInterface(nameof(IQueryable)) != null? typeof(Queryable) : parentExp.Type.GetInterface(nameof(IEnumerable)) != null ? typeof(Enumerable) : throw new ArgumentException(parentExp.Type.Name);
            var argType = TypeHelper.GetCollectionGenericArg(parentExp.Type);
            var any = collectionType.GetMethods(BindingFlags.Static | BindingFlags.Public).First(m => m.Name == "Any" && m.GetParameters().Length == 1).MakeGenericMethod(new[] { argType });
            return Expression.Call(any, parentExp);
        }

    }
}