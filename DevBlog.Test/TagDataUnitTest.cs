using DevBlog.Library.Services;
using DevBlog.Web.Data;
using DevBlog.Web.Models.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevBlog.Library.Tests.Services
{
    [TestClass]
    public class TagServicesTests
    {
        private BlogDbContext _dbContext;
        private ITagServices _tagServices;

        [TestInitialize]
        public void Initialize()
        {
            // Create an in-memory database for testing purposes
            var options = new DbContextOptionsBuilder<BlogDbContext>()
                .UseInMemoryDatabase("TestTagDatabase")
                .Options;

            _dbContext = new BlogDbContext(options);
            _tagServices = new TagServices(_dbContext);
        }

        [TestMethod]
        public async Task GetAllAsync_ReturnsAllTags()
        {
            // Arrange
            var tags = new List<Tag>
            {
                new Tag { Id = Guid.NewGuid(), Name = "tag1", DisplayName = "Tag 1" },
                new Tag { Id = Guid.NewGuid(), Name = "tag2", DisplayName = "Tag 2" },
                new Tag { Id = Guid.NewGuid(), Name = "tag3", DisplayName = "Tag 3" }
            };
            await _dbContext.Tags.AddRangeAsync(tags);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _tagServices.GetAllAsync();

            // Assert
            Assert.AreEqual(tags.Count, result.Count());
            CollectionAssert.AreEquivalent(tags, result.ToList());
        }

        [TestMethod]
        public async Task GetAsync_ReturnsExistingTag()
        {
            // Arrange
            var tag = new Tag { Id = Guid.NewGuid(), Name = "tag1", DisplayName = "Tag 1" };
            await _dbContext.Tags.AddAsync(tag);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _tagServices.GetAsync(tag.Id);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(tag.Id, result.Id);
            Assert.AreEqual(tag.Name, result.Name);
            Assert.AreEqual(tag.DisplayName, result.DisplayName);
        }

        [TestMethod]
        public async Task GetAsync_ReturnsNullForNonExistingTag()
        {
            // Arrange
            var nonExistingId = Guid.NewGuid();

            // Act
            var result = await _tagServices.GetAsync(nonExistingId);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task AddAsync_AddsNewTag()
        {
            // Arrange
            var tag = new Tag { Name = "tag1", DisplayName = "Tag 1" };

            // Act
            var result = await _tagServices.AddAsync(tag);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(tag.Name, result.Name);
            Assert.AreEqual(tag.DisplayName, result.DisplayName);
            Assert.IsNotNull(result.Id);
            Assert.AreNotEqual(Guid.Empty, result.Id);

            var addedTag = await _dbContext.Tags.FindAsync(result.Id);
            Assert.IsNotNull(addedTag);
            Assert.AreEqual(tag.Name, addedTag.Name);
            Assert.AreEqual(tag.DisplayName, addedTag.DisplayName);
        }

        [TestMethod]
        public async Task UpdateAsync_UpdatesExistingTag()
        {
            // Arrange
            var tag = new Tag { Name = "tag1", DisplayName = "Tag 1" };
            await _dbContext.Tags.AddAsync(tag);
            await _dbContext.SaveChangesAsync();

            var updatedTag = new Tag { Id = tag.Id, Name = "updated-tag1", DisplayName = "Updated Tag 1" };

            // Act
            var result = await _tagServices.UpdateAsync(updatedTag);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(updatedTag.Id, result.Id);
            Assert.AreEqual(updatedTag.Name, result.Name);
            Assert.AreEqual(updatedTag.DisplayName, result.DisplayName);

            var updatedEntity = await _dbContext.Tags.FindAsync(updatedTag.Id);
            Assert.IsNotNull(updatedEntity);
            Assert.AreEqual(updatedTag.Name, updatedEntity.Name);
            Assert.AreEqual(updatedTag.DisplayName, updatedEntity.DisplayName);
        }

        [TestMethod]
        public async Task UpdateAsync_ReturnsNullForNonExistingTag()
        {
            // Arrange
            var nonExistingTag = new Tag { Id = Guid.NewGuid(), Name = "non-existing-tag", DisplayName = "Non-existing Tag" };

            // Act
            var result = await _tagServices.UpdateAsync(nonExistingTag);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task DeleteAsync_DeletesExistingTag()
        {
            // Arrange
            var tag = new Tag { Name = "tag1", DisplayName = "Tag 1" };
            await _dbContext.Tags.AddAsync(tag);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _tagServices.DeleteAsync(tag.Id);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(tag.Id, result.Id);
            Assert.AreEqual(tag.Name, result.Name);
            Assert.AreEqual(tag.DisplayName, result.DisplayName);

            var deletedEntity = await _dbContext.Tags.FindAsync(tag.Id);
            Assert.IsNull(deletedEntity);
        }

        [TestMethod]
        public async Task DeleteAsync_ReturnsNullForNonExistingTag()
        {
            // Arrange
            var nonExistingId = Guid.NewGuid();

            // Act
            var result = await _tagServices.DeleteAsync(nonExistingId);

            // Assert
            Assert.IsNull(result);
        }
    }

}
