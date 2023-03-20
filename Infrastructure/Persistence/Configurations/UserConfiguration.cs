using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<IdentityUser>
{
    
    public void Configure(EntityTypeBuilder<IdentityUser> builder)
    {
        var hasher = new PasswordHasher<IdentityUser>();
        
        var user = new IdentityUser("someuser");
        user.Id = "82584756-126d-4382-b8a2-e8b439b630ad";
        user.PasswordHash = hasher.HashPassword(user, "someuser");
        user.NormalizedUserName = "SOMEUSER";
        
        var moderator = new IdentityUser("somemoderator");
        moderator.Id = "6f5768da-b384-4a79-abab-bc82cec127d9";
        moderator.PasswordHash = hasher.HashPassword(moderator, "somemoderator");
        moderator.NormalizedUserName = "SOMEMODERATOR";
        
        builder.HasData(user, moderator);
    }
}
