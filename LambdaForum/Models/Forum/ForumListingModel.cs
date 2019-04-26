using LambdaForum.Models.Post;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LambdaForum.Models.Forum
{
    public class ForumListingModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }

        public int NumberOfUsers { get; set; }
        public bool HasRecentPost { get; set; }

        public PostListingModel Latest { get; set; }
        public IEnumerable<PostListingModel> AllPosts { get; set; }
    }
}
