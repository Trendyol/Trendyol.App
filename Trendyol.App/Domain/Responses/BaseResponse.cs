using System.Collections.Generic;
using Trendyol.App.Domain.Dtos;

namespace Trendyol.App.Domain.Responses
{
    public class BaseResponse
    {
        public bool HasError => Errors.HasElements();

        public List<ErrorDto> Errors { get; set; }
    }
}