using Trendyol.App.Domain.Responses;

namespace Trendyol.App.WebApi.Models
{
    public class ErrorResponse : BaseResponse
    {
        public string ErrorCode { get; set; }

        public string AdditionalInfo { get; set; }
    }
}
