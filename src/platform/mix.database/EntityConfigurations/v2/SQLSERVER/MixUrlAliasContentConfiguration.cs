﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mix.Database.Entities.Cms.v2;
using Mix.Database.EntityConfigurations.v2.SQLSERVER.Base;
using Mix.Shared.Enums;

namespace Mix.Database.EntityConfigurations.v2.SQLSERVER
{
    public class MixUrlAliasContentConfiguration : SqlServerEntityBaseConfiguration<MixUrlAliasContent, int>
    {
        public override void Configure(EntityTypeBuilder<MixUrlAliasContent> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.SourceId)
                .HasColumnType($"{_config.NString}{_config.SmallLength}")
                .HasCharSet(_config.CharSet)
                .UseCollation(_config.DatabaseCollation);

            builder.Property(e => e.Alias)
                .IsRequired()
                .HasColumnType($"{_config.NString}{_config.MediumLength}")
                .HasCharSet(_config.CharSet)
                .UseCollation(_config.DatabaseCollation);

            builder.Property(e => e.Type)
                .HasConversion(new EnumToStringConverter<MixUrlAliasType>())
                .HasColumnType($"{_config.NString}{_config.SmallLength}")
                .HasCharSet(_config.CharSet)
                .UseCollation(_config.DatabaseCollation);
        }
    }
}