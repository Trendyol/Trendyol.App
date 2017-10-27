using Trendyol.App.Domain.Dtos;
using Trendyol.App.Domain.Responses;

namespace Trendyol.App.WebApi.Extensions
{
    public static class BaseResponseExtensions
    {
        public static void AddNotFoundMessage(this BaseResponse baseResponse)
        {
            baseResponse.AddErrorMessage(Constants.MessageTypes.NotFound);
        }
    }
}
