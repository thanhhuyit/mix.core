﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mix.Database.Entities.Cms.v2;
using Mix.Database.EntityConfigurations.v2.POSTGRES.Base;
using System;

namespace Mix.Database.EntityConfigurations.v2.POSTGRES
{
    public class MixDataConfiguration : PostgresEntityBaseConfiguration<MixData, Guid>
    {
        public override void Configure(EntityTypeBuilder<MixData> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.MixDatabaseName)
              .HasColumnType($"{_config.NString}{_config.MediumLength}")
              .HasCharSet(_config.CharSet)
              .UseCollation(_config.DatabaseCollation);
        }
    }
}