using System;

namespace Trendyol.App.EntityFramework.Mapping
{
    [AttributeUsage(AttributeTargets.Class)]
    public class DataContextAttribute : Attribute
    {
        private Type _type;

        public Type Type
        {
            get { return _type; }
            set
            {
                if (!value.IsSubclassOf(typeof(DataContextBase)))
                {
                    throw new InvalidOperationException($"Only types those derive from DataContextBase can be assigned to DataContextAttribute's Type property. The type {value.FullName} is not derived from DataContextBase.");
                }

                _type = value;
            }
        }
    }
}