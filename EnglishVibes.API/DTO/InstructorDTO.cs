using EnglishVibes.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnglishVibes.Service.DTO
{
    public class InstructorDTO
    {
        [Range(16, 80)]
        public int Age { get; set; }

        [MinLength(4)]
        public string UserName { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }
        public int NoOfGroups { get; set; } = 0;
    }
}
