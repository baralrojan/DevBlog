﻿using DevBlog.Web.Data;
using DevBlog.Web.Models.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevBlog.Library.Services
{
    public class BlogPostServices : IBlogPostService
    {
        private readonly BlogDbContext blogDbContext;

        public BlogPostServices(BlogDbContext blogDbContext)
        {
            this.blogDbContext = blogDbContext;
        }
        public async Task<BlogPost> AddAsync(BlogPost blogPost)
        {
            await blogDbContext.AddAsync(blogPost);
            await blogDbContext.SaveChangesAsync();
            return blogPost;
        }

        public async Task<BlogPost?> DeleteAsync(Guid id)
        {
            var existingBlog = await blogDbContext.BlogPosts.FindAsync(id);

            if (existingBlog != null)
            {
                blogDbContext.BlogPosts.Remove(existingBlog);
                await blogDbContext.SaveChangesAsync();
                return existingBlog;
            }

            return null;
        }

        public async Task<IEnumerable<BlogPost>> GetAllAsync()
        {
            return await blogDbContext.BlogPosts.Include(x => x.Tags).ToListAsync();
        }

        public async Task<BlogPost?> GetAsync(Guid id)
        {
            return await blogDbContext.BlogPosts.Include(x => x.Tags).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<BlogPost?> UpdateAsync(BlogPost blogPost)
        {
            var existingBlog = await blogDbContext.BlogPosts.Include(x => x.Tags)
                .FirstOrDefaultAsync(x => x.Id == blogPost.Id);

            if (existingBlog != null)
            {
                existingBlog.Id = blogPost.Id;
                existingBlog.Heading = blogPost.Heading;
                existingBlog.PageTitle = blogPost.PageTitle;
                existingBlog.Content = blogPost.Content;
                existingBlog.ShortDescription = blogPost.ShortDescription;
                existingBlog.Author = blogPost.Author;
                existingBlog.FeaturedImageUrl = blogPost.FeaturedImageUrl;
                existingBlog.UrlHandle = blogPost.UrlHandle;
                existingBlog.Visible = blogPost.Visible;
                existingBlog.PublishedDate = blogPost.PublishedDate;
                existingBlog.Tags = blogPost.Tags;

                await blogDbContext.SaveChangesAsync();
                return existingBlog;
            }

            return null;
        }
    }
}
