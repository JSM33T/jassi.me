﻿using Jassi.Entities.Dedicated;
using Jassi.Entities.DTO;
using Jassi.Entities.Shared;
using Jassi.Repositories;
using Jassi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Reflection;

namespace Jassi.API.Controllers.Dedicated
{
    [Route("api/blog")]
    [ApiController]
    public class BlogController(IOptionsMonitor<AlmondcoveConfig> config, ILogger<FoundationController> logger, IHttpContextAccessor httpContextAccessor, ITelegramService telegramService, IBlogRepository BlogRepository) : FoundationController(config, logger, httpContextAccessor, telegramService)
    {
        private readonly IBlogRepository _BlogRepo = BlogRepository;

        [HttpPost("search")]
        #region Paginated blogs with search criteria
        public async Task<IActionResult> GetBlogsByPagination([FromBody] Blog_GetRequest request)
        {
            return await ExecuteActionAsync(async () =>
            {
                int statusCode = StatusCodes.Status200OK;
                string message = string.Empty;
                List<string> hints = [];
                PaginatedResult<Blog_GetBlogs> result;

                result = await _BlogRepo.GetPaginatedBlogsAsync(request);

                if (result.TotalRecords == 0)
                {
                    statusCode = StatusCodes.Status404NotFound;
                    message = "Not found";
                    hints.Add("no Blogs match this criteria");
                }

                return (statusCode, result, message, hints);
            }, MethodBase.GetCurrentMethod().Name);
        }

        #endregion

        [HttpGet("load/{year}/{slug}")]
        #region load blog by slug
        public async Task<IActionResult> GetStuff(string slug, string year)
        {
            return await ExecuteActionAsync(async () =>
            {
                int statusCode = default;
                string message = string.Empty;
                List<string> hints = [];
                Blog_GetDetails BlogDetails = new();
                Blog Blog = new();

                var filePath = Path.Combine("wwwroot", "content","blogs", year, slug, $"content.md");
                Blog = await _BlogRepo.GetBlogDetailsBySlug(slug);
                if (Blog != null)
                {
                    BlogDetails.Slug = Blog.Slug;
                    BlogDetails.Name = Blog.BlogName;
                    BlogDetails.DateAdded = Blog.DateAdded;
                    BlogDetails.Id = Blog.Id;

                    if (System.IO.File.Exists(filePath))
                    {
                        BlogDetails.Content = await System.IO.File.ReadAllTextAsync(filePath);
                        statusCode = StatusCodes.Status200OK;
                        BlogDetails.Authors = await _BlogRepo.GetBlogAuthorsByBlogId(BlogDetails.Id);
                        message = "Retrieved";
                    }
                }
                else
                {
                    message = "not found";
                    statusCode = StatusCodes.Status404NotFound;
                    hints.Add("NO blog found with this criteria");
                    BlogDetails = null;
                    
                }
               

                return (statusCode, BlogDetails, message, hints);
            }, MethodBase.GetCurrentMethod().Name);
        }

        #endregion

        [HttpGet("details/{slug}")]
        #region Get blog details post loading
        public async Task<IActionResult> GetCategoryStuff(string slug)
        {
            return await ExecuteActionAsync(async () =>
            {
                int statusCode = default;
                string message = string.Empty;
                List<string> hints = [];
                Blog blog;

                blog = await _BlogRepo.GetBlogDetailsBySlug(slug);
                if (blog  != null)
                {
                    message = "retrieved";
                    statusCode = StatusCodes.Status200OK;
                }
                else
                {
                    hints.Add("No blog found with this criteria");
                }

                return (statusCode, blog, message, hints);
            }, MethodBase.GetCurrentMethod().Name);
        }
        #endregion

        [HttpGet("getcategories")]
        #region Get categories on side pane
        public async Task<IActionResult> GetCategoryStuff()
        {
            return await ExecuteActionAsync(async () =>
            {
                int statusCode = default;
                string message = string.Empty;
                List<string> hints = [];
                List<BlogCategory> categories;

                categories = await _BlogRepo.GetCategories();
                if (categories.Count > 0)
                {
                    message = "retrieved";
                    statusCode = StatusCodes.Status200OK;
                }

                return (statusCode, categories, message, hints);
            }, MethodBase.GetCurrentMethod().Name);
        }

        #endregion
    }
}
