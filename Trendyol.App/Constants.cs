using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Trendyol.App
{
    public struct Constants
    {
        public struct MessageTypes
        {
            public const string Error = "error";
            public const string Info = "info";
            public const string Warning = "warning";
            public const string Success = "success";
        }

        public const string DateTimeProvider = "DateTimeProvider";
        public const string ConfigManager = "ConfigManager";

        public static JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings()
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };
    }
}
