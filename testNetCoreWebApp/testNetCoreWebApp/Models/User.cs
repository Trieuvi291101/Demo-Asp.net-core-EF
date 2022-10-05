using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace testNetCoreWebApp.Models
{
    public partial class User
    {
        public User()
        {
            SaleOrder = new HashSet<SaleOrder>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public virtual ICollection<SaleOrder> SaleOrder { get; set; }
  
    }
}
