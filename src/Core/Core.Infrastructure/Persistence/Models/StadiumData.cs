﻿namespace BettingSystem.Infrastructure.Persistence.Models
{
    using System.Collections.Generic;
    using Application.Common.Mapping;
    using AutoMapper;

    internal class StadiumData :
        IMapFrom<Domain.Matches.Models.Stadium>,
        IMapTo<Domain.Matches.Models.Stadium>
    {
        public int Id { get; set; }

        public string Name { get; set; } = default!;

        public ImageData Image { get; set; } = default!;

        public ICollection<MatchData> Matches { get; } = new HashSet<MatchData>();

        public void Mapping(Profile mapper)
        {
            mapper
                .CreateMap<
                    StadiumData,
                    Domain.Matches.Models.Stadium>()
                .ReverseMap();
        }
    }
}
