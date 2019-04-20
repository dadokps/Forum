using System.Diagnostics;
using System.Linq;
using LambdaForum.Models;
using LambdaForum.Models.Home;
using LambdaForum.Models.Post;
using LambdaForums.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace LambdaForum.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPost _postService;

        public HomeController(IPost postService)
        {
            _postService = postService;
        }

        public IActionResult Index()
        {
            var model = BuildHomeIndexModel();
            return View(model);
        }

        public HomeIndexModel BuildHomeIndexModel()
        {
            var latest = _postService.GetLatestPosts(10);

            var posts = latest.Select(post => new PostListingModel
            {
                Id = post.Id,
                Title = post.Title,
                AuthorName = post.User.UserName,
                AuthorId = post.User.Id,
                AuthorRating = post.User.Rating,
                DatePosted = post.Created.ToString(),
                RepliesCount = _postService.GetReplyCount(post.Id),
                ForumName = post.Forum.Title,
                ForumImageUrl = _postService.GetForumImageUrl(post.Id),
                ForumId = post.Forum.Id
            });

            return new HomeIndexModel()
            {
                LatestPosts = posts
            };

        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
