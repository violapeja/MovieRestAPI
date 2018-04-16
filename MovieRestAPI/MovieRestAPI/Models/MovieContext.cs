namespace MovieRestAPI.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class MovieContext : DbContext
    {
        public MovieContext()
            : base("name=MovieContext")
        {
        }

        public virtual DbSet<Actor> Actors { get; set; }
        public virtual DbSet<MovieActor> MovieActors { get; set; }
        public virtual DbSet<Movie> Movies { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Actor>()
                .Property(e => e.FirstName)
                .IsUnicode(false);

            modelBuilder.Entity<Actor>()
                .Property(e => e.LastName)
                .IsUnicode(false);

            modelBuilder.Entity<Actor>()
                .HasMany(e => e.MovieActors)
                .WithRequired(e => e.Actor)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Movie>()
                .Property(e => e.Title)
                .IsUnicode(false);

            modelBuilder.Entity<Movie>()
                .Property(e => e.Year)
                .IsUnicode(false);

            modelBuilder.Entity<Movie>()
                .Property(e => e.Director)
                .IsUnicode(false);

            modelBuilder.Entity<Movie>()
                .HasMany(e => e.MovieActors)
                .WithRequired(e => e.Movie)
                .WillCascadeOnDelete(false);
        }
    }
}
