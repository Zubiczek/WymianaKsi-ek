using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WymianaKsiazek.Models.MapperModels
{
    public class CreateUserMP
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string AddressName { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
    }
}
