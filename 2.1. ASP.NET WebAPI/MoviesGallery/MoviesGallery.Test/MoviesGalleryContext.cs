namespace MoviesGallery.Test
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using MoviesGallery.Models;

    public class MoviesGalleryContext : DbContext
    {
        
        public MoviesGalleryContext()
            : base("name=MoviesGalleryContext")
        {
        }

        

        public virtual DbSet<Movie> Movies { get; set; }

        public virtual DbSet<Actor> Actors { get; set; }
    }

    
}