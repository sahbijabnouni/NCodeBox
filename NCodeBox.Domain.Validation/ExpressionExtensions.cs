using System;
using System.Linq.Expressions;
using System.Reflection;

namespace NCodeBox.Domain.Validation
{
    public  class ExpressionExtensions<T> 
    {
        public static PropertyInfo GetProperty(Expression<Func<T, object>> property)
        {
            PropertyInfo propertyInfo = null;
            if (property.Body is MemberExpression)
            {
                propertyInfo = (property.Body as MemberExpression).Member as PropertyInfo;
            }
            else
            {
                propertyInfo = (((UnaryExpression)property.Body).Operand as MemberExpression).Member as PropertyInfo;
            }
            return propertyInfo;
        }
    }
}
