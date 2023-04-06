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
        public IActionResult List() 
        {
            //use dbContext to read the tags
            var tags = blogDbContext.Tags.ToList();
            return View(tags);
        }
    }
}
