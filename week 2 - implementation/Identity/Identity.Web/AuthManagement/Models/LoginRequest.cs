using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

public class LoginRequest
{
    [Required]
    [NotNull]
    public string Login { get; set; }

    [Required]
    [NotNull]
    public string FavoriteTeat { get; set; }
}