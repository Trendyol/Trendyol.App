using System;
using Trendyol.App.Data;

namespace Domain.Objects
{
    public class Sample : IEntity<long>
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public int Size { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}