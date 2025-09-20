using System.ComponentModel.DataAnnotations;

namespace MotoFindrUserAPI.Application.DTOs
{
    public class MotoqueiroDTO
    {
        public int Id { get; set; }
        [Required]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "O nome deve ter no mínimo 3 caracteres")]
        public string Nome { get; set; } = string.Empty;
        [Required]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "CPF deve ter 11 caracteres")]
        public string Cpf { get; set; } = string.Empty;
        public int? EnderecoId { get; set; }
        [StringLength(200, MinimumLength = 5)]
        public EnderecoDTO? Endereco { get; set; }
        [Required]
        public DateTime DataNascimento { get; set; }
        public int? MotoId { get; set; }
    }

}
