using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EnglishVibes.Data.Models
{
    public class Instructor : ApplicationUser
    {
        //public string Id { get; set; }
        [JsonIgnore]
        public ICollection<Group>? Groups { get; set; }
    }
}
