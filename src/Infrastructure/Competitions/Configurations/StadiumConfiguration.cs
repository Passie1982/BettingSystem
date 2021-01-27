﻿namespace BettingSystem.Infrastructure.Competitions.Configurations
{
    using Common.Persistence.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using static Domain.Common.Models.ModelConstants.Common;

    internal class StadiumConfiguration : IEntityTypeConfiguration<StadiumData>
    {
        public void Configure(EntityTypeBuilder<StadiumData> builder)
        {
            builder
                .HasKey(s => s.Id);

            builder
                .Property(s => s.Name)
                .HasMaxLength(MaxNameLength)
                .IsRequired();

            builder
                .OwnsOne(s => s.Image, i =>
                {
                    i.WithOwner();

                    i.Property(img => img.OriginalContent).IsRequired();
                    i.Property(img => img.ThumbnailContent).IsRequired();
                });
        }
    }
}
