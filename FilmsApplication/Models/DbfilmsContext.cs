using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace FilmsApplication.Models;

public partial class DbfilmsContext : DbContext
{
    public DbfilmsContext()
    {
    }

    public DbfilmsContext(DbContextOptions<DbfilmsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Actor> Actors { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<Director> Directors { get; set; }

    public virtual DbSet<Film> Films { get; set; }

    public virtual DbSet<FilmActor> FilmActors { get; set; }

    public virtual DbSet<FilmDirector> FilmDirectors { get; set; }

    public virtual DbSet<FilmGenre> FilmGenres { get; set; }

    public virtual DbSet<Genre> Genres { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=DESKTOP-FQ6KEJ3\\SQLEXPRESS;Database=DBFilms; Trusted_Connection=True; TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Actor>(entity =>
        {
            entity.ToTable("Actor");

            entity.Property(e => e.ActorBirthDay).HasColumnType("date");
            entity.Property(e => e.ActorDeathDay).HasColumnType("date");
            entity.Property(e => e.ActorName).HasMaxLength(50);

            entity.HasOne(d => d.ActorCountry).WithMany(p => p.Actors)
                .HasForeignKey(d => d.ActorCountryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Actor_Country");
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.ToTable("Country");

            entity.Property(e => e.CountryName).HasMaxLength(50);
        });

        modelBuilder.Entity<Director>(entity =>
        {
            entity.ToTable("Director");

            entity.Property(e => e.DirectorBirthDay).HasColumnType("date");
            entity.Property(e => e.DirectorDeathDay).HasColumnType("date");
            entity.Property(e => e.DirectorName).HasMaxLength(50);
        });

        modelBuilder.Entity<Film>(entity =>
        {
            entity.HasKey(e => e.FilmId).HasName("PK_Films");

            entity.ToTable("Film");

            entity.Property(e => e.FilmDateRelease).HasColumnType("date");
            entity.Property(e => e.FilmName).HasMaxLength(100);
        });

        modelBuilder.Entity<FilmActor>(entity =>
        {
            entity.ToTable("FilmActor");

            entity.HasOne(d => d.Actor).WithMany(p => p.FilmActors)
                .HasForeignKey(d => d.ActorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FilmActor_Actor");

            entity.HasOne(d => d.Film).WithMany(p => p.FilmActors)
                .HasForeignKey(d => d.FilmId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FilmActor_Film");
        });

        modelBuilder.Entity<FilmDirector>(entity =>
        {
            entity.ToTable("FilmDirector");

            entity.HasOne(d => d.Director).WithMany(p => p.FilmDirectors)
                .HasForeignKey(d => d.DirectorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FilmDirector_Director");

            entity.HasOne(d => d.Film).WithMany(p => p.FilmDirectors)
                .HasForeignKey(d => d.FilmId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Film_Director");
        });

        modelBuilder.Entity<FilmGenre>(entity =>
        {
            entity.ToTable("FilmGenre");

            entity.HasOne(d => d.Film).WithMany(p => p.FilmGenres)
                .HasForeignKey(d => d.FilmId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FilmGenre");

            entity.HasOne(d => d.Genre).WithMany(p => p.FilmGenres)
                .HasForeignKey(d => d.GenreId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FilmGenre_Genre");
        });

        modelBuilder.Entity<Genre>(entity =>
        {
            entity.ToTable("Genre");

            entity.Property(e => e.GenreName).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
