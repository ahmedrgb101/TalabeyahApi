using DataAnnotationsExtensions;
using System.ComponentModel.DataAnnotations;

namespace TalabeyahTaskApi.Application.Identity.Users;

public class CreateUserRequest
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    [Email]
    public string Email { get; set; } = default!;
    public string UserName { get; set; } = default!;
    public string Password { get; set; } = default!;
    public string ConfirmPassword { get; set; } = default!;
    [Phone]
    public string? PhoneNumber { get; set; }
    public UserType Type { get; set; }
}