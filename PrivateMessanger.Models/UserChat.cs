using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateMessanger.Models
{
    public class UserChat
    {
        public int UserId { get; set; }
        public int ChatId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        [ForeignKey("ChatId")]
        public Chat Chat { get; set; }
    }
}
