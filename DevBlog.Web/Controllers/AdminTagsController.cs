
using DevBlog.Library.Services;
using DevBlog.Web.Data;
using DevBlog.Web.Models.Domain;
using DevBlog.Web.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;


namespace DevBlog.Web.Controllers
{

    public class AdminTagsController : Controller
    {
        private readonly ITagServices tagDataServices;
        public AdminTagsController(ITagServices tagDataServices)
        {
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
        public async Task<IActionResult> Add(AddTagRequest addTagRequest)
        {
            //Mapping AddTagRequest to Tag domain model
            var tag = new Tag
            {
                Name = addTagRequest.Name,
                DisplayName = addTagRequest.DisplayName,
            };

            await tagDataServices.AddAsync(tag);         
            //Context save to database
            return RedirectToAction("List");
        }

        [HttpGet]
        [ActionName("List")]
        //List or View Tag Data
        public async Task<IActionResult> List() 
        {
            //use dbContext to read the tags
            var tags = await tagDataServices.GetAllAsync();
            return View(tags);
        }

        [HttpGet]
        //Get Tag Data
        public async Task<IActionResult> Edit(Guid Id)
        {
            var tag = await tagDataServices.GetAsync(Id);
            if(tag!=null) 
            {
                var editTagRequest = new EditTagRequest
                {
                    Id = tag.Id,
                    Name = tag.Name,
                    DisplayName = tag.DisplayName
                };
                return View(editTagRequest);
            }
            return View(null);
        }

        [HttpPost]
        //Edit Tag Data
        public async Task<IActionResult> Edit(EditTagRequest editTagRequest)
        {
            var tag = new Tag
            {
                Id = editTagRequest.Id,
                Name = editTagRequest.Name,
                DisplayName = editTagRequest.DisplayName
            };
            var updatedTag = await tagDataServices.UpdateAsync(tag);
            if(updatedTag!=null)
            {
                //Show Success Message
                return RedirectToAction("List");
            }
            else
            {
                // Show Error Message
            }

            return RedirectToAction("Edit", new { id = editTagRequest.Id});
        }


        //Delete Tag Data
        public async Task<IActionResult> Delete(EditTagRequest editTagRequest)
        {
            var deletedTag = await tagDataServices.DeleteAsync(editTagRequest.Id);
            if(deletedTag!=null)
            {
                //Show Success Message
                return RedirectToAction("List");
            }

            //Show Error Message
            return RedirectToAction("Edit", new {id = editTagRequest.Id});
        }




    }

}
