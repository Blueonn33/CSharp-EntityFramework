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
            string result = ExportSongsAboveDuration(context, 4);

            Console.WriteLine(result);
        }

        public static string ExportAlbumsInfo(MusicHubDbContext dbContext, int producerId)
        {
            // AsNoTracking() -> tells to EF Core that we do NOT need to track changes
            var albumsInfo = dbContext.Albums
                .AsNoTracking()
                .AsSplitQuery()
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

        public static string ExportSongsAboveDuration(MusicHubDbContext dbContext, int duration)
        {
            var songs = dbContext.Songs
                .AsNoTracking()
                .Select(s => new
                {
                    s.Name,
                    Performers = s.SongPerformers
                        .Select(sp => sp.Performer)
                        .Select(p => p.FirstName + " " + p.LastName)
                        .OrderBy(p => p)
                        .ToArray(),
                    WriterName = s.Writer.Name,
                    AlbumProducerName = (s.Album != null && s.Album.Producer != null) ? s.Album.Producer.Name : null,
                    s.Duration
                })
                .OrderBy(s => s.Name)
                .ThenBy(s => s.WriterName)
                .AsEnumerable()
                .Where(s => s.Duration.TotalSeconds > duration)
                .ToArray();

            StringBuilder sb = new();
            int songIndex = 1;

            foreach (var song in songs)
            {
                sb
                    .AppendLine($"-Song #{songIndex++}")
                    .AppendLine($"---SongName: {song.Name}")
                    .AppendLine($"---Writer: {song.WriterName}");

                foreach (var performer in song.Performers)
                {
                    sb.AppendLine($"---Performer: {performer}");
                }

                sb
                    .AppendLine($"---AlbumProducer: {song.AlbumProducerName}")
                    .AppendLine($"---Duration: {song.Duration.ToString("c")}");
            }

            return sb.ToString().TrimEnd();
        }
    }
}