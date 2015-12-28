using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace MoviesGallery.Models
{
    public class User : IdentityUser
    {
        private ICollection<Movie> movies;
        private ICollection<Actor> actors;
        private ICollection<Review> reviews;

        public User()
        {
            this.movies = new HashSet<Movie>();
            this.actors = new HashSet<Actor>();
            this.reviews = new HashSet<Review>();
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }

        public string PersonalPage { get; set; }

        public Gender Gender { get; set; }

        public DateTime BirthDate { get; set; }

        public virtual ICollection<Movie> Movies
        {
            get { return this.movies; }
            set { this.movies = value; }
        }

        public virtual ICollection<Actor> Actors
        {
            get { return this.actors; }
            set { this.actors = value; }
        }

        public virtual ICollection<Review> Reviews
        {
            get { return this.reviews; }
            set { this.reviews = value; }
        }

    }
}
