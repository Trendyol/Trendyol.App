using System;

namespace Trendyol.App.Domain.Abstractions
{
    public interface IDateTimeProvider
    {
        DateTimeKind Kind { get; }

        DateTime Now { get; }
    }
}