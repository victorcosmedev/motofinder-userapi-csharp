using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MotoFindrUserAPI.Domain.Entities
{
    [Table("tb_mf_moto")]
    public class MotoEntity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Modelo { get; set; } = string.Empty;
        [Required]
        public int AnoDeFabricacao {  get; set; }
        [Required]
        public string Chassi { get; set; } = string.Empty;
        [Required]
        [MaxLength(7)]
        public string Placa { get; set; } = string.Empty;
        [Required]
        public int? MotoqueiroId { get; set; }
        public MotoqueiroEntity? Motoqueiro { get; set; }
    }
}
