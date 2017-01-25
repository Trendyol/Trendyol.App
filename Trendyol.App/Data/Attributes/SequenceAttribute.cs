using System;

namespace Trendyol.App.Data.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class SequenceAttribute : Attribute
    {
        public string Name { get; set; }
    }
}