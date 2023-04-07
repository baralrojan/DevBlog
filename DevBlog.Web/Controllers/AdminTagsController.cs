using DevBlog.Web.Data;
using DevBlog.Web.Models.Domain;
using DevBlog.Web.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace DevBlog.Web.Controllers
{

    public class AdminTagsController : Controller
    {
        private readonly BlogDbContext blogDbContext;
        public AdminTagsController(BlogDbContext blogDbContext)
        {
            this.blogDbContext= blogDbContext;
        }
        [HttpGet]
        public IActionResult Add()
        {

            return View();
        }

        [HttpPost]
        [ActionName("Add")]
        //Add Tag Data
        public IActionResult Add(AddTagRequest addTagRequest)
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

            return RedirectToAction("List");
        }

        [HttpGet]
        [ActionName("List")]
        //List or View Tag Data
        public IActionResult List() 
        {
            //use dbContext to read the tags
            var tags = blogDbContext.Tags.ToList();
            return View(tags);
        }

        [HttpGet]
        //Get Tag Data
        public IActionResult Edit(Guid Id)
        {
            var tag = blogDbContext.Tags.FirstOrDefault(x => x.Id == Id);
            if(tag!=null) 
            {
                var editTagRequest = new EditTagRequest
                {
                    Id = tag.Id,
                    Name = tag.Name,
                    DisplayName= tag.DisplayName,
                };
                return View(editTagRequest);
            }
            return View(null);
        }

        [HttpPost]
        //Edit Tag Data
        public IActionResult Edit(EditTagRequest editTagRequest)
        {
            var tag = new Tag
            {
                Id = editTagRequest.Id,
                Name = editTagRequest.Name,
                DisplayName = editTagRequest.DisplayName,
            };
            var existingTag = blogDbContext.Tags.Find(tag.Id);
            if(existingTag!=null) 
            {
                existingTag.Name = tag.Name;
                existingTag.DisplayName = tag.DisplayName;

                //save changes
                blogDbContext.SaveChanges();
                return RedirectToAction("List");
            }
            return RedirectToAction("Edit", new {id = editTagRequest.Id});
        }

        //Delete Tag Data
        public IActionResult Delete(EditTagRequest editTagRequest)
        {
            var tag = blogDbContext.Tags.Find(editTagRequest.Id);

            if(tag!=null)
            {
                blogDbContext.Tags.Remove(tag);
                blogDbContext.SaveChanges() ;

                return RedirectToAction("List");
            }
            return RedirectToAction("Edit", new {id = editTagRequest.Id});
        }
    }

}
