using System;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class AccomodationUnitConfiguration : IEntityTypeConfiguration<AccomodationUnit>
{
    public void Configure(EntityTypeBuilder<AccomodationUnit> builder)
    {
        builder.HasKey(au => au.Id);
        builder.HasQueryFilter(au => !au.IsDeleted);
    }
}
