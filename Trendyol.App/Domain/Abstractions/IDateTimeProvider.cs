using System;

namespace Trendyol.App.Domain.Abstractions
{
    public interface IDateTimeProvider
    {
        DateTime Now { get; }
    }
}