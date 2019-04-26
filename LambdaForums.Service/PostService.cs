using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LambdaForum.Data;
using LambdaForums.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace LambdaForums.Service
{
    public class PostService : IPost
    {
        private readonly ApplicationDbContext _context;

        public PostService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Add(Post post)
        {
            _context.Add(post);
            await _context.SaveChangesAsync();
        }

        public Task Delete(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task EditPostContent(int forumId, string newContent)
        {
            throw new System.NotImplementedException();
        }

        // Get all Posts with linked tabels
        public IEnumerable<Post> GetAll()
        {
            var posts = _context.Posts
                .Include(post => post.Forum)
                .Include(post => post.User)
                .Include(post => post.Replies)
                .ThenInclude(reply => reply.User);
            return posts;
        }

        public Post GetById(int id)
        {
            return _context.Posts.Where(post => post.Id == id)
                .Include(post => post.Forum)
                .Include(post => post.User)
                .Include(post => post.Replies)
                    .ThenInclude(reply => reply.User)
                .FirstOrDefault();
        }

        
        public IEnumerable<Post> GetFilteredPosts(string searchQuery)
        {
            var query = searchQuery.ToLower();

            return _context.Posts
                .Include(post => post.Forum)
                .Include(post => post.User)
                .Include(post => post.Replies)
                .Where(post =>
                    post.Title.ToLower().Contains(query)
                    || post.Content.ToLower().Contains(query));
        }
      
        
        // Get 10 latest posts
        public IEnumerable<Post> GetLatestPosts(int count)
        {
            var allPosts = GetAll().OrderByDescending(post => post.Created);
            return allPosts.Take(count);
        }

        // Get all Posts of particular Forum
        public IEnumerable<Post> GetPostsByForum(int id)
        {
            return _context.Forums.Where(forum => forum.Id == id).First().Posts;
        }
        
        // Get the number of Replies for the particular Post
        public int GetReplyCount(int id)
        {
            return GetById(id).Replies.Count();
        }

        // Get the Image of the Forum
        public string GetForumImageUrl(int id)
        {
            var post = GetById(id);
            return post.Forum.ImageUrl;
        }


    }
}
