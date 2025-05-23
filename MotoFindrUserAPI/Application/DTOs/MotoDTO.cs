using System.ComponentModel.DataAnnotations;

namespace MotoFindrUserAPI.Application.DTOs
{
    public class MotoDTO
    {
        public int Id { get; set; }
        [StringLength(100, MinimumLength = 2, ErrorMessage = "O modelo deve ter entre 2 e 100 caracteres")]
        public string Modelo { get; set; } = string.Empty;
        [Required]
        [Range(1900, 2025, ErrorMessage = "O ano deve estar entre 1900 e 2100")]
        public int AnoDeFabricacao { get; set; }
        [RegularExpression(@"^[A-HJ-NPR-Z0-9]{17}$",
                ErrorMessage = "Chassi inválido (deve ter 17 caracteres alfanuméricos)")]
        public string Chassi { get; set; } = string.Empty;
        [StringLength(7, MinimumLength = 7, ErrorMessage = "A placa deve ter 7 caracteres.")]
        public string Placa { get; set; } = string.Empty;
        public int? MotoqueiroId { get; set; }
    }
}
