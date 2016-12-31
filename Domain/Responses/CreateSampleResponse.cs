using Domain.Objects;
using Trendyol.App.Domain.Responses;

namespace Domain.Responses
{
    public class CreateSampleResponse : BaseResponse
    {
        public Sample Sample { get; set; }
    }
}