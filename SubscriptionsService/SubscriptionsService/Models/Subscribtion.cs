using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace SubscriptionsService.Models
{
    [Index("SubscriberId","PosterId")]
    public class Subscribtion
    {
        [Key]
        public int Id { get; set; }
        public int SubscriberId { get; set; }
        public int PosterId { get; set; }
    }
}
