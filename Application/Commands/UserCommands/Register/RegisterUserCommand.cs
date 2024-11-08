using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Application.Commands.UserCommands.Register;

public record RegisterUserCommand : IRequest<string>
{
    [Required]
    public string FirstName { get; set; } = string.Empty;
    [Required]
    public string LastName { get; set; } = string.Empty;
    [Required]        
    public string Username { get; set; } = string.Empty;
    [Required]
    public DateTime DateOfBirthDay { get; set; }
    [Required]
    [RegularExpression("^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$", ErrorMessage = "Email Invalied.")]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    [Required]
    [MinLength(6), MaxLength(32)]
    public string Password { get; set; } = string.Empty;
    [Required]
    [Compare("Password")]
    public string ConfirmPassword { get; set; } = string.Empty;
    public bool AcceptTerms { get; set; }
}