using Microsoft.EntityFrameworkCore;
using System.Text;

namespace MusicHub
{
    using Data;
    using Initializer;
    using System;

    public class StartUp
    {
        public static void Main()
        {
            using MusicHubDbContext context =
                new MusicHubDbContext();

            DbInitializer.ResetDatabase(context);

            //Test your solutions here
            string result = ExportAlbumsInfo(context, 9);

            Console.WriteLine(result);
        }

        public static string ExportAlbumsInfo(MusicHubDbContext dbContext, int producerId)
        {
            // AsNoTracking() -> tells to EF Core that we do NOT need to track changes
            var albumsInfo = dbContext.Albums
                .AsNoTracking()
                .Include(a => a.Songs)
                .Where(a => a.ProducerId == producerId)
                .Select(a => new
                {
                    a.Name,
                    a.ReleaseDate,
                    ProducerName = a.Producer != null ? a.Producer.Name : null,
                    Songs = a.Songs
                        .Select(s => new
                        {
                            SongName = s.Name,
                            SongPrice = s.Price,
                            WriterName = s.Writer.Name
                        })
                        .OrderByDescending(s => s.SongName)
                        .ThenBy(s => s.WriterName)
                        .ToArray(),
                    TotalPrice = a.Price
                })
                .AsEnumerable()
                .OrderByDescending(a => a.TotalPrice)
                .ToArray();

            StringBuilder sb = new();

            foreach (var album in albumsInfo)
            {
                sb
                    .AppendLine($"-AlbumName: {album.Name}")
                    .AppendLine($"-ReleaseDate: {album.ReleaseDate.ToString("MM/dd/yyyy")}")
                    .AppendLine($"-ProducerName: {album.ProducerName}")
                    .AppendLine("-Songs:");

                int songIndex = 1;

                foreach (var s in album.Songs)
                {
                    sb
                        .AppendLine($"---#{songIndex++}")
                        .AppendLine($"---SongName: {s.SongName}")
                        .AppendLine($"---Price: {s.SongPrice:F2}")
                        .AppendLine($"---Writer: {s.WriterName}");
                }

                sb.AppendLine($"-AlbumPrice: {album.TotalPrice:F2}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string ExportSongsAboveDuration(MusicHubDbContext context, int duration)
        {
            throw new NotImplementedException();
        }
    }
}