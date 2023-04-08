namespace DevBlog.Web.Models.Domain
{
    /*many to many relationship between Tag and BlogPost*/
    public class Tag
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? DisplayName { get; set; }
        /*many to many relationship between Tag and BlogPost*/
        public ICollection<BlogPost>? BlogPosts { get; set; }
    }
}
