using DevBlog.Web.Data;
using DevBlog.Web.Models.Domain;
using DevBlog.Web.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevBlog.Library.Services
{
    public class TagDataServices
    {
        private readonly BlogDbContext blogDbContext;
        public TagDataServices(BlogDbContext blogDbContext)
        {
            this.blogDbContext = blogDbContext;
        }

        public bool AddTag(Tag tag)
        {
            try
            {
                blogDbContext.Tags.Add(tag);
                blogDbContext.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<Tag> ListTags()
        {
            return blogDbContext.Tags.ToList();
        }

        public EditTagRequest? GetTag(Guid Id)
        {
            var tag = blogDbContext.Tags.FirstOrDefault(x => x.Id == Id);
            if (tag != null)
            {
                var editTagRequest = new EditTagRequest
                {
                    Id = tag.Id,
                    Name = tag.Name,
                    DisplayName = tag.DisplayName,
                };
                return editTagRequest;
            }
            return null;
        }

        public bool EditTag(EditTagRequest editTagRequest)
        {
            var existingTag = blogDbContext.Tags.Find(editTagRequest.Id);
            if (existingTag != null)
            {
                existingTag.Name = editTagRequest.Name;
                existingTag.DisplayName = editTagRequest.DisplayName;

                blogDbContext.SaveChanges();
                return true;
            }

            return false;
        }

        public bool DeleteTag(Guid Id)
        {
            var tag = blogDbContext.Tags.Find(Id);
            if (tag != null)
            {
                blogDbContext.Tags.Remove(tag);
                blogDbContext.SaveChanges();
                return true;
            }
            return false;
        }
    }
}

