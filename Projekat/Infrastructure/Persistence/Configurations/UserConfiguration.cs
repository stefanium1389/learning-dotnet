using System;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);
        builder.HasIndex(u => u.Username)
            .IsUnique();

        builder.HasMany(u => u.CreatedArrangements);
        
        builder.HasMany(u => u.Reservations)
            .WithOne(r => r.User)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
