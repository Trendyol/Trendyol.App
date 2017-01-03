using Trendyol.App.Domain.Enums;

namespace Trendyol.App.Domain.Dtos
{
    public class MessageDto
    {
        public MessageType Type { get; set; }

        public string Content { get; set; }

        public MessageDto(string content, MessageType type)
        {
            Content = content;
            Type = type;
        }
    }
}