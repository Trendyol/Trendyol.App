using System.Collections.Generic;
using System.Linq;
using Trendyol.App.Domain.Dtos;
using Trendyol.App.Domain.Enums;
using Trendyol.App.Domain.Responses;

namespace Trendyol.App.Mvc.Models
{
    public class BaseViewModel
    {
        public bool HasError => Messages.Any(m => m.Type == MessageType.Error);

        public List<MessageDto> Messages { get; set; }

        public BaseViewModel()
        {
            Messages = new List<MessageDto>();
        }

        public BaseViewModel(BaseResponse response)
        {
            Messages = response.Messages;
        }

        public void AddMessage(string content, MessageType type)
        {
            Messages.Add(new MessageDto(content, type));
        }
    }
}