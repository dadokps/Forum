using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LambdaForums.Data.Models
{
    public interface IPost
    {
        Post GetById(int id);
        IEnumerable<Post> GetAll();
        IEnumerable<Post> GetFilteredPosts(string search);
        IEnumerable<Post> GetPostsByForum(int id);
        IEnumerable<Post> GetLatestPosts(int forumId);
        string GetForumImageUrl(int id);
        int GetReplyCount(int id);

        Task Add(Post post);
        Task Delete(int id);
        Task EditPostContent(int forumId, string newContent);
        
        //Task AddReply(PostReply reply);
    }
}
