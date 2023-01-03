using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieApi.Domain.Entities;

namespace MovieApi.Data.Configuration;

public class MovieConfiguration : IEntityTypeConfiguration<Movie>
{
    public void Configure(EntityTypeBuilder<Movie> builder)
    {
        builder.ToTable("Movies");
        builder.HasKey(m => m.Id);
        builder.Property(m => m.Id).ValueGeneratedOnAdd();
        builder.Property(m => m.Summary).HasMaxLength(500).IsRequired();
        builder.Property(m => m.Title).HasMaxLength(100).IsRequired();
        builder.Property(m => m.ReleaseDate).IsRequired();
        builder.Property(m => m.OffTheatersDate).IsRequired();
        builder.Property(m => m.PosterUri).HasMaxLength(150).IsRequired();
        builder.Property(m => m.Room).IsRequired();
    }
}