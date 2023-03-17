using Domain.Entities.FlightAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class FlightConfiguration : IEntityTypeConfiguration<Flight>
{
    public void Configure(EntityTypeBuilder<Flight> builder)
    {
        builder.ToTable("Flight");

        builder.Property(x => x.Id)
            .HasDefaultValueSql("newsequentialid()");

        builder.Property(x => x.Origin)
            .HasMaxLength(256);

        builder.Property(x => x.Destination)
            .HasMaxLength(256);

        builder.HasIndex(x => new
        {
            x.Origin,
            x.Destination
        });
        
        builder.HasIndex(x => x.Destination);
    }
}
