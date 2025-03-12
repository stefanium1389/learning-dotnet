using System;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class ArrangementConfiguration : IEntityTypeConfiguration<Arrangement>
{
    public void Configure(EntityTypeBuilder<Arrangement> builder)
    {
        builder.HasKey(a => a.Id);
        builder.HasOne(a => a.Accomodation);
        builder.HasOne(a => a.MeetingPlace);
        builder.HasQueryFilter(a => !a.IsDeleted);
        builder.Ignore(a => a.MaximumPassengers);
        builder.Ignore(a => a.FromPrice);
    }
}
