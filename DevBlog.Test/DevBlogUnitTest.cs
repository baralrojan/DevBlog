using DevBlog.Library.Services;
using DevBlog.Web.Data;
using DevBlog.Web.Models.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevBlog.Library.Tests
{
    [TestClass]
    public class BlogPostServiceTests
    {
        private BlogDbContext _dbContext;
        private IBlogPostService _blogPostService;

        [TestInitialize]
        public void Initialize()
        {
            var options = new DbContextOptionsBuilder<BlogDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _dbContext = new BlogDbContext(options);
            _blogPostService = new BlogPostServices(_dbContext);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _dbContext.Database.EnsureDeleted();
        }

        [TestMethod]
        public async Task AddAsync_ShouldAddBlogPost()
        {
            var blogPost = new BlogPost
            {
                Id = Guid.NewGuid(),
                Heading = "Test Blog Post",
                PageTitle = "Test Blog Post",
                Content = "This is a test blog post.",
                ShortDescription = "A test blog post.",
                Author = "Test Author",
                FeaturedImageUrl = "http://example.com/image.jpg",
                UrlHandle = "test-blog-post",
                Visible = true,
                PublishedDate = DateTime.UtcNow,
                Tags = new List<Tag>
                {
                     new Tag { Id = Guid.NewGuid(), Name = "tag1", DisplayName = "Tag 1" }
                }
                //This Test was failing.
            };

            var result = await _blogPostService.AddAsync(blogPost);

            Assert.IsNotNull(result);
            Assert.AreEqual(blogPost.Id, result.Id);
        }

        [TestMethod]
        public async Task GetAllAsync_ShouldReturnAllBlogPosts()
        {
            var blogPost1 = new BlogPost
            {
                Id = Guid.NewGuid(),
                Heading = "Test Blog Post 1",
                PageTitle = "Test Blog Post 1",
                Content = "This is test blog post 1.",
                ShortDescription = "A test blog post.",
                Author = "Test Author",
                FeaturedImageUrl = "http://example.com/image1.jpg",
                UrlHandle = "test-blog-post-1",
                Visible = true,
                PublishedDate = DateTime.UtcNow,
                Tags = new List<Tag>
            {
                new Tag { Id = Guid.NewGuid(), Name = "tag1", DisplayName = "Tag 1" },
            }
        };

            var blogPost2 = new BlogPost
            {
                Id = Guid.NewGuid(),
                Heading = "Test Blog Post 2",
                PageTitle = "Test Blog Post 2",
                Content = "This is test blog post 2.",
                ShortDescription = "Another test blog post.",
                Author = "Test Author",
                FeaturedImageUrl = "http://example.com/image2.jpg",
                UrlHandle = "test-blog-post-2",
                Visible = true,
                PublishedDate = DateTime.UtcNow,
                Tags = new List<Tag>
            {
                new Tag { Id = Guid.NewGuid(), Name = "tag1", DisplayName = "Tag 1" },
            }
            };

            await _dbContext.BlogPosts.AddRangeAsync(blogPost1, blogPost2);
            await _dbContext.SaveChangesAsync();

   
            var result = await _blogPostService.GetAllAsync();


            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
        }

    }
}


