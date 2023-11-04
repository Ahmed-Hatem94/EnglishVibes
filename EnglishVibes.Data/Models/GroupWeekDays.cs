using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EnglishVibes.Data.Models
{
    public class GroupWeekDays
    {
        public int GroupId { get; set; }
        public DayOfWeek WeekDay { get; set; }
       
        public Group? Group { get; set; }
    }
}
