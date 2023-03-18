using Infrastructure.Constants;
using Infrastructure.DbEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class FlightStatusConfiguration : IEntityTypeConfiguration<FlightStatusDbModel>
{
    public void Configure(EntityTypeBuilder<FlightStatusDbModel> builder)
    {
        builder.ToTable("FlightStatus");
        
        builder.Property(x => x.Id)
            .HasDefaultValueSql("newsequentialid()");

        builder.Property(x => x.Name)
            .HasMaxLength(255)
            .IsRequired();

        builder.HasIndex(x => x.Name)
            .IsUnique();

        builder.HasData
        (
            new FlightStatusDbModel() { Id = FlightStatusConstants.InTimeId, Name = "Без задержек" },
            new FlightStatusDbModel() { Id = FlightStatusConstants.DelayedId, Name = "Задержка" },
            new FlightStatusDbModel() { Id = FlightStatusConstants.CancelledId, Name = "Отменён" }
        );
    }
}
