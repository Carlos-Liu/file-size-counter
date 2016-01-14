using System;
using System.Linq.Expressions;
using System.Reflection;
using FileSizeCounter.Properties;

namespace FileSizeCounter.MicroMvvm
{
  /// <summary>
  /// Facility to check if there is the property in an instance.
  /// </summary>
  public static class PropertySupport
  {
    /// <summary>
    /// Debug facility to help find the issues that the instance does not contain a property.
    /// </summary>
    /// <typeparam name="T">Property type.</typeparam>
    /// <param name="propertyExpresssion">property expression.</param>
    /// <returns>Property name.</returns>
    public static String ExtractPropertyName<T>(Expression<Func<T>> propertyExpresssion)
    {
      if (propertyExpresssion == null)
      {
        throw new ArgumentNullException("propertyExpresssion");
      }

      var memberExpression = propertyExpresssion.Body as MemberExpression;
      if (memberExpression == null)
      {
        throw new ArgumentException(Resources.PropertySupport_Error_ExpressionIsNotMemberExpression, "propertyExpresssion");
      }

      var property = memberExpression.Member as PropertyInfo;
      if (property == null)
      {
        throw new ArgumentException(Resources.PropertySupport_Error_MemberExpressionNotAccessProperty, "propertyExpresssion");
      }

      var getMethod = property.GetGetMethod(true);
      if (getMethod.IsStatic)
      {
        throw new ArgumentException(Resources.PropertySupport_Error_PropertyIsStatic, "propertyExpresssion");
      }

      return memberExpression.Member.Name;
    }
  }
}
