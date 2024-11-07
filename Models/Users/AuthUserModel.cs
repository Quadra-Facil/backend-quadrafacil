using System.ComponentModel.DataAnnotations;

namespace QuadraFacil_backend.API.Models.Users;

public class User
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string? UserName { get; set; }

    [Required]
    [EmailAddress]
    public string? Email { get; set; }

    [MaxLength(100)]
    [Required]
    [MinLength(6)]
    public string? Password { get; set; }

    [Phone]
    public string? Phone { get; set; }

    [MaxLength(10)]
    public string? Role { get; set; }

}


//public class User
//{
//    public int Id { get; set; }
//    public string Username { get; set; }
//    public string Password { get; set; }
//    public string Role { get; set; }
//}
