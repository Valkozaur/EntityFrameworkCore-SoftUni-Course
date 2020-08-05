using System;
using System.Globalization;
using System.Linq;
using MusicHub.Data.Models;
using MusicHub.Data.Models.Enums;
using MusicHub.DataProcessor.ExportDtOs;
using MusicHub.DataProcessor.ImportDtos;

namespace MusicHub
{
    using AutoMapper;

    public class MusicHubProfile : Profile
    {
        // Configure your AutoMapper here if you wish to use it. If not, DO NOT DELETE THIS CLASS
        public MusicHubProfile()
        {
            //Import Writer
            CreateMap<ImportWriterDtO, Writer>();

            //Import Albums
            CreateMap<ImportAlbumDtO, Album>()
                .ForMember(x => x.ReleaseDate,
                    y => y.MapFrom(x =>
                        DateTime.ParseExact(x.ReleaseDate, "dd/MM/yyyy", CultureInfo.InvariantCulture)));

            //Import Producers
            CreateMap<ImportProducersDtO, Producer>();

            //Import Song
            CreateMap<ImportSongDtO, Song>()
                .ForMember(x => x.CreatedOn,
                    y => y.MapFrom(x => DateTime.ParseExact(x.CreatedOn, "dd/MM/yyyy", CultureInfo.InvariantCulture,
                        DateTimeStyles.None)))
                .ForMember(x => x.Duration,
                    y => y.MapFrom(x =>
                        TimeSpan.ParseExact(x.Duration, "c", CultureInfo.InvariantCulture, TimeSpanStyles.None)))
                .ForMember(x => x.Genre,
                    y => y.MapFrom(x => Enum.Parse<Genre>(x.Genre)));

            //Import Performer
            CreateMap<ImportPerformerDtO, Performer>()
                .ForMember(x => x.Songs, y => y.Ignore());

            //Export Song
            CreateMap<Song, ExportSongDtO>()
                .ForMember(x => x.SongName,
                    y => y.MapFrom(x => x.Name))
                .ForMember(x => x.Writer,
                    y => y.MapFrom(x => x.Writer.Name))
                .ForMember(x => x.AlbumProducer,
                    y => y.MapFrom(x => x.Album.Producer.Name))
                .ForMember(x => x.Performer,
                    y => y.MapFrom(x => x.SongPerformers.First().Performer.FirstName + " " + x.SongPerformers.First().Performer.LastName))
                .ForMember(x => x.Duration,
                    y => y.MapFrom(x => x.Duration.ToString("c")));
        }
    }
}
