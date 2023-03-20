using Infrastructure.DbEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class FlightConfiguration : IEntityTypeConfiguration<FlightDbModel>
{
    public void Configure(EntityTypeBuilder<FlightDbModel> builder)
    {
        builder.ToTable("Flight");

        builder.Property(x => x.Id)
            .HasDefaultValueSql("newsequentialid()");

        builder.Property(x => x.Origin)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(x => x.Destination)
            .HasMaxLength(255)
            .IsRequired();

        builder.HasIndex(x => new
        {
            x.Origin,
            x.Destination
        });
        
        builder.HasIndex(x => x.Destination);
    }
}
