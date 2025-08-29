using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MotoFindrUserAPI.Domain.Entities
{
    [Table("tb_mf_endereco")]
    public class EnderecoEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Logradouro { get; set; } = string.Empty;
        [Required]
        public string Complemento { get; set; } = string.Empty;
        [Required]
        public string Uf { get; set; } = string.Empty;
        [Required]
        public string Numero { get; set; } = string.Empty;
        [Required]
        public string Localidade { get; set; } = string.Empty;
        [Required]
        public int MotoqueiroId { get; set; }
    }
}
