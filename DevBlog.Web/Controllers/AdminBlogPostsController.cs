using DevBlog.Library.Models.ViewModels;
using DevBlog.Library.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DevBlog.Web.Controllers
{
    public class AdminBlogPostsController : Controller
    {
        private readonly ITagServices tagServices;
        //private readonly IBlogPostRepository blogPostRepository;

        public AdminBlogPostsController(ITagServices tagServices)
        {
            this.tagServices = tagServices;
            //this.blogPostRepository = blogPostRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            // get tags from repository
            var tags = await tagServices.GetAllAsync();

            var model = new AddBlogPostRequest
            {
                Tags = tags.Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() })
            };

            return View(model);
        }
    }
}
