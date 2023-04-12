using DevBlog.Web.Data;
using DevBlog.Web.Models.Domain;
using DevBlog.Web.Models.ViewModels;
using DevBlog.Library.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace DevBlog.Library.Tests.Services
{
    [TestClass]
    public class TagDataServicesIntegrationTests
    {
        private BlogDbContext _dbContext;
        private TagDataServices _tagDataServices;

        [TestInitialize]
        public void Initialize()
        {
            var options = new DbContextOptionsBuilder<BlogDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _dbContext = new BlogDbContext(options);
            _tagDataServices = new TagDataServices(_dbContext);
        }

        [TestMethod]
        public void AddTag_Should_Return_True_When_Adding_Valid_Tag()
        {
            // Arrange
            var tag = new Tag
            {
                Id = Guid.NewGuid(),
                Name = "TestTag",
                DisplayName = "Test Tag"
            };

            // Act
            var result = _tagDataServices.AddTag(tag);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ListTags_Should_Return_All_Tags()
        {
            // Arrange
            var tags = new List<Tag>
            {
                new Tag
                {
                    Id = Guid.NewGuid(),
                    Name = "Tag1",
                    DisplayName = "Tag 1"
                },
                new Tag
                {
                    Id = Guid.NewGuid(),
                    Name = "Tag2",
                    DisplayName = "Tag 2"
                },
                new Tag
                {
                    Id = Guid.NewGuid(),
                    Name = "Tag3",
                    DisplayName = "Tag 3"
                }
            };
            _dbContext.Tags.AddRange(tags);
            _dbContext.SaveChanges();

            // Act
            var result = _tagDataServices.ListTags();

            // Assert
            Assert.AreEqual(tags.Count, result.Count);
        }

        [TestMethod]
        public void GetTag_Should_Return_EditTagRequest_For_Valid_Tag_Id()
        {
            // Arrange
            var tag = new Tag
            {
                Id = Guid.NewGuid(),
                Name = "TestTag",
                DisplayName = "Test Tag"
            };
            _dbContext.Tags.Add(tag);
            _dbContext.SaveChanges();

            // Act
            var result = _tagDataServices.GetTag(tag.Id);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(tag.Id, result?.Id);
            Assert.AreEqual(tag.Name, result?.Name);
            Assert.AreEqual(tag.DisplayName, result?.DisplayName);
        }

        [TestMethod]
        public void GetTag_Should_Return_Null_For_Invalid_Tag_Id()
        {
            // Arrange
            var tagId = Guid.NewGuid();

            // Act
            var result = _tagDataServices.GetTag(tagId);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void EditTag_Should_Return_True_For_Valid_EditTagRequest()
        {
            // Arrange
            var tag = new Tag
            {
                Id = Guid.NewGuid(),
                Name = "TestTag",
                DisplayName = "Test Tag"
            };
            _dbContext.Tags.Add(tag);
            _dbContext.SaveChanges();

            var editTagRequest = new EditTagRequest
            {
                Id = tag.Id,
                Name = "NewTestTag",
                DisplayName = "New Test Tag"
            };

            // Act
            var result = _tagDataServices.EditTag(editTagRequest);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(editTagRequest.Name, tag.Name);
            Assert.AreEqual(editTagRequest.DisplayName, tag.DisplayName);
        }

        [TestMethod]
        public void EditTag_Should_Return_False_For_Invalid_EditTagRequest()
        {
            // Arrange
            var editTagRequest = new EditTagRequest
            {
                Id = Guid.NewGuid(),
                Name = "NewTestTag",
                DisplayName = "New Test Tag"
            };

            // Act
            var result = _tagDataServices.EditTag(editTagRequest);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void DeleteTag_Should_Return_True_For_Valid_Tag_Id()
        {
            // Arrange
            var tag = new Tag
            {
                Id = Guid.NewGuid(),
                Name = "TestTag",
                DisplayName = "Test Tag"
            };
            _dbContext.Tags.Add(tag);
            _dbContext.SaveChanges();

            // Act
            var result = _tagDataServices.DeleteTag(tag.Id);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void DeleteTag_Should_Return_False_For_Invalid_Tag_Id()
        {
            // Arrange
            var tagId = Guid.NewGuid();

            // Act
            var result = _tagDataServices.DeleteTag(tagId);

            // Assert
            Assert.IsFalse(result);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }
    }
}



