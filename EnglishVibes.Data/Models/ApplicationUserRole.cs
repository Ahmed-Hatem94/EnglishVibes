using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnglishVibes.Data.Models
{
    public class ApplicationUserRole : IdentityUserRole<Guid>
    {
        public ApplicationUser? ApplicationUser { get; set; }
    }
}
