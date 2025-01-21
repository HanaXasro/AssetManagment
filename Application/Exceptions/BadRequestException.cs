using FluentValidation.Results;

namespace Application.Exceptions
{
    public class BadRequestException : Exception 
    {

        public IDictionary<string, string[]> Errors { get; set; }
        public BadRequestException(string message) : base(message)
        {
            Errors = new Dictionary<string, string[]>();
        }

        public BadRequestException(string message,ValidationResult validationResult) : base(message)
        {
            Errors = validationResult.ToDictionary();
        }
  
    }
}
