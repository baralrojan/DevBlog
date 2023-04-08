using DevBlog.Web.Controllers;
using DevBlog.Web.Data;
using DevBlog.Web.Models.Domain;
using DevBlog.Web.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DevBlog.Web.Tests.Controllers
{
    [TestClass]
    public class AdminTagsControllerTests
    {
        private BlogDbContext _dbContext;
        private AdminTagsController _controller;

        [TestInitialize]
        public void Initialize()
        {
            var options = new DbContextOptionsBuilder<BlogDbContext>()
                .UseInMemoryDatabase(databaseName: "test_database")
                .Options;

            _dbContext = new BlogDbContext(options);
            _controller = new AdminTagsController(_dbContext);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _dbContext.Dispose();
        }

        [TestMethod]
        public void Add_ReturnsViewResult()
        {
            // Act
            var result = _controller.Add() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Add_SavesTagToDatabase()
        {
            // Arrange
            var request = new AddTagRequest
            {
                Name = "test",
                DisplayName = "Test"
            };

            // Act
            var result = _controller.Add(request) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            var tag = _dbContext.Tags.FirstOrDefault(t => t.Name == request.Name);
            Assert.IsNotNull(tag);
        }

        [TestMethod]
        public void List_ReturnsViewResultWithTags()
        {
            // Arrange
            _dbContext.Tags.Add(new Tag { Name = "test", DisplayName = "Test" });
            _dbContext.SaveChanges();

            // Act
            var result = _controller.List() as ViewResult;
            var tags = result?.Model as List<Tag>;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(tags);
          
        }

        [TestMethod]
        public void Edit_ReturnsViewResultWithEditTagRequest()
        {
            // Arrange
            var tag = new Tag { Name = "test", DisplayName = "Test" };
            _dbContext.Tags.Add(tag);
            _dbContext.SaveChanges();

            // Act
            var result = _controller.Edit(tag.Id) as ViewResult;
            var editTagRequest = result?.Model as EditTagRequest;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(editTagRequest);
            Assert.AreEqual(tag.Id, editTagRequest.Id);
            Assert.AreEqual(tag.Name, editTagRequest.Name);
            Assert.AreEqual(tag.DisplayName, editTagRequest.DisplayName);
        }

        [TestMethod]
        public void Edit_UpdatesTagInDatabase()
        {
            // Arrange
            var tag = new Tag { Name = "test", DisplayName = "Test" };
            _dbContext.Tags.Add(tag);
            _dbContext.SaveChanges();

            var request = new EditTagRequest
            {
                Id = tag.Id,
                Name = "new name",
                DisplayName = "New Name"
            };

            // Act
            var result = _controller.Edit(request) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            var updatedTag = _dbContext.Tags.Find(tag.Id);
            Assert.IsNotNull(updatedTag);
            Assert.AreEqual(request.Name, updatedTag.Name);
            Assert.AreEqual(request.DisplayName, updatedTag.DisplayName);
        }

        [TestMethod]
        public void Delete_ValidTagId_RemovesTagFromDatabase()
        {
            // Arrange
            // Add a tag to the in-memory database
            var tag = new Tag { Id = Guid.NewGuid(), Name = "Test Tag", DisplayName = "Test Tag" };
            _dbContext.Tags.Add(tag);
            _dbContext.SaveChanges();

            // Create an instance of the controller to be tested
            var controller = new AdminTagsController(_dbContext);

            // Act
            // Call the Delete method with the tag's ID
            var result = controller.Delete(new EditTagRequest { Id = tag.Id }) as RedirectToActionResult;

            // Assert
            // Verify that the tag was removed from the database
            Assert.AreEqual("List", result.ActionName);
            Assert.IsFalse(_dbContext.Tags.Any(t => t.Id == tag.Id));
        }

        [TestMethod]
        public void Delete_InvalidTagId_ReturnsToEditView()
        {
            // Arrange
            // Create an instance of the controller to be tested
            var controller = new AdminTagsController(_dbContext);

            // Act
            // Call the Delete method with an invalid tag ID
            var result = controller.Delete(new EditTagRequest { Id = Guid.NewGuid() }) as RedirectToActionResult;

            // Assert
            // Verify that the method returned to the Edit view
            Assert.AreEqual("Edit", result.ActionName);
        }

    }
}
