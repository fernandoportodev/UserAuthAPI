using System.ComponentModel.DataAnnotations;

namespace UserAuthAPI.DTOs;

public class UserRegisterDTO
{
    [Required]
    [MaxLength(150)]
    public string? Username { get; set; }
    [Required]
    [MinLength(8, ErrorMessage = "Deve ser inserido no mínimo 8 caracteres")]
    public string? Password { get; set; }
}
