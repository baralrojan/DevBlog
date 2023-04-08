// Class library project

using DevBlog.Web.Data;
using DevBlog.Web.Models.Domain;
using DevBlog.Web.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DevBlog.Library
{
    public class TagDataLibrary : Controller
    {
        private readonly BlogDbContext _blogDbContext;

        public TagDataLibrary(BlogDbContext blogDbContext)
        {
            _blogDbContext = blogDbContext;
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ActionName("Add")]
        public IActionResult AddTag(AddTagRequest addTagRequest)
        {
            if (ModelState.IsValid)
            {
                var tag = new Tag
                {
                    Name = addTagRequest.Name,
                    DisplayName = addTagRequest.DisplayName,
                };

                _blogDbContext.Tags.Add(tag);
                _blogDbContext.SaveChanges();

                return RedirectToAction("List");
            }

            return View(addTagRequest);
        }

        [HttpGet]
        [ActionName("List")]
        public IActionResult ListTags()
        {
            var tags = _blogDbContext.Tags.ToList();
            return View(tags);
        }

        [HttpGet]
        public IActionResult Edit(Guid id)
        {
            var tag = _blogDbContext.Tags.FirstOrDefault(x => x.Id == id);

            if (tag != null)
            {
                var editTagRequest = new EditTagRequest
                {
                    Id = tag.Id,
                    Name = tag.Name,
                    DisplayName = tag.DisplayName,
                };

                return View(editTagRequest);
            }

            return RedirectToAction("List");
        }

        [HttpPost]
        public IActionResult EditTag(EditTagRequest editTagRequest)
        {
            if (ModelState.IsValid)
            {
                var tag = new Tag
                {
                    Id = editTagRequest.Id,
                    Name = editTagRequest.Name,
                    DisplayName = editTagRequest.DisplayName,
                };

                var existingTag = _blogDbContext.Tags.Find(tag.Id);

                if (existingTag != null)
                {
                    existingTag.Name = tag.Name;
                    existingTag.DisplayName = tag.DisplayName;

                    _blogDbContext.SaveChanges();

                    return RedirectToAction("List");
                }

                return RedirectToAction("List");
            }

            return View(editTagRequest);
        }

        [HttpPost]
        public IActionResult Delete(EditTagRequest editTagRequest)
        {
            var tag = _blogDbContext.Tags.Find(editTagRequest.Id);

            if (tag != null)
            {
                _blogDbContext.Tags.Remove(tag);
                _blogDbContext.SaveChanges();

                return RedirectToAction("List");
            }

            return RedirectToAction("List");
        }
    }
}
