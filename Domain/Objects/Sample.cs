using System;
using Trendyol.App.Data;
using Trendyol.App.Data.Attributes;

namespace Domain.Objects
{
    [Sequence(Name = "TestSequence")]
    public class Sample : IEntity<long>
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public int Size { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}