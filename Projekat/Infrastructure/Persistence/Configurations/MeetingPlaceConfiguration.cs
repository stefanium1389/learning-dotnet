using System;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class MeetingPlaceConfiguration : IEntityTypeConfiguration<MeetingPlace>
{
    public void Configure(EntityTypeBuilder<MeetingPlace> builder)
    {
        builder.HasOne(mp => mp.Address);
    }
}
