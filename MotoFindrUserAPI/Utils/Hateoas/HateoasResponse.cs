namespace MotoFindrUserAPI.Utils.Hateoas
{
    public class HateoasResponse<T>
    {
        public string? Message { get; set; }
        public T? Data { get; set; }
        public List<LinkDto> Links { get; set; }
    }
}
