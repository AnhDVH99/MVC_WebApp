using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ASP.NET_Core_MVC_Piacom.Data
{
    public class AuthDbContext : IdentityDbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Seed Roles(User, Admin, SuperAdmin)
            var adminRoleId = "c7c2a664-8cc8-49ff-bb50-c24d0fe79eef";
            var superAdminRoleId = "0ad02305-2e6b-43cf-bd6a-eb294cd190c6";
            var userRoleId = "49e52ab4-24e8-48dd-8c2d-f6f86603ef45";

            var roles = new List<IdentityRole>()
                {
                new IdentityRole
                {
                    Name = "Admin",
                    NormalizedName = "Admin",
                    Id = adminRoleId,
                    ConcurrencyStamp = adminRoleId
                },
                new IdentityRole
                {
                    Name = "SuperAdmin",
                    NormalizedName = "SuperAdmin",
                    Id = superAdminRoleId,
                    ConcurrencyStamp = superAdminRoleId
                },
                new IdentityRole
                {
                    Name = "User",
                    NormalizedName = "User",
                    Id = userRoleId,
                    ConcurrencyStamp = userRoleId
                },

            };
            builder.Entity<IdentityRole>().HasData(roles);
            // Seed SuperAdminUser
            var superAdminUserId = "b6c11373-3183-445b-b8f6-e0ad8215dcca";
            var superAdminUser = new IdentityUser
            {
                UserName = "superadmin@piacom.com",
                Email = "superadmin@piacom.com",
                NormalizedUserName = "superadmin@piacom.com".ToUpper(),
                NormalizedEmail = "superadmin@piacom.com".ToUpper(),
                Id = superAdminUserId
            };
            superAdminUser.PasswordHash = new PasswordHasher<IdentityUser>()
                .HashPassword(superAdminUser, "Superadmin@123");

            builder.Entity<IdentityUser>().HasData(superAdminUser);


            // Add all roles to SuperAdminUser
            var superAdminRoles = new List<IdentityUserRole<string>>
            {
                new IdentityUserRole<string>
                {
                    RoleId = adminRoleId,
                    UserId = superAdminUserId
                },
                new IdentityUserRole<string>
                {
                    RoleId = superAdminRoleId,
                    UserId = superAdminUserId
                },
                new IdentityUserRole<string>
                {
                    RoleId = userRoleId,
                    UserId = superAdminUserId
                }
            };
            builder.Entity<IdentityUserRole<string>>().HasData(superAdminRoles);

        }
    }
}
