using System;
using Trendyol.App.Domain.Abstractions;

namespace Trendyol.App.Domain.Objects.DateTimeProviders
{
    public class UtcDateTimeProvider : IDateTimeProvider
    {
        public DateTimeKind Kind => DateTimeKind.Utc;
        public DateTime Now => DateTime.UtcNow;
    }
}