namespace Tryitter.Data.Mappings;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Tryitter.Models;

public class UserMap : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(x => x.UserId);
        builder.Property(x => x.UserId)
            .ValueGeneratedOnAdd()
            .UseIdentityColumn();

        builder.Property(x => x.Name)
            .IsRequired()
            .HasColumnName("Name")
            .HasColumnType("NVARCHAR")
            .HasMaxLength(100);
        
        builder.Property(x => x.Module)
            .HasColumnName("Module")
            .HasColumnType("VARCHAR")
            .HasMaxLength(100);

        builder.Property(x => x.Status)
            .HasColumnName("Status")
            .HasColumnType("VARCHAR")
            .HasMaxLength(80);

        builder.Property(x => x.Password)
            .IsRequired()
            .HasColumnName("Password")
            .HasColumnType("VARCHAR")
            .HasMaxLength(100);

        builder.Property(x => x.Role)
            .IsRequired()
            .HasColumnName("Role")
            .HasColumnType("VARCHAR")
            .HasMaxLength(10)
            .HasDefaultValue("User");

        builder.Property(x => x.Username)
            .IsRequired()
            .HasColumnName("Username")
            .HasColumnType("VARCHAR")
            .HasMaxLength(80);

        builder.Property(x => x.Email)
            .IsRequired()
            .HasColumnName("Email")
            .HasColumnType("VARCHAR")
            .HasMaxLength(300);
        
        builder.HasIndex(x => x.Email, "IX_User_Email")
            .IsUnique();
        builder.HasIndex(x => x.Username, "IX_User_Username")
            .IsUnique();

    }
}
