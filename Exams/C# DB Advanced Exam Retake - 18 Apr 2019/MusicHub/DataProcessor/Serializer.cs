using System.Linq;
using System.Net.Http.Headers;
using AutoMapper.QueryableExtensions;
using MusicHub.DataProcessor.ExportDtOs;
using MusicHub.XmlFacade;
using Newtonsoft.Json;

namespace MusicHub.DataProcessor
{
    using System;

    using Data;

    public class Serializer
    {
        public static string ExportAlbumsInfo(MusicHubDbContext context, int producerId)
        {
            var albumsToExport = context.Albums
                .Where(a => a.ProducerId == producerId)
                .OrderByDescending(a => a.Songs.Sum(s => s.Price))
                .Select(a => new
                {
                    AlbumName = a.Name,
                    ReleaseDate = a.ReleaseDate.ToString("MM/dd/yyyy"),
                    ProducerName = a.Producer.Name,
                    Songs = a.Songs
                        .Select(s => new
                        {
                            SongName = s.Name,
                            Price = s.Price.ToString("F2"),
                            Writer = s.Writer.Name
                        })
                        .OrderByDescending(s => s.SongName)
                        .ThenBy(s => s.Writer)
                        .ToArray(),
                    AlbumPrice = a.Songs.Sum(s => s.Price).ToString("F2")
                });

            var albumsJson = JsonConvert.SerializeObject(albumsToExport, Formatting.Indented);

            return albumsJson;
        }

        public static string ExportSongsAboveDuration(MusicHubDbContext context, int duration)
        {
            var songsToExport = context.Songs
                .Where(s => s.Duration.TotalSeconds > duration)
                .ProjectTo<ExportSongDtO>()
                .OrderBy(s => s.SongName)
                .ThenBy(s => s.Writer)
                .ThenBy(s => s.Performer)
                .ToArray();

            var songXml = XmlConverter.Serialize(songsToExport, "Songs");

            return songXml;
        }
    }
}

/*Use the method provided in the project skeleton, which receives a song duration (in seconds). Export the songs which are above the given duration. For each song, export its name, performer full name, writer name, album producer and duration in format("c"). Sort the songs by their name (ascending), by writer (ascending) and by performer (ascending). 
*/
