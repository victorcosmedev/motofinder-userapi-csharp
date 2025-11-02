using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace MotoFindrUserAPI.Domain.Entities
{
    [Table("tb_mf_user")]
    [Index(nameof(Email), IsUnique = true)]
    [Index(nameof(Username), IsUnique = true)]
    public class UserEntity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Username { get; set; } = string.Empty;
        [Required]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string PasswordHash { get; set; } = string.Empty;
    }
}
