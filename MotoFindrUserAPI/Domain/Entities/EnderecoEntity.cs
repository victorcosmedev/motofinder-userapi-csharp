using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

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
        [MinLength(8, ErrorMessage = "CEP precisa ter 8 caracteres")]
        [MaxLength(8, ErrorMessage = "CEP precisa ter 8 caracteres")]
        public string? Cep { get; set; }
        [Required]
        public int MotoqueiroId { get; set; }
        [Required]
        [JsonIgnore]
        public MotoqueiroEntity Motoqueiro { get; set; }
    }
}
