using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProyectoPAW.Areas.Identity.Data;

namespace ProyectoPAW.Areas.Identity.Data;

public class UsersContext : IdentityDbContext<ApplicationUser>
{
    public UsersContext(DbContextOptions<UsersContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);

        builder.ApplyConfiguration(new ApplicationUsersEntityConfiguration());
    }
}

public class ApplicationUsersEntityConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.Property(u => u.Nombre).IsRequired().HasMaxLength(100);
        builder.Property(u => u.Apellidos).IsRequired().HasMaxLength(100);
        builder.Property(u => u.Cedula).IsRequired().HasMaxLength(20);
        builder.Property(u => u.Telefono).IsRequired().HasMaxLength(15);
    }
}

