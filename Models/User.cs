using System.ComponentModel.DataAnnotations;

namespace UserAuthAPI.Models;

public class User
{
    public int Id { get; set; }
    [Required]
    [MaxLength(150)]
    public string? Username { get; set; }
    [Required]
    [MinLength(8, ErrorMessage = "Deve ser inserido no mínimo 8 caracteres")]
    public string? PasswordHash { get; set; }
}
