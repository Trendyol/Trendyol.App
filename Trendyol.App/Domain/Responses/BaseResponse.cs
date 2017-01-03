using System.Collections.Generic;
using System.Linq;
using Trendyol.App.Domain.Dtos;
using Trendyol.App.Domain.Enums;

namespace Trendyol.App.Domain.Responses
{
    public class BaseResponse
    {
        public bool HasError => Messages.Any(m => m.Type == MessageType.Error);

        public List<MessageDto> Messages { get; set; }

        public BaseResponse()
        {
            Messages = new List<MessageDto>();
        }

        public void AddMessage(string content, MessageType type)
        {
            Messages.Add(new MessageDto(content, type));
        }
    }

    public abstract class BaseResponse<T> : BaseResponse
    {
        public abstract T Data { get; set; }
    }
}