using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MoviesGallery.Models
{
    public class Movie
    {
        private ICollection<Actor> actors;
        private ICollection<Review> reviews; 

        public Movie()
        {
            this.actors = new HashSet<Actor>();
            this.reviews = new HashSet<Review>();
        }

        [Key]
        public int Id { get; set; }

        public string Title { get; set; }

        [Range(0,59)]
        public int Length { get; set; }

        [Range(1,10)]
        public int Ration { get; set; }

        public string Country { get; set; }

        public int GenreId { get; set; }

        public virtual Gender Gender { get; set; }

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
