using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WymianaKsiazek.Functions
{
    public class LoggedInUser : ILoggedInUser
    {
        private HttpContext _httpContext;
        public LoggedInUser(IHttpContextAccessor httpContextAccessor)
        {
            _httpContext = httpContextAccessor.HttpContext;
        }
        public bool IsUserLoggedIn()
        {
            ISessionFeature sessionFeature = _httpContext.Features.Get<ISessionFeature>();
            if (sessionFeature != null)
            {
                string token = sessionFeature.Session.GetString("Token"); ;
                if (token != null)
                {
                    return true;
                }
            }
            return false;
        }
        public string GetUserId()
        {
            ISessionFeature sessionFeature = _httpContext.Features.Get<ISessionFeature>();
            if (sessionFeature != null)
            {
                string userid = sessionFeature.Session.GetString("UserId");
                return userid;
            }
            return null;
        }

    }
}
