using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnglishVibes.Service.DTO
{
    public class ActiveStudentDTO
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string SelectedStudyPlan { get; set; } // Private Or Group
        public string CurrentLevel { get; set; }
        public decimal PayedAmount { get; set; }
        public bool ActiveStatus { get; set; }
        public int GroupId { get; set; }
    }
}
