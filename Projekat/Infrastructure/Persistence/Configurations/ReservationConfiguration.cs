using System;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class ReservationConfiguration : IEntityTypeConfiguration<Reservation>
{
    public void Configure(EntityTypeBuilder<Reservation> builder)
    {
        builder.HasOne(r => r.AccomodationUnit);
        builder.HasOne(r => r.Arrangement);
        builder.HasOne(r => r.User)
            .WithMany(u=> u.Reservations)
            .OnDelete(DeleteBehavior.Restrict);
        builder.Ignore(r => r.Name);
        builder.Ignore(r => r.StartDate);
        builder.Ignore(r => r.Price);
    }
}
