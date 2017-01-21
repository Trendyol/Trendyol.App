using System.Runtime.Serialization;

namespace Trendyol.App.Domain.Objects
{
    [DataContract]
    public class OperationParameter
    {
        [DataMember(Name = "op")]
        public string Operation { get; set; }

        [DataMember(Name = "path")]
        public string Path { get; set; }

        [DataMember(Name = "value")]
        public string Value { get; set; }
    }
}
