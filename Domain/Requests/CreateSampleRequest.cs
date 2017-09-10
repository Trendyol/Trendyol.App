using System.ComponentModel.DataAnnotations;
using Domain.Validator;
using FluentValidation.Attributes;

namespace Domain.Requests
{
    [Validator(typeof(CreateSampleRequestValidator))]
    public class CreateSampleRequest
    {
        [Required]
        public string Name { get; set; }

        public string Size { get; set; }

        [Range(1, 12)]
        public int TestNumber { get; set; }
    }
}