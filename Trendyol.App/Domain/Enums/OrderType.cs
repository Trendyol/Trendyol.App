using System.Runtime.Serialization;

namespace Trendyol.App.Domain.Enums
{
    public enum OrderType
    {
        [EnumMember(Value = "asc")]
        Asc,

        [EnumMember(Value = "desc")]
        Desc
    }
}