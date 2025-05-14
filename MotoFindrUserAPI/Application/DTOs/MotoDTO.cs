namespace MotoFindrUserAPI.Application.DTOs
{
    public class MotoDTO
    {
        public int Id { get; set; }
        public string Modelo { get; set; } = string.Empty;
        public int AnoDeFabricacao { get; set; }
        public string Chassi { get; set; } = string.Empty;
        public string Placa { get; set; } = string.Empty;
        public int? MotoqueiroId { get; set; }
    }
}
