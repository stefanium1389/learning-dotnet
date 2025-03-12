using System;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class AccomodationConfiguration : IEntityTypeConfiguration<Accomodation>
{
    public void Configure(EntityTypeBuilder<Accomodation> builder)
    {
        builder.HasKey(a => a.Id);
        builder.HasMany(a => a.AccomodationUnits);
        builder.HasQueryFilter(a => !a.IsDeleted);
        builder.Ignore(a => a.UnitCount);
        builder.Ignore(a => a.UnbookedUnits);
    }
}
