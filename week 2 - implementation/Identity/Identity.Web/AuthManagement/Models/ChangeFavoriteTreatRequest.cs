using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

public class ChangeFavoriteTreatRequest
{
    [Required]
    [NotNull]
    public string NewFavoriteTeat { get; set; }
}