using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence;

public sealed class RepositoryDbContext : DbContext
{
    public RepositoryDbContext(DbContextOptions options) : base(options)
    {
        ChangeTracker.LazyLoadingEnabled = false;
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder) 
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(RepositoryDbContext).Assembly);
    }

    public DbSet<User> Users {get; set;}
    public DbSet<Accomodation> Accomodations {get; set;}
    public DbSet<Address> Addresses {get; set;}
    public DbSet<AccomodationUnit> AccomodationUnits {get; set;}
    public DbSet<Arrangement> Arrangements {get; set;}
    public DbSet<Comment> Comments {get; set;}
    public DbSet<MeetingPlace> MeetingPlaces {get; set;}
    public DbSet<Reservation> Reservations {get; set;}
}
