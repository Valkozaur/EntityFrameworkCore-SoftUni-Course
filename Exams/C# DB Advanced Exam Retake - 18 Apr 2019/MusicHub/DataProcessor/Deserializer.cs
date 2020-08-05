using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Internal;
using MusicHub.Data.Models;
using MusicHub.Data.Models.Enums;
using MusicHub.DataProcessor.ImportDtos;
using MusicHub.XmlFacade;
using Newtonsoft.Json;
using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

namespace MusicHub.DataProcessor
{
    using System;

    using Data;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data";

        private const string SuccessfullyImportedWriter 
            = "Imported {0}";
        private const string SuccessfullyImportedProducerWithPhone 
            = "Imported {0} with phone: {1} produces {2} albums";
        private const string SuccessfullyImportedProducerWithNoPhone
            = "Imported {0} with no phone number produces {1} albums";
        private const string SuccessfullyImportedSong 
            = "Imported {0} ({1} genre) with duration {2}";
        private const string SuccessfullyImportedPerformer
            = "Imported {0} ({1} songs)";

        public static string ImportWriters(MusicHubDbContext context, string jsonString)
        {
            var sb = new StringBuilder();

            var writerDtOs = JsonConvert.DeserializeObject<ImportWriterDtO[]>(jsonString);

            var writers = new List<Writer>();
            foreach (var writerDtO in writerDtOs)
            {
                if (!IsValid(writerDtO))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var writer = Mapper.Map<Writer>(writerDtO);
                writers.Add(writer);

                sb.AppendFormat(SuccessfullyImportedWriter, writer.Name);
                sb.AppendLine();
            }

            context.Writers.AddRange(writers);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportProducersAlbums(MusicHubDbContext context, string jsonString)
        {
            var sb = new StringBuilder();

            var producersDtOs = JsonConvert.DeserializeObject<ImportProducersDtO[]>(jsonString);

            var producers = new List<Producer>();
            foreach (var producerDtO in producersDtOs)
            {
                if (!IsValid(producerDtO))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var albumError = false;

                var albums = new HashSet<Album>();
                foreach (var albumDtO in producerDtO.Albums)
                {
                    bool dateIsValid = 
                        DateTime.TryParseExact(albumDtO.ReleaseDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _);

                    if (!IsValid(albumDtO) || !dateIsValid)
                    {
                        albumError = true;
                        break;
                    }

                    var album = Mapper.Map<Album>(albumDtO);

                    albums.Add(album);
                }

                if (albumError)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var producer = new Producer()
                {
                    Name = producerDtO.Name,
                    Pseudonym = producerDtO.Pseudonym,
                    PhoneNumber = producerDtO.PhoneNumber,
                    Albums = albums
                };

                producers.Add(producer);

                if (producer.PhoneNumber != null)
                {
                    sb.AppendFormat(SuccessfullyImportedProducerWithPhone, producer.Name, producer.PhoneNumber, producer.Albums.Count);
                }
                else
                {
                    sb.AppendFormat(SuccessfullyImportedProducerWithNoPhone, producer.Name, producer.Albums.Count);
                }

                sb.AppendLine();
            }

            context.AddRange(producers);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportSongs(MusicHubDbContext context, string xmlString)
        {
            var sb = new StringBuilder();

            var songDtOs = XmlConverter.Deserializer<ImportSongDtO>(xmlString, "Songs");

            var songs = new List<Song>();
            foreach (var songDtO in songDtOs)
            {
                var songIsValid = IsValid(songDtO);

                var createdOnIsValid 
                    = DateTime.TryParseExact(songDtO.CreatedOn, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _);

                var durationIsValid 
                    = TimeSpan.TryParseExact(songDtO.Duration, "c", CultureInfo.InvariantCulture, TimeSpanStyles.None, out _);

                var genreIsValid = Enum.TryParse(typeof(Genre), songDtO.Genre, out _);

                var writerExists = context.Writers.Any(w => w.Id == songDtO.WriterId);

                var albumExists = context.Albums.Any(a => a.Id == songDtO.AlbumId);

                if (!songIsValid || 
                    !createdOnIsValid || 
                    !durationIsValid || 
                    !genreIsValid ||
                    !writerExists ||
                    !albumExists)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var song = Mapper.Map<Song>(songDtO);

                songs.Add(song);
                sb.AppendFormat(SuccessfullyImportedSong, 
                    song.Name, 
                    song.Genre.ToString(), 
                    song.Duration.ToString());

                sb.AppendLine();
            }

            context.Songs.AddRange(songs);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportSongPerformers(MusicHubDbContext context, string xmlString)
        {
            var sb = new StringBuilder();

            var songDtOs = XmlConverter.Deserializer<ImportPerformerDtO>(xmlString, "Performers");

            //List of valid performers
            var performers = new List<Performer>();
            foreach (var performerDtO in songDtOs)
            {
                var performerIsValid = IsValid(performerDtO);

                if (!performerIsValid)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                //Create performer
                var performer = Mapper.Map<Performer>(performerDtO);

                //It should stay false if everything is correct
                var songListError = false;

                foreach (var performerSongDtO in performerDtO.Songs)
                {
                    //Checks if song exists in the database
                    var songExists = context.Songs.Any(s => s.Id == performerSongDtO.SongId);

                    if (!songExists)
                    {
                        songListError = true;
                        break;
                    }

                    var songPerformer = new SongPerformer()
                    {
                        SongId = performerSongDtO.SongId,
                        PerformerId = performer.Id
                    };

                    performer.Songs.Add(songPerformer);
                }

                if (songListError)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                //Add the performer to the list with valid performers
                performers.Add(performer);

                sb.AppendFormat(SuccessfullyImportedPerformer, 
                    performer.FirstName, 
                    performer.Songs.Count);
                sb.AppendLine();
            }

            //Saves the list of valid performers in the database
            context.Performers.AddRange(performers);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}