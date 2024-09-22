using FluentValidation.Results;

namespace Application.Exceptions
{
    public class BadRequestEx : Exception 
    {

        public IDictionary<string, string[]> Errors { get; set; }
        public BadRequestEx(string message) : base(message)
        {
            Errors = new Dictionary<string, string[]>();
        }

        public BadRequestEx(string message,ValidationResult validationResult) : base(message)
        {
            Errors = validationResult.ToDictionary();
        }
  
    }
}
