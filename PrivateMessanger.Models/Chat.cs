using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateMessanger.Models
{
    public class Chat
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        public bool IsPrivate { get; set; }

        public ICollection<Message> Messages { get; set; } = new List<Message>();
        public ICollection<UserChat> UserChats { get; set; } = new List<UserChat>();
    }
}
