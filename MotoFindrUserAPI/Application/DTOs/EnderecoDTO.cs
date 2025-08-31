using System.ComponentModel.DataAnnotations;

namespace MotoFindrUserAPI.Application.DTOs
{
    public class EnderecoDTO
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Logradouro é obrigatório")]
        [Range(10, 200, ErrorMessage = "Logradouro deve ter entre 10 e 200 caracteres")]
        public string Logradouro { get; set; } = string.Empty;
        [Required(ErrorMessage = "Complemento é obrigatório")]
        [Range(5, 100, ErrorMessage = "Complemento deve ter entre 5 e 100 caracteres")]
        public string Complemento { get; set; } = string.Empty;
        [Required(ErrorMessage = "UF é obrigatório")]
        [StringLength(2, MinimumLength = 2, ErrorMessage = "UF deve ter 2 caracteres")]
        public string Uf { get; set; } = string.Empty;
        [Required(ErrorMessage = "Número é obrigatório")]
        [Range(1, 10, ErrorMessage = "Número deve ter entre 1 e 10 caracteres")]
        public string Numero { get; set; } = string.Empty;
        [Required(ErrorMessage = "Localidade é obrigatório")]
        [Range(3, 100, ErrorMessage = "Localidade deve ter entre 3 e 100 caracteres")]
        public string Municipio { get; set; } = string.Empty;
        public int? MotoqueiroId { get; set; }
        [MinLength(8, ErrorMessage = "CEP precisa ter 8 caracteres")]
        [MaxLength(8, ErrorMessage = "CEP precisa ter 8 caracteres")]
        public string? Cep { get; set; }
    }
}
