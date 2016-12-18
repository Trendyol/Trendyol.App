using System;

namespace Trendyol.App.Dtos
{
    public class ErrorDto
    {
        public string Code { get; set; }

        public string DisplayMessage { get; set; }

        public ErrorDto(string displayMessage)
            : this(displayMessage, String.Empty)
        {
        }

        public ErrorDto(string displayMessage, string code)
        {
            DisplayMessage = displayMessage;
            Code = code;
        }
    }
}
