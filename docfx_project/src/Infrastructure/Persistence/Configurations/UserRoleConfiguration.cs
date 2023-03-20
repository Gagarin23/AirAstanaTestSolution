using Application.Common.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class UserRoleConfiguration : IEntityTypeConfiguration<IdentityUserRole<string>>
{
    public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
    {
        builder.HasData
        (
            new IdentityUserRole<string>()
            {
                RoleId = AuthConstants.UserRoleId,
                UserId = "82584756-126d-4382-b8a2-e8b439b630ad" // someuser
            },
            new IdentityUserRole<string>()
            {
                RoleId = AuthConstants.ModeratorRoleId,
                UserId = "6f5768da-b384-4a79-abab-bc82cec127d9" // somemoderator
            }
        );
    }
}
