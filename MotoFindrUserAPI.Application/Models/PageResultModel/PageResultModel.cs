namespace MotoFindrUserAPI.Application.Models.PageResultModel
{
    public class PageResultModel<T>
    {
        public required T Items { get; set; }
        public int TotalItens { get; set; }
        public int NumeroPagina { get; set; }
        public int TamanhoPagina { get; set; }
    }
}
