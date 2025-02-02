using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateMessanger.DTOs
{
    public class UserLoginDto
    {
        [Required]
        public string Tag { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
