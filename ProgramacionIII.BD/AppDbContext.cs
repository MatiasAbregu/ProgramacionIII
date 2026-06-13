using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ProgramacionIII.BD
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}

/*var adminGUID = "b3d11b32-159b-4375-9276-2e9b88497fa1";
var adminuserGUID = "a1b22c33-34bd-4a7b-8162-1a8b99327fc3";
            
modelBuilder.Entity<IdentityRole>().HasData(
    new IdentityRole
    {
        Id = adminGUID,
        Name = "Administrador",
        NormalizedName = "ADMINISTRADOR",
        ConcurrencyStamp = "1"
    },
    new IdentityRole{
        Name = "Preceptor",
        NormalizedName = "PRECEPTOR",
        ConcurrencyStamp = "2"
    });

var adminUser = new IdentityUser()
{
    Id = adminuserGUID,
    UserName = "Superadministrador", 
    NormalizedUserName = "SUPERADMINISTRADOR",
    Email = "",
    NormalizedEmail = "",
    EmailConfirmed = true,
    SecurityStamp = "7de63f21-7299-4c12-9c16-5bfa816e8125"
};
            
adminUser.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(adminUser, "!ISPT-AQUILES-#2027#");
modelBuilder.Entity<IdentityUser>().HasData(adminUser);
            
modelBuilder.Entity<IdentityUserRole<string>>().HasData(
    new IdentityUserRole<string>
    {
        UserId = adminuserGUID,
        RoleId = adminGUID
    }
);*/