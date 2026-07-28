using Infrastracture.Catalogue.EF.Persistance;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;

namespace Shopping.mediatype
    
{

    public static class Mediatypes{
        public const string image = "Image";
        public const string video = "Video";

    }
    public class mediaTypeDataSeeder
    {
        public static async Task seedAsync(IServiceProvider service) {

            using var scope = service.CreateScope();

            var db = scope.ServiceProvider.GetRequiredService<CatalogueContext>();
            var mediaTypes = new[]
            {
                Mediatypes.image, Mediatypes.video,
            
            
            
            };
            foreach (var t in mediaTypes) {
                if (!await db.mediaTypes.AnyAsync(x=>x.Name == t))
                {
                     db.mediaTypes.Add( new Catalogue.Domain.Models.MediaType {Name = t } );
                    
                    
                }


            }
            await db.SaveChangesAsync();



        }
    }
}
