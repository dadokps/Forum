using LambdaForum.Data.Models;
using LambdaForum.Models.Post;
using LambdaForum.Models.Reply;
using LambdaForums.Data;
using LambdaForums.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LambdaForum.Controllers
{
    public class PostController : Controller
    {

        private readonly IPost _postService;
        private readonly IForum _forumService;

        private static UserManager<ApplicationUser> _userManager;

        // Constructor for dependency injection to pass services to PostController
        public PostController(IPost postService, IForum forumService, IApplicationUser userService, UserManager<ApplicationUser> userManager)
        {
            _postService = postService;
            _forumService = forumService;
            _userManager = userManager;
        }

        public IActionResult Index(int id)
        {
            var post = _postService.GetById(id);
            var replies = GetPostReplies(post).OrderBy(reply => reply.Date);

            var model = new PostIndexModel
            {
                Id = post.Id,
                Title = post.Title,
                AuthorId = post.User.Id,
                AuthorName = post.User.UserName,
                AuthorImageUrl = post.User.ProfileImage,
                AuthorRating = post.User.Rating,
                //IsAuthorAdmin = IsAuthorAdmin(post.User),
                Date = post.Created,
                //PostContent = _postFormatter.Prettify(post.Content),
                Replies = replies,
                ForumId = post.Forum.Id,
                ForumName = post.Forum.Title
            };

            return View(model);
        }

        private IEnumerable<PostReplyModel> GetPostReplies(Post post)
        {
            return post.Replies.Select(reply => new PostReplyModel
            {
                Id = reply.Id,
                AuthorName = reply.User.UserName,
                AuthorId = reply.User.Id,
                AuthorImageUrl = reply.User.ProfileImage,
                AuthorRating = reply.User.Rating,
                Date = reply.Created,
                ReplyContent = reply.Content
               // IsAuthorAdmin = IsAuthorAdmin(reply.User)
            });
        }

    }
}