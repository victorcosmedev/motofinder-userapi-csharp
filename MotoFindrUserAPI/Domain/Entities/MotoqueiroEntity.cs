using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MotoFindrUserAPI.Domain.Entities
{
    [Table("tb_mf_motoqueiro")]
    public class MotoqueiroEntity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Nome { get; set; }
        [Required]
        public string Cpf { get; set; }
        [Required]
        public string Endereco { get; set; }
        [Required]
        public DateTime DataNascimento { get; set; }
        public int? MotoId { get; set; }
        public MotoEntity? Moto { get; set; }

    }
}
