using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnglishVibes.Service.DTO
{
    public class UserLoginDTO
    {
        //public UserType UserType { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
    //public enum UserType
    //{
    //    admin=1,
    //    instructur=2,
    //    student=3

    //}
}
