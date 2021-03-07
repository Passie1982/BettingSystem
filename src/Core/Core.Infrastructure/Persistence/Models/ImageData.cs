﻿namespace BettingSystem.Infrastructure.Persistence.Models
{
    using Application.Common.Mapping;
    using AutoMapper;

    internal class ImageData :
        IMapFrom<Domain.Matches.Models.Image>,
        IMapTo<Domain.Matches.Models.Image>
    {
        public byte[] OriginalContent { get; set; } = default!;

        public byte[] ThumbnailContent { get; set; } = default!;

        public void Mapping(Profile mapper)
        {
            mapper
                .CreateMap<
                    ImageData,
                    Domain.Matches.Models.Image>()
                .ReverseMap();
        }
    }
}
