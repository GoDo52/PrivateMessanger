using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateMessenger.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(15)]
        public string Tag { get; set; }

        [Required]
        [MaxLength(30)]
        public string UserName { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        public bool IsApproved { get; set; } = false;

        public int AdministrationRoleId { get; set; } = 1;

        public ICollection<UserChat> UserChats { get; set; } = new List<UserChat>();

        [ForeignKey("AdministrationRoleId")]
        public AdministrationRole AdministrationRole { get; set; }
    }
}
