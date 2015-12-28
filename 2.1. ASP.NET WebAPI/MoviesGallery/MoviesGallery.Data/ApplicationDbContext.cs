using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using MoviesGallery.Models;

namespace MoviesGallery.Data
{

    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext()
            : base("MoviesGalleryConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public virtual DbSet<User> Users { get; set; }

        public virtual DbSet<Movie> Movies { get; set; }

        public virtual DbSet<Actor> Actors { get; set; }

        public virtual DbSet<Review> Reviews { get; set; }

        public virtual DbSet<Genre> Genr { get; set; }
    }

}
