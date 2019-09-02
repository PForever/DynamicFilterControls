using DynamicFilter.Abstract.Sort;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Data.SqlTypes;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DynamicFilter.Help
{
    public static class ExpressionLogicOperations
    {
        class ParameterVisitor : ExpressionVisitor
        {
            private readonly ReadOnlyCollection<ParameterExpression> from, to;
            public ParameterVisitor(ReadOnlyCollection<ParameterExpression> from, ReadOnlyCollection<ParameterExpression> to)
            {
                if (from == null) throw new ArgumentNullException("from");
                if (to == null) throw new ArgumentNullException("to");
                if (from.Count != to.Count) throw new InvalidOperationException(
                    "Parameter lengths must match");
                this.from = from;
                this.to = to;
            }
            protected override Expression VisitParameter(ParameterExpression node)
            {
                for (int i = 0; i < from.Count; i++)
                {
                    if (node == from[i]) return to[i];
                }
                return node;
            }
        }
        public static Expression<Func<T, bool>> AndAlso<T>(
            Expression<Func<T, bool>> x, Expression<Func<T, bool>> y)
        {
            var newY = new ParameterVisitor(y.Parameters, x.Parameters)
                .VisitAndConvert(y.Body, "AndAlso");
            return Expression.Lambda<Func<T, bool>>(
                Expression.AndAlso(x.Body, newY),
                x.Parameters);
        }
        public static LambdaExpression AndAlso(
            LambdaExpression x, LambdaExpression y)
        {
            var newY = new ParameterVisitor(y.Parameters, x.Parameters)
                .VisitAndConvert(y.Body, "AndAlso");
            return Expression.Lambda(
                Expression.AndAlso(x.Body, newY),
                x.Parameters);
        }
        public static Expression<Func<T, bool>> OrElse<T>(
            Expression<Func<T, bool>> x, Expression<Func<T, bool>> y)
        {
            var newY = new ParameterVisitor(y.Parameters, x.Parameters)
                .VisitAndConvert(y.Body, "OrElse");
            return Expression.Lambda<Func<T, bool>>(
                Expression.OrElse(x.Body, newY),
                x.Parameters);
        }
        public static LambdaExpression OrElse(
            LambdaExpression x, LambdaExpression y)
        {
            var newY = new ParameterVisitor(y.Parameters, x.Parameters)
                .VisitAndConvert(y.Body, "OrElse");
            return Expression.Lambda(
                Expression.OrElse(x.Body, newY),
                x.Parameters);
        }
        public static Expression<Func<T, bool>> Or<T>(
            Expression<Func<T, bool>> x, Expression<Func<T, bool>> y)
        {
            var newY = new ParameterVisitor(y.Parameters, x.Parameters)
                .VisitAndConvert(y.Body, "Or");
            return Expression.Lambda<Func<T, bool>>(
                Expression.Or(x.Body, newY),
                x.Parameters);
        }
        public static LambdaExpression Or(
            LambdaExpression x, LambdaExpression y)
        {
            var newY = new ParameterVisitor(y.Parameters, x.Parameters)
                .VisitAndConvert(y.Body, "Or");
            return Expression.Lambda(
                Expression.Or(x.Body, newY),
                x.Parameters);
        }

        //private static Expression SymbolConditionsOrDefaultr<TEntity, TValue>(Expression<Func<TEntity, TValue>> property, IList<TValue> node, int elementIndex, int nodeIndex, int propertyCount)
        //{
        //    if (nodeIndex >= node.Count || elementIndex > propertyCount) return null;
        //    var charter = Expression.Constant(node[nodeIndex]);

        //    var item = Expression.ArrayIndex(property, Expression.Constant(elementIndex));
        //    var eq = Expression.Equal(item, charter);

        //    Expression subBranches = null;
        //    int nextIndex = ++nodeIndex;
        //    do
        //    {
        //        var innerExpression = SymbolConditionsOrDefaultr(property, node, ++elementIndex, nextIndex, propertyCount);
        //        if (innerExpression == null) break;

        //        var subBranch = Expression.AndAlso(eq, innerExpression);

        //        subBranches = subBranches == null ? subBranch : Expression.OrElse(subBranches, subBranch);
        //    } while (elementIndex < propertyCount);

        //    return subBranches;
        //}


        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> src, LambdaExpression lambda, SortingDirection direction = SortingDirection.Asc)
        {
            var typeResult = lambda.ReturnType;
            var orderby = GetOrder<T>(typeResult, direction);
            return (IOrderedQueryable<T>)orderby.Invoke(null, new object[] { src, lambda });
        }
        public static IOrderedQueryable OrderBy(this IQueryable src, Type type, LambdaExpression lambda, SortingDirection direction = SortingDirection.Asc)
        {
            var typeResult = lambda.ReturnType;
            var orderby = GetOrder(type, typeResult, direction);
            return (IOrderedQueryable)orderby.Invoke(null, new object[] { src, lambda });
        }

        private static MethodInfo GetOrder<T>(Type typeResult, SortingDirection direction) => GetOrder(typeof(T), typeResult, direction);

        private static MethodInfo GetOrder(Type typeSrc, Type typeResult, SortingDirection direction)
        {
            string name;
            switch (direction)
            {
                case SortingDirection.Asc:
                    name = nameof(Queryable.OrderBy);
                    break;
                case SortingDirection.Desc:
                    name = nameof(Queryable.OrderByDescending);
                    break;
                default: throw new NotSupportedException(direction.ToString());
            }
            return _methodDictionary2.GetOrAdd((name, typeSrc, typeResult, 2), MethodFactory);
        }
        private static MethodInfo GetThen<T>(Type typeResult, SortingDirection direction) => GetThen(typeof(T), typeResult, direction);

        private static MethodInfo GetThen(Type typeSrc, Type typeResult, SortingDirection direction)
        {
            string name;
            switch (direction)
            {
                case SortingDirection.Asc:
                    name = nameof(Queryable.ThenBy);
                    break;
                case SortingDirection.Desc:
                    name = nameof(Queryable.ThenByDescending);
                    break;
                default: throw new NotSupportedException(direction.ToString());
            }
            return _methodDictionary2.GetOrAdd((name, typeSrc, typeResult, 2), MethodFactory);
        }

        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> src, params (LambdaExpression Selector, SortingDirection Direction)[] lambda)
        {
            var l = lambda[0];
            var s = src.OrderBy(l.Selector, l.Direction);
            for (int i = 1; i < lambda.Length; i++)
            {
                l = lambda[i];
                s = s.ThenBy(l.Selector, l.Direction);
            }
            return s;
        }

        public static IOrderedQueryable OrderBy(this IQueryable src, Type type, params (LambdaExpression Selector, SortingDirection Direction)[] lambda)
        {
            var l = lambda[0];
            var s = src.OrderBy(type, l.Selector, l.Direction);
            for (int i = 1; i < lambda.Length; i++)
            {
                l = lambda[i];
                s = s.ThenBy(type, l.Selector, l.Direction);
            }
            return s;
        }
        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> src, IEnumerable<(LambdaExpression Selector, SortingDirection Direction)> lambda)
        {
            return OrderBy(src, lambda.ToArray());
        }
        public static IOrderedQueryable OrderBy(this IQueryable src, Type type, IEnumerable<(LambdaExpression Selector, SortingDirection Direction)> lambda)
        {
            return OrderBy(src, type, lambda.ToArray());
        }

        public static IOrderedQueryable<T> ThenBy<T>(this IOrderedQueryable<T> src, LambdaExpression lambda, SortingDirection direction)
        {
            var typeResult = lambda.ReturnType;
            var thenBy = GetThen<T>(typeResult, direction);
            return (IOrderedQueryable<T>)thenBy.Invoke(null, new object[] { src, lambda });
        }
        public static IOrderedQueryable ThenBy(this IOrderedQueryable src, Type type, LambdaExpression lambda, SortingDirection direction)
        {
            var typeResult = lambda.ReturnType;
            var thenBy = GetThen(type, typeResult, direction);
            return (IOrderedQueryable)thenBy.Invoke(null, new object[] { src, lambda });
        }

        public static IQueryable Take(this IQueryable src, Type type, int count) => (IQueryable)Invoke(nameof(Queryable.Take), type, src, count);
        public static IQueryable Skip(this IQueryable src, Type type, int count) => (IQueryable)Invoke(nameof(Queryable.Skip), type, src, count);
        public static IQueryable Where(this IQueryable src, Type type, LambdaExpression predicate) => (IQueryable)Invoke(nameof(Queryable.Where), type, src, predicate);
        public static int Count (this IQueryable src, Type type) => (int)Invoke(nameof(Queryable.Count), type, src);
        public static Task<int> CountAsync (this IQueryable src, Type type, CancellationToken token) => (Task<int>)InvokeData(nameof(QueryableExtensions.CountAsync), type, src, token);
        public static List<object> ToListExt(this IQueryable src)
        {
            try
            {
                var list = new List<object>();
                foreach (var item in src) list.Add(item);
                return list;
            }
            catch (Exception)
            {
                return null;
            }
            
        }
        private static object Invoke(string name, Type type, params object[] args)
        {
            var method = _methodDictionary.GetOrAdd((name, type, args.Length), MethodFactory);
            return method.Invoke(null, args);
        }
        private static object InvokeData(string name, Type type, params object[] args)
        {
            var method = _methodDictionary.GetOrAdd((name, type, args.Length), MethodFactoryData);
            return method.Invoke(null, args);
        }
        private static object InvokeThis(string name, Type type, params object[] args)
        {
            var method = _methodDictionary.GetOrAdd((name, type, args.Length), MethodFactoryThis);
            return method.Invoke(null, args);
        }
        private static MethodInfo MethodFactory((string name, Type src, Type dst, int len) key)
        {
            return typeof(Queryable).GetMethods(BindingFlags.Static | BindingFlags.Public).First(m => m.Name == key.name && m.GetParameters().Length == key.len).MakeGenericMethod(new[] { key.src, key.dst});
        }
        private static MethodInfo MethodFactory((string name, Type generic, int len) key)
        {
            return typeof(Queryable).GetMethods(BindingFlags.Static | BindingFlags.Public).First(m => m.Name == key.name && m.GetParameters().Length == key.len).MakeGenericMethod(new[] { key.generic });
        }
        private static MethodInfo MethodFactoryData((string name, Type generic, int len) key)
        {
            return typeof(QueryableExtensions).GetMethods(BindingFlags.Static | BindingFlags.Public).First(m => m.Name == key.name && m.GetParameters().Length == key.len).MakeGenericMethod(new[] { key.generic });
        }
        private static MethodInfo MethodFactoryThis((string name, Type generic, int len) key)
        {
            return typeof(ExpressionLogicOperations).GetMethods(BindingFlags.Static | BindingFlags.Public).First(m => m.Name == key.name && m.GetParameters().Length == key.len).MakeGenericMethod(new[] { key.generic });
        }

        private static ConcurrentDictionary<(string Name, Type Generic, int Len), MethodInfo> _methodDictionary = new ConcurrentDictionary<(string Name, Type Generic, int Len), MethodInfo>();
        private static ConcurrentDictionary<(string Name, Type Src, Type Dst, int Len), MethodInfo> _methodDictionary2 = new ConcurrentDictionary<(string Name, Type Generic, Type Dst, int Len), MethodInfo>();
    }
}
