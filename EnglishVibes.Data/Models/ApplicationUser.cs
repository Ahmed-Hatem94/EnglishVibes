using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnglishVibes.Data.Models
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        //public ApplicationUser() : base()
        //{
        //    UserRoles = new HashSet<IdentityUserRole<Guid>>();
        //}

        //[InverseProperty("User")]
        public ICollection<ApplicationUserRole>? UserRoles { get; set; }

        //public ICollection<Group>? Groups { get; set; }
    }
}
