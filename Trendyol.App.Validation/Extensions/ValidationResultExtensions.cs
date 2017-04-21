using System.Collections.Generic;
using FluentValidation.Results;
using Trendyol.App.Domain.Dtos;
using Trendyol.App.Domain.Responses;

namespace Trendyol.App.Validation.Extensions
{
    public static class ValidationResultExtensions
    {
        public static List<MessageDto> GetErrors(this ValidationResult validationResult)
        {
            List<MessageDto> errors = new List<MessageDto>();

            if (validationResult.Errors != null && validationResult.Errors.Count > 0)
            {
                foreach (ValidationFailure error in validationResult.Errors)
                {
                    errors.Add(new MessageDto(error.ErrorMessage, Constants.MessageTypes.Error));
                }
            }

            return errors;
        }

        public static T GetErrors<T>(this ValidationResult validationResult) where T : BaseResponse, new()
        {
            T response = new T();

            if (validationResult.Errors != null && validationResult.Errors.Count > 0)
            {
                foreach (ValidationFailure error in validationResult.Errors)
                {
                    response.AddErrorMessage(error.ErrorMessage);
                }
            }

            return response;
        }
    }
}