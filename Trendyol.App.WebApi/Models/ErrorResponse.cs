using System.Collections.Generic;
using Trendyol.App.Dtos;

namespace Trendyol.App.WebApi.Models
{
    public class ErrorResponse
    {
        public string Message { get; set; }

        public List<ErrorDto> Errors { get; set; }
    }
}
