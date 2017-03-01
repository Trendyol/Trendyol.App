using System;
using Trendyol.App.Domain.Abstractions;

namespace Trendyol.App.Domain.Objects.DateTimeProviders
{
    public class UtcDateTimeProvider : IDateTimeProvider
    {
        public DateTime Now => DateTime.UtcNow;
    }
}