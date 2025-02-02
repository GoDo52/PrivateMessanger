using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateMessanger.Models
{
    public class Message
    {
        public int Id { get; set; }

        [Required]
        public int ChatId { get; set; }

        [Required]
        public int SenderId { get; set; }

        [Required]
        public string TextContent { get; set; }

        public DateTime TimeStamp { get; set; } = DateTime.UtcNow;

        [ForeignKey("ChatId")]
        public Chat Chat { get; set; }

        [ForeignKey("SenderId")]
        public User User { get; set; }

    }
}
