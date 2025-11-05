using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MotoFindrUserAPI.Domain.Entities
{
    [Table("tb_mf_precificacao_moto")]
    public class PrecificacaoMotoEntity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int MotoId { get; set; }
        [Required]
        [ForeignKey(nameof(MotoId))]
        public MotoEntity Moto { get; set; }
        [Required]
        public double Preco { get; set; }
    }
}
