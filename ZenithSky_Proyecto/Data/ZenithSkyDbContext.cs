using System.Reflection.Emit;
using ZenithSky_Proyecto.Domin;
using Microsoft.EntityFrameworkCore;

namespace ZenithSky_Proyecto.Data
{
  public class ZenithSkyDbContext : DbContext
    {
        public ZenithSkyDbContext(DbContextOptions<ZenithSkyDbContext> options) : base(options) { }

        public DbSet<Airport> Airports => Set<Airport>();
        public DbSet<Airline> Airlines => Set<Airline>();
        public DbSet<AirplaneType> AirplaneTypes => Set<AirplaneType>();
        public DbSet<Airplane> Airplanes => Set<Airplane>();

        protected override void OnModelCreating(ModelBuilder mb)
        {
            mb.Entity<Airport>(e =>
            {
                e.ToTable("airport");
                e.HasKey(x => x.AirportId);
                e.Property(x => x.AirportId).HasColumnName("airport_id").ValueGeneratedOnAdd();
                e.Property(x => x.Iata).HasColumnName("iata").HasColumnType("char(3)").IsRequired();
                e.Property(x => x.Icao).HasColumnName("icao").HasColumnType("char(4)").IsRequired();
                e.Property(x => x.Name).HasColumnName("name").HasMaxLength(100).IsRequired();
            });

            mb.Entity<Airline>(e =>
            {
                e.ToTable("airline");
                e.HasKey(x => x.AirlineId);
                e.Property(x => x.AirlineId).HasColumnName("airline_id").ValueGeneratedOnAdd();
                e.Property(x => x.Iata).HasColumnName("iata").HasColumnType("char(2)").IsRequired();
                e.Property(x => x.AirlineName).HasColumnName("airlinename").HasMaxLength(100).IsRequired();
                e.Property(x => x.BaseAirportId).HasColumnName("base_airport");
                e.HasOne(x => x.Airport)
                 .WithMany(a => a.Airlines)
                 .HasForeignKey(x => x.BaseAirportId)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            mb.Entity<AirplaneType>(e =>
            {
                e.ToTable("airplane_type");
                e.HasKey(x => x.TypeId);
                e.Property(x => x.TypeId).HasColumnName("type_id").ValueGeneratedOnAdd();
                e.Property(x => x.Identifier).HasColumnName("identifier").HasMaxLength(50).IsRequired();
                e.Property(x => x.Description).HasColumnName("description").HasMaxLength(250);
            });

            mb.Entity<Airplane>(e =>
            {
                e.ToTable("airplane");
                e.HasKey(x => x.AirplaneId);
                e.Property(x => x.AirplaneId).HasColumnName("airplane_id").ValueGeneratedOnAdd();
                e.Property(x => x.Capacity).HasColumnName("capacity");
                e.Property(x => x.TypeId).HasColumnName("type_id");
                e.Property(x => x.AirlineId).HasColumnName("airline_id");
                e.HasOne(x => x.AirplaneType)
                 .WithMany(t => t.Airplanes)
                 .HasForeignKey(x => x.TypeId)
                 .OnDelete(DeleteBehavior.Restrict);
                e.HasOne(x => x.Airline)
                 .WithMany(a => a.Airplanes)
                 .HasForeignKey(x => x.AirlineId)
                 .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }

}
