using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;

namespace CleanAspCore.Core.Common.SetProperty;

public static class SetPropertyExtensions
{
    public static UpdateSettersBuilder<TSource> SetPropertyIfNotNull<TSource, TProperty>(this UpdateSettersBuilder<TSource> builder, Expression<Func<TSource, TProperty>> propertyExpression,
        TProperty valueExpression) => valueExpression is not null ? builder.SetProperty(propertyExpression, valueExpression) : builder;
}
