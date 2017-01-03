using Domain.Objects;
using Trendyol.App.Domain.Responses;

namespace Domain.Responses
{
    public class CreateSampleResponse : BaseResponse<Sample>
    {
        public override Sample Data { get; set; }
    }
}