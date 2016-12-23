using System;
using System.Reflection;

namespace Trendyol.App.WebApi.Models
{
    public class PatchParameter
    {
        public string Path { get; set; }

        public string Value { get; set; }

        public void Update(object target)
        {
            PropertyInfo propertyInfo = target.GetType().GetProperty(Path.ToLower(), BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            propertyInfo.SetValue(target, Convert.ChangeType(Value, propertyInfo.PropertyType), null);
        }
    }
}