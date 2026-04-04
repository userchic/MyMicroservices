using Microsoft.EntityFrameworkCore;
using TextPostsService.Abstractions;
using TextPostsService.DataBase;
using TextPostsService.Model;

namespace TextPostsService.Repositories
{
    public class TextPostRepository : ITextPostRepository
    {
        TextPostContext _context;
        public TextPostRepository(TextPostContext context)
        {
            _context = context;
        }
        public async Task<ICollection<TextPost>> GetUserPostsPage(int page,int userId)
        {
            return await _context.Posts.Where(post => post.UserId == userId).OrderBy(post => -post.Id).Skip(10 * (page - 1)).Take(10).ToListAsync();
        }
        public async Task<TextPost?> GetPost(int id)
        {
            return await _context.Posts.FirstOrDefaultAsync(post => post.Id == id);
        }

        public async Task<TextPost?> GetUserPost(int id, int userId)
        {
            return await _context.Posts.FirstOrDefaultAsync(post => post.Id == id && post.UserId==userId);
        }
        public async Task<TextPost> Create(TextPost post)
        {
            await _context.Posts.AddAsync(post);
            return post;
        }
        public void Update(TextPost post)
        {
            _context.Posts.Update(post);
        }
        public void Delete(TextPost post)
        {
            _context.Posts.Remove(post);
        }
        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }


    }
}
