using DevBlog.Web.Data;
using DevBlog.Web.Models.Domain;
using DevBlog.Web.Models.ViewModels;
using DevBlog.Library.Services;
using Microsoft.EntityFrameworkCore;

using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;
using OpenQA.Selenium.Support.UI;

namespace DevBlog.Library.Tests
{
    [TestClass]
    public class TagServicesIntegrationTests
    {
        private BlogDbContext dbContext;
        private TagServices tagServices;
        private IWebDriver _webDriver;

        [TestInitialize]
        public void Initialize()
        {
            var options = new DbContextOptionsBuilder<BlogDbContext>()
                .UseInMemoryDatabase(databaseName: "DevBlog")
                .Options;
            dbContext = new BlogDbContext(options);
            tagServices = new TagServices(dbContext);

            
        }

   


        [TestMethod]

        public void TestMethod1()
        {
            
            _webDriver = new ChromeDriver();
            _webDriver.Navigate().GoToUrl("https://localhost:7024/");
            Assert.IsTrue(_webDriver.Title.Contains("Home Page"));

            _webDriver?.Quit();

        }

        [TestCleanup]
        public void Cleanup()
        {
            dbContext.Database.EnsureDeleted();
            dbContext.Dispose();
        }

        [TestMethod]
        public void AddTagView_ShouldSubmitFormSuccessfully()
        {

            new DriverManager().SetUpDriver(new ChromeConfig());
            _webDriver = new ChromeDriver();
            _webDriver.Navigate().GoToUrl("https://localhost:7024/AdminTags/Add");

            var nameField = _webDriver.FindElement(By.Id("name"));
                nameField.SendKeys("Test Tag");

                var displayNameField = _webDriver.FindElement(By.Id("displayName"));
                displayNameField.SendKeys("Test Tag Display Name");

                var submitButton = _webDriver.FindElement(By.CssSelector("button[type='submit']"));
                submitButton.Click();

                var url = _webDriver.Url;
                Assert.IsTrue(url.Contains("/AdminTags"));

                _webDriver.Quit();
        }


        [TestMethod]
        public async Task DeleteAsync_ShouldRemoveTagFromDatabase()
        {

            var tag = new Tag
            {
                Id = Guid.NewGuid(),
                Name = "Test Tag",
                DisplayName = "Test Tag Display Name"
            };
            await dbContext.Tags.AddAsync(tag);
            await dbContext.SaveChangesAsync();

            var result = await tagServices.DeleteAsync(tag.Id);

            Assert.IsNotNull(result);
            Assert.AreEqual(tag, result);

            var dbTag = await dbContext.Tags.FindAsync(tag.Id);
            Assert.IsNull(dbTag);
        }

        [TestMethod]
        public async Task GetAllAsync_ShouldReturnAllTagsFromDatabase()
        {
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


            var result = await tagServices.GetAllAsync();


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

            var tag = new Tag
            {
                Id = Guid.NewGuid(),
                Name = "Test Tag",
                DisplayName = "Test Tag Display Name"
            };
            await dbContext.Tags.AddAsync(tag);
            await dbContext.SaveChangesAsync();


            var result = await tagServices.GetAsync(tag.Id);

            Assert.IsNotNull(result);
            Assert.AreEqual(tag, result);
            Assert.AreEqual(tag.Name, result.Name);
            Assert.AreEqual(tag.DisplayName, result.DisplayName);
        }

        [TestMethod]
        public async Task UpdateAsync_ShouldUpdateTagInDatabase()
        {

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


            var result = await tagServices.UpdateAsync(updatedTag);

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
