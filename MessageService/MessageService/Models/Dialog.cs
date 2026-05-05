using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace MessageService.Models
{
    [Index("Id","User1Id","User2Id")]
    public class Dialog
    {
        [Key]
        public int Id { get; set; }
        public int User1Id { get; set; }
        public int User2Id { get; set; }
        public List<Message> Messages { get; set; }
    }
}
