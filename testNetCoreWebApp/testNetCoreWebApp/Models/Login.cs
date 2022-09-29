using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace testNetCoreWebApp.Models
{
	public class Login
    {
        [Key]
        public int userId { get; set; }

        [Required(ErrorMessage = "Username is requried!")]
        public string username { get; set; }

        [Required(ErrorMessage = "Password is requried!")]
        public string password { get; set; }
    }
}
