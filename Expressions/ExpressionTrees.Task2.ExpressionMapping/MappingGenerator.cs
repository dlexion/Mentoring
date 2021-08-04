using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ExpressionTrees.Task2.ExpressionMapping
{
    public class MappingGenerator
    {
        public Mapper<TSource, TDestination> Generate<TSource, TDestination>() where TDestination : new()
        {
            var parameter = Expression.Parameter(typeof(TSource), "source");

            Type inType = typeof(TSource);
            var inProperties = inType.GetProperties();

            Type outType = typeof(TDestination);
            var outProperties = outType.GetProperties().ToDictionary(x => x.Name);

            var expressions = new List<Expression>();

            var inInstance = Expression.Variable(typeof(TSource), "input");
            var outInstance = Expression.Variable(typeof(TDestination), "result");

            expressions.Add(Expression.Assign(inInstance, parameter));
            expressions.Add(Expression.Assign(outInstance, Expression.New(typeof(TDestination))));

            foreach (var prop in inProperties)
            {
                if (!outProperties.TryGetValue(prop.Name, out var outProperty)) 
                    continue;

                var sourceValue = Expression.Property(inInstance, prop.Name);
                var outValue = Expression.Property(outInstance, outProperty);

                expressions.Add(Expression.Assign(outValue, sourceValue));
            }

            expressions.Add(outInstance);

            var body = Expression.Block(new[] { inInstance, outInstance }, expressions);

            var mapper = new Mapper<TSource, TDestination>(Expression.Lambda<Func<TSource, TDestination>>(body, parameter).Compile());
            return mapper;
        }
    }
}
