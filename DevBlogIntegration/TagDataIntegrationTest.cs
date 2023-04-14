using DevBlog.Web.Data;
using DevBlog.Web.Models.Domain;
using DevBlog.Web.Models.ViewModels;
using DevBlog.Library.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevBlog.Library.Tests
{
    [TestClass]
    public class TagServicesIntegrationTests
    {
        private BlogDbContext dbContext;
        private TagServices tagServices;

        [TestInitialize]
        public void Initialize()
        {
            var options = new DbContextOptionsBuilder<BlogDbContext>()
                .UseInMemoryDatabase(databaseName: "DevBlog")
                .Options;
            dbContext = new BlogDbContext(options);
            tagServices = new TagServices(dbContext);
        }

        [TestCleanup]
        public void Cleanup()
        {
            dbContext.Database.EnsureDeleted();
            dbContext.Dispose();
        }

        [TestMethod]
        public async Task AddAsync_ShouldAddTagToDatabase()
        {
            // Arrange
            var tag = new Tag
            {
                Id = Guid.NewGuid(),
                Name = "Test Tag",
                DisplayName = "Test Tag Display Name"
            };

            // Act
            var result = await tagServices.AddAsync(tag);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(tag, result);

            var dbTag = await dbContext.Tags.FirstOrDefaultAsync(x => x.Id == tag.Id);
            Assert.IsNotNull(dbTag);
            Assert.AreEqual(tag.Name, dbTag.Name);
            Assert.AreEqual(tag.DisplayName, dbTag.DisplayName);
        }

        [TestMethod]
        public async Task DeleteAsync_ShouldRemoveTagFromDatabase()
        {
            // Arrange
            var tag = new Tag
            {
                Id = Guid.NewGuid(),
                Name = "Test Tag",
                DisplayName = "Test Tag Display Name"
            };
            await dbContext.Tags.AddAsync(tag);
            await dbContext.SaveChangesAsync();

            // Act
            var result = await tagServices.DeleteAsync(tag.Id);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(tag, result);

            var dbTag = await dbContext.Tags.FindAsync(tag.Id);
            Assert.IsNull(dbTag);
        }

        [TestMethod]
        public async Task GetAllAsync_ShouldReturnAllTagsFromDatabase()
        {
            // Arrange
            var tag1 = new Tag
            {
                Id = Guid.NewGuid(),
                Name = "Test Tag 1",
                DisplayName = "Test Tag 1 Display Name"
            };
            var tag2 = new Tag
            {
                Id = Guid.NewGuid(),
                Name = "Test Tag 2",
                DisplayName = "Test Tag 2 Display Name"
            };
            await dbContext.Tags.AddRangeAsync(tag1, tag2);
            await dbContext.SaveChangesAsync();

            // Act
            var result = await tagServices.GetAllAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());

            var dbTag1 = result.FirstOrDefault(x => x.Id == tag1.Id);
            Assert.IsNotNull(dbTag1);
            Assert.AreEqual(tag1.Name, dbTag1.Name);
            Assert.AreEqual(tag1.DisplayName, dbTag1.DisplayName);

            var dbTag2 = result.FirstOrDefault(x => x.Id == tag2.Id);
            Assert.IsNotNull(dbTag2);
            Assert.AreEqual(tag2.Name, dbTag2.Name);
            Assert.AreEqual(tag2.DisplayName, dbTag2.DisplayName);
        }

        [TestMethod]
        public async Task GetAsync_ShouldReturnTagFromDatabase()
        {
            // Arrange
            var tag = new Tag
            {
                Id = Guid.NewGuid(),
                Name = "Test Tag",
                DisplayName = "Test Tag Display Name"
            };
            await dbContext.Tags.AddAsync(tag);
            await dbContext.SaveChangesAsync();

            // Act
            var result = await tagServices.GetAsync(tag.Id);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(tag, result);
            Assert.AreEqual(tag.Name, result.Name);
            Assert.AreEqual(tag.DisplayName, result.DisplayName);
        }

        [TestMethod]
        public async Task UpdateAsync_ShouldUpdateTagInDatabase()
        {
            // Arrange
            var tag = new Tag
            {
                Id = Guid.NewGuid(),
                Name = "Test Tag",
                DisplayName = "Test Tag Display Name"
            };
            await dbContext.Tags.AddAsync(tag);
            await dbContext.SaveChangesAsync();

            var updatedTag = new Tag
            {
                Id = tag.Id,
                Name = "Updated Test Tag",
                DisplayName = "Updated Test Tag Display Name"
            };

            // Act
            var result = await tagServices.UpdateAsync(updatedTag);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(tag.Id, result.Id);
            Assert.AreEqual(updatedTag.Name, result.Name);
            Assert.AreEqual(updatedTag.DisplayName, result.DisplayName);

            var dbTag = await dbContext.Tags.FindAsync(tag.Id);
            Assert.IsNotNull(dbTag);
            Assert.AreEqual(updatedTag.Name, dbTag.Name);
            Assert.AreEqual(updatedTag.DisplayName, dbTag.DisplayName);
        }
    }
}
