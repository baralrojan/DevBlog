using DevBlog.Web.Data;
using DevBlog.Web.Models.Domain;
using DevBlog.Web.Models.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace DevBlog
{
    public class DevBlogLibrary
    {
        private readonly BlogDbContext blogDbContext;

        public DevBlogLibrary(BlogDbContext blogDbContext)
        {
            this.blogDbContext = blogDbContext;
        }

        public int AddTag(AddTagRequest addTagRequest)
        {
            var tag = new Tag
            {
                Name = addTagRequest.Name,
                DisplayName = addTagRequest.DisplayName
            };

            blogDbContext.Tags.Add(tag);
            blogDbContext.SaveChanges();
            return blogDbContext.SaveChanges();
        }

    
    }
}
