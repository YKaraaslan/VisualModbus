using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    public class Class1
    {
        public interface IUser
        {
            string userName { get; set; }
            string password { get; set; }
            string nameSurname { get; set; }
            string userMail { get; set; }
            string phoneNumber { get; set; }
            string userDbID { get; set; }
        }
    }
}
