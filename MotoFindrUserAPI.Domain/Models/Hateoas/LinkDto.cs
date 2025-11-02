namespace MotoFindrUserAPI.Domain.Models.Hateoas
{
    public class LinkDto
    {
        public string Rel { get; set; }
        public string Href { get; set; }
        public string Method { get; set; }
    }
}
