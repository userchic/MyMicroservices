using System.Runtime.InteropServices;
using TextPostsService.DTO;

namespace TextPostsService.Model
{
    public class TextPost
    {
        public int Id { get; set; }
        public string Text{ get; set; }
        public DateTime PostTime { get; set; }
        public int UserId { get; set; }
        public static TextPost Create(CreatePostRequest request,int userId)
        {
            return new TextPost()
            {
                PostTime = DateTime.UtcNow,
                Text=request.Text,
                UserId = userId
            };
        }
        public override bool Equals(object? obj)
        {
            if (obj is TextPost)
            {
                TextPost post = (TextPost)obj;
                return post.Id==Id &&
                    post.Text==Text &&
                    post.UserId==UserId &&
                    post.PostTime==PostTime;
            }
            return false;
        }
    }
}
