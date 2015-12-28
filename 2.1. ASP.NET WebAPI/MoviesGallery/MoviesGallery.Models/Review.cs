using System;
using System.ComponentModel.DataAnnotations;

namespace MoviesGallery.Models
{
    public class Review
    {
        [Key]
        public int Id { get; set; }

        public string Content { get; set; }

        public DateTime DateOfCreation { get; set; }

        public int UserId { get; set; }

        public virtual User User { get; set; }

        public int MovieId { get; set; }

        public virtual Movie Movie { get; set; }
    }
}
