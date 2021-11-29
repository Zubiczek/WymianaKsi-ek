using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WymianaKsiazek.Functions
{
    public interface ILoggedInUser
    {
        bool IsUserLoggedIn();
        string GetUserId();
    }
}
