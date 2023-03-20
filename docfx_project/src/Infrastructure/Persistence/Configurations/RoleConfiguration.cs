using System;
using Application.Common.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
{
    public void Configure(EntityTypeBuilder<IdentityRole> builder)
    {
        builder.HasData
        (
            new IdentityRole(AuthConstants.UserRoleName) { Id = AuthConstants.UserRoleId },
            new IdentityRole(AuthConstants.ModeratorRoleName) { Id = AuthConstants.ModeratorRoleId }
        );
    }
}
