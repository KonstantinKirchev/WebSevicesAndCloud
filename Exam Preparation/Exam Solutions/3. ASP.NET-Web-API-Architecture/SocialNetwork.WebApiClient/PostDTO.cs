namespace SocialNetwork.WebApiClient
{
    public class PostDTO // DTO - Data Transfer Object
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public string Author { get; set; }

        public int Likes { get; set; }
    }
}
