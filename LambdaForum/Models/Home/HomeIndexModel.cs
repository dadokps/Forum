using LambdaForum.Models.Post;
using System.Collections.Generic;

namespace LambdaForum.Models.Home
{
    public class HomeIndexModel
    {
        public string SearchQuery { get; set; }
        public IEnumerable<PostListingModel> LatestPosts { get; set; }
    }
}
