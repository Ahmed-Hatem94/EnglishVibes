using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnglishVibes.Data.Models
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        [Range(16, 80)]
        public int Age { get; set; }
    }
}
