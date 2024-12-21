using Microsoft.EntityFrameworkCore;

namespace UrlShortener.API {
    public class UrlShorteningService(ApplicationDbContext dbContext) {
        
        private readonly Random _random = new ();
        public async Task<string> GenerateUniqueCode() {

            //7 karakter oluştur
            var codeChars = new char[ShortLinkSettings.Length];
            //maximum 
             int maxValue = ShortLinkSettings.Alphabet.Length;
           
            while (true) {
                //7 kere dön
                for (var i = 0; i < ShortLinkSettings.Length; i++) {
                    // 1 ile 22 arasından bir sayı seç 8
                    var randomIndex = _random.Next(maxValue);
                    //H
                    codeChars[i] = ShortLinkSettings.Alphabet[randomIndex];
                }
                //123HBAS
                var code = new string(codeChars);
                // Dbye bak eğer bu code mevcut değilse kodu return et eğer varsa tekrar döngüye gir
                if (!await dbContext.ShortenedUrls.AnyAsync(s => s.Code == code)) {
                    return code;
                }
            }

        }


    }
}
