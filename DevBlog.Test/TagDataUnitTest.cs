using DevBlog.Web.Data;
using DevBlog.Web.Models.Domain;
using DevBlog.Web.Models.ViewModels;
using DevBlog.Library.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DevBlog.Library.Tests.Services
{
    [TestClass]
    public class TagDataServicesTests
    {
        private readonly DbContextOptions<BlogDbContext> options = new DbContextOptionsBuilder<BlogDbContext>()
            .UseInMemoryDatabase(databaseName: "BlogTestDatabase")
            .Options;

        [TestMethod]
        public void AddTag_Should_Return_True()
        {
            // Arrange
            using (var context = new BlogDbContext(options))
            {
                var service = new TagDataServices(context);

                var tag = new Tag
                {
                    Id = Guid.NewGuid(),
                    Name = "test tag",
                    DisplayName = "Test Tag"
                };

                // Act
                var result = service.AddTag(tag);

                // Assert
                Assert.IsTrue(result);
            }
        }

        [TestMethod]
        public void ListTags_Should_Return_List_Of_Tags()
        {
            // Arrange
            using (var context = new BlogDbContext(options))
            {
                var service = new TagDataServices(context);

                var tag1 = new Tag
                {
                    Id = Guid.NewGuid(),
                    Name = "test tag 1",
                    DisplayName = "Test Tag 1"
                };

              

                context.Tags.Add(tag1);
                context.SaveChanges();

                // Act
                var result = service.ListTags();

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(2, result.Count);
                Assert.IsTrue(result.Any(x => x.Id == tag1.Id && x.Name == tag1.Name && x.DisplayName == tag1.DisplayName));
            }
        }

        [TestMethod]
        public void GetTag_Should_Return_EditTagRequest_IfExists()
        {
            // Arrange
            using (var context = new BlogDbContext(options))
            {
                var service = new TagDataServices(context);

                var tag = new Tag
                {
                    Id = Guid.NewGuid(),
                    Name = "test tag",
                    DisplayName = "Test Tag"
                };

                context.Tags.Add(tag);
                context.SaveChanges();

                // Act
                var result = service.GetTag(tag.Id);

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(tag.Id, result.Id);
                Assert.AreEqual(tag.Name, result.Name);
                Assert.AreEqual(tag.DisplayName, result.DisplayName);
            }
        }

        [TestMethod]
        public void GetTag_Should_Return_Null_IfNotExists()
        {
            // Arrange
            using (var context = new BlogDbContext(options))
            {
                var service = new TagDataServices(context);

                // Act
                var result = service.GetTag(Guid.NewGuid());

                // Assert
                Assert.IsNull(result);
            }
        }

        [TestMethod]
        public void EditTag_Should_Return_True_IfTagExists()
        {
            // Arrange
            using (var context = new BlogDbContext(options))
            {
                var service = new TagDataServices(context);

                var tag = new Tag
                {
                    Id = Guid.NewGuid(),
                    Name = "test tag",
                    DisplayName = "Test Tag"
                };

                context.Tags.Add(tag);
                context.SaveChanges();

                var editTagRequest = new EditTagRequest
                {
                    Id = tag.Id,
                    Name = "new test tag name",
                    DisplayName = "New Test Tag Name"
                };

                // Act
                var result = service.EditTag(editTagRequest);

                // Assert
                Assert.IsTrue(result);
                Assert.AreEqual("new test tag name", context.Tags.First().Name);
                Assert.AreEqual("New Test Tag Name", context.Tags.First().DisplayName);
            }
        }

        [TestMethod]
        public void EditTag_Should_Return_False_IfTagDoesNotExist()
        {
            // Arrange
            using (var context = new BlogDbContext(options))
            {
                var service = new TagDataServices(context);

                var editTagRequest = new EditTagRequest
                {
                    Id = Guid.NewGuid(),
                    Name = "new test tag name",
                    DisplayName = "New Test Tag Name"
                };

                // Act
                var result = service.EditTag(editTagRequest);

                // Assert
                Assert.IsFalse(result);
            }
        }

        [TestMethod]
        public void DeleteTag_Should_Return_True_IfTagExists()
        {
            // Arrange
            using (var context = new BlogDbContext(options))
            {
                var service = new TagDataServices(context);

                var tag = new Tag
                {
                    Id = Guid.NewGuid(),
                    Name = "test tag",
                    DisplayName = "Test Tag"
                };

                context.Tags.Add(tag);
                context.SaveChanges();

                // Act
                var result = service.DeleteTag(tag.Id);
                Console.WriteLine(result);

                // Assert
                Assert.IsTrue(result);
                Assert.AreEqual(1, context.Tags.Count());
            }
        }

        [TestMethod]
        public void DeleteTag_Should_Return_False_IfTagDoesNotExist()
        {
            // Arrange
            using (var context = new BlogDbContext(options))
            {
                var service = new TagDataServices(context);

                // Act
                var result = service.DeleteTag(Guid.NewGuid());

                // Assert
                Assert.IsFalse(result);
            }
        }
    }
}


