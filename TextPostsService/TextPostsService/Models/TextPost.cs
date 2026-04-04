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
    }
}
