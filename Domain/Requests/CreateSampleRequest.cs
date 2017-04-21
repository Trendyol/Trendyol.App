using Domain.Validator;
using FluentValidation.Attributes;

namespace Domain.Requests
{
    [Validator(typeof(CreateSampleRequestValidator))]
    public class CreateSampleRequest
    {
        public string Name { get; set; }

        public string Size { get; set; }
    }
}