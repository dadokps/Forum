using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LambdaForum.Models.Forum;
using LambdaForums.Data;
using LambdaForums.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace LambdaForum.Controllers
{
    public class ForumController : Controller
    {
        private readonly IForum _forumService;
        private readonly IPost _postsService;

        public ForumController(IForum forumService)
        {
            _forumService = forumService;
        }

        public IActionResult Index()
        {
            var forums = _forumService.GetAll()
                .Select(forum => new ForumListingModel {
                    Id = forum.Id,
                    Title = forum.Title,
                    Description = forum.Description
                });

            var model = new ForumIndexModel
            {
                ForumList = forums
            };

            return View(model);
        }

        // Get Forum
        public IActionResult Topic(int id)
        {
            var forum = _forumService.GetById(id);
            var posts = _postsService.GetFilteredPosts(id); // get the Posts from particular Forum

            var postListings = 
        }
    }
}