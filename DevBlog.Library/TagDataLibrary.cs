using DevBlog.Web.Data;
using DevBlog.Web.Models.Domain;
using DevBlog.Web.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace DevBlog.Web.Controllers
{

    public class TagDataLibrary : Controller
    {
        private readonly BlogDbContext blogDbContext;
        public TagDataLibrary(BlogDbContext blogDbContext)
        {
            this.blogDbContext = blogDbContext;
        }
        [HttpGet]
      

        [HttpPost]
        [ActionName("Add")]
        //Add Tag Data
        public Tag Add(AddTagRequest addTagRequest)
        {
            //Mapping AddTagRequest to Tag domain model
            var tag = new Tag
            {
                Name = addTagRequest.Name,
                DisplayName = addTagRequest.DisplayName,
            };

            blogDbContext.Tags.Add(tag);
            //Context save to database
            blogDbContext.SaveChanges();
            return tag;
        }

        [HttpPost]
        //Edit Tag Data
        public Tag Edit(EditTagRequest editTagRequest)
        {
            var tag = new Tag
            {
                Id = editTagRequest.Id,
                Name = editTagRequest.Name,
                DisplayName = editTagRequest.DisplayName,
            };
            var existingTag = blogDbContext.Tags.Find(tag.Id);
            if (existingTag != null)
            {
                existingTag.Name = tag.Name;
                existingTag.DisplayName = tag.DisplayName;

                //save changes
                blogDbContext.SaveChanges();
                return tag;
            }
            return tag;
        }

        [HttpGet]
        //Get Tag Data
        public bool Edit(Guid Id)
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
                return true;
            }
            return false;
        }

        //Delete Tag Data
        public bool Delete(EditTagRequest editTagRequest)
        {
            var tag = blogDbContext.Tags.Find(editTagRequest.Id);

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


