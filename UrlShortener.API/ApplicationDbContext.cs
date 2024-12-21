using Microsoft.EntityFrameworkCore;

namespace UrlShortener.API {
    public class ApplicationDbContext :DbContext{
        //Program cs tarafında dbcontext optionsları vereceğim appdbcontext çağırıldığında bu ayarları base e gönderecek
        public ApplicationDbContext(DbContextOptions options):base(options) {
            
        }

        public DbSet<ShortenedUrl> ShortenedUrls{ get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder) {

            //ShortenedUrl tablosundaki Code sütunu en fazla 7 karaketer olabilir
            modelBuilder.Entity<ShortenedUrl>(builder => {
                builder.Property(shortenedCode => shortenedCode.Code).HasMaxLength(ShortLinkSettings.Length);
            });

            //Code unique olmalıdır 
            modelBuilder.Entity<ShortenedUrl>(builder => {
                builder.HasIndex(shortenedUrl => shortenedUrl.Code).IsUnique();
            });
        }


    }
}
