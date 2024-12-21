namespace UrlShortener.API {
    public class ShortenedUrl {
        public Guid Id { get; set; }
        public string LongUrl { get; set; } = default!;
        public string ShortUrl { get; set; } = default!;
        public string Code { get; set; } = default!;
        public DateTime CreatedOnUtc { get; set; }
    }
}
