using DevBlog.Web.Controllers;
using DevBlog.Web.Data;
using DevBlog.Web.Models.Domain;
using DevBlog.Web.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace DevBlogIntegration
{
    [TestClass]
    public class TagDataIntegrationTest

    {
        private DbContextOptions<BlogDbContext> options;
        private BlogDbContext context;

        [TestInitialize]
        public void Setup()
        {
            // Setup in-memory database for testing
            options = new DbContextOptionsBuilder<BlogDbContext>()
                .UseInMemoryDatabase("TestDatabase")
                .Options;

            context = new BlogDbContext(options);

            // Seed database with test data
            var tags = new[]
            {
                new Tag { Id = Guid.NewGuid(), Name = "Tag1", DisplayName = "Tag 1" },
                new Tag { Id = Guid.NewGuid(), Name = "Tag2", DisplayName = "Tag 2" },
                new Tag { Id = Guid.NewGuid(), Name = "Tag3", DisplayName = "Tag 3" }
            };

            context.Tags.AddRange(tags);
            context.SaveChanges();
        }

        [TestCleanup]
        public void Cleanup()
        {
            // Cleanup in-memory database after testing
            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [TestMethod]
        public void TestAddTag()
        {
            // Arrange
            var controller = new AdminTagsController(context);
            var addTagRequest = new AddTagRequest { Name = "Tag4", DisplayName = "Tag 4" };

            // Act
            controller.Add(addTagRequest);

            // Assert
            var tags = context.Tags.ToList();
            Assert.AreEqual(4, tags.Count);
            var addedTag = tags.FirstOrDefault(x => x.Name == "Tag4");
            Assert.IsNotNull(addedTag);
            Assert.AreEqual("Tag 4", addedTag.DisplayName);
        }

        [TestMethod]
        public void TestEditTag()
        {
            // Arrange
            var controller = new AdminTagsController(context);
            var tagToEdit = context.Tags.First();
            var editTagRequest = new EditTagRequest { Id = tagToEdit.Id, Name = "Tag1_New", DisplayName = "Tag 1 New" };

            // Act
            controller.Edit(editTagRequest);

            // Assert
            var editedTag = context.Tags.Find(tagToEdit.Id);
            Assert.AreEqual("Tag1_New", editedTag.Name);
            Assert.AreEqual("Tag 1 New", editedTag.DisplayName);
        }

        [TestMethod]
        public void TestDeleteTag()
        {
            // Arrange
            var controller = new AdminTagsController(context);
            var tagToDelete = context.Tags.First();

            // Act
            controller.Delete(new EditTagRequest { Id = tagToDelete.Id });

            // Assert
            var deletedTag = context.Tags.Find(tagToDelete.Id);
            Assert.IsNull(deletedTag);
            var tags = context.Tags.ToList();
            Assert.AreEqual(2, tags.Count);
        }
    }
}
