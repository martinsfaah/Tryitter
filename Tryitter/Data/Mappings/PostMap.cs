namespace Tryitter.Data.Mappings;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Tryitter.Models;

public class PostMap : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        builder.ToTable("Posts");

        builder.HasKey(x => x.PostId);
        builder.Property(x => x.PostId)
            .ValueGeneratedOnAdd()
            .UseIdentityColumn();

        builder.Property(x => x.Content)
            .HasColumnName("Content")
            .HasColumnType("NVARCHAR")
            .HasMaxLength(300);

        builder.Property(x => x.ImageUrl)
            .HasColumnName("ImageUrl")
            .HasColumnType("VARCHAR");
        
        builder.Property(x => x.ContentType)
            .HasColumnName("ContentType")
            .HasColumnType("VARCHAR")
            .HasMaxLength(10);

        builder.HasOne(x => x.User)
            .WithMany(x => x.Posts)
            .HasForeignKey("UserId")
            .HasConstraintName("FK_User_Post")
            .OnDelete(DeleteBehavior.Cascade);
    }
}
