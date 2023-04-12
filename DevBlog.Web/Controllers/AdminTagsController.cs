
using DevBlog.Library.Services;
using DevBlog.Web.Data;
using DevBlog.Web.Models.Domain;
using DevBlog.Web.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;


namespace DevBlog.Web.Controllers
{

    public class AdminTagsController : Controller
    {
        private readonly BlogDbContext blogDbContext;
        private readonly TagDataServices tagDataServices;
        public AdminTagsController(BlogDbContext blogDbContext,TagDataServices tagDataServices)
        {
            this.blogDbContext= blogDbContext;
            this.tagDataServices= tagDataServices;
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

            bool result = tagDataServices.AddTag(tag);
            //Context save to database
            return RedirectToAction("List");
        }

        [HttpGet]
        [ActionName("List")]
        //List or View Tag Data
        public IActionResult List() 
        {
            //use dbContext to read the tags
            var tags = tagDataServices.ListTags();
            return View(tags);
        }

        [HttpGet]
        //Get Tag Data
        public IActionResult Edit(Guid Id)
        {
            var tag = tagDataServices.GetTag(Id);
            if(tag!=null) 
            {
                return View(tag);
            }
            return View(null);
        }

        [HttpPost]
        //Edit Tag Data
        public IActionResult Edit(EditTagRequest editTagRequest)
        {
            if (tagDataServices.EditTag(editTagRequest))
            {
                return RedirectToAction("List");
            }

            // handle case when tag not found
            return NotFound();
        }


        //Delete Tag Data
        public IActionResult Delete(EditTagRequest editTagRequest)
        {
            var result = tagDataServices.DeleteTag(editTagRequest.Id);

            if(result)
            {
                return RedirectToAction("List");
            }
            return RedirectToAction("Edit", new {id = editTagRequest.Id});
        }




    }

}
