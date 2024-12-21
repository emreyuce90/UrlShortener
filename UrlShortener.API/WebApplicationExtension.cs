using Microsoft.EntityFrameworkCore;

namespace UrlShortener.API {
    public static class WebApplicationExtension {
        public record ShortenUrlRequest(string Url);
        public static void AddMinimalApisExt(this WebApplication app) {


            app.MapPost(pattern: "shorten", async (

                ShortenUrlRequest request,
                UrlShorteningService urlShorteningService,
                ApplicationDbContext dbContext,
                HttpContext httpContext
                ) => {
                    //Gelen Url'i stringe dönüştürmeye çalışır başarılı olursa true başarısız olursa false döner
                    //Absolute pathini verir out ile dışarı atar , _ kullanmamızın sebebi bu değişkeni kullanmayacağız amacımız sadece urli validate etmek
                    if (!Uri.TryCreate(request.Url, UriKind.Absolute, out _)) {
                        return Results.BadRequest("The specified URL is invalid.");
                    }

                    //Kod üretilir
                    var code = await urlShorteningService.GenerateUniqueCode();

                    //Gelen istek elde edilir
                    var req = httpContext.Request;

                    // Shortened url sınıfı gerekli verilerle doldurulur
                    var shortenedUrl = new ShortenedUrl {
                        Id = Guid.NewGuid(),
                        LongUrl = request.Url,
                        Code = code,
                        ShortUrl = $"{req.Scheme}://{req.Host}/{code}",
                        CreatedOnUtc = DateTime.UtcNow
                    };



                    dbContext.ShortenedUrls.Add(shortenedUrl);

                    await dbContext.SaveChangesAsync();

                    return Results.Ok(shortenedUrl.ShortUrl);


                });

            app.MapGet("api/{code}", async (string code, ApplicationDbContext dbContext) => {
                var shortenedUrl = await dbContext
                    .ShortenedUrls
                    .SingleOrDefaultAsync(s => s.Code == code);

                if (shortenedUrl is null) {
                    return Results.NotFound();
                }

                return Results.Redirect(shortenedUrl.LongUrl);
            });

        }
    }
}
