using System;
using System.Security.Claims;

namespace MovieFinder.Utils
{
    public class Session
    {
        private ClaimsPrincipal _userSession;
        public int UserId;
     
        public Session(ClaimsPrincipal userSession)
        {
            _userSession = userSession;

            UserId = GetClaim("UserId"); 
        }

        private int GetClaim(string claimName)
        {
            return Convert.ToInt32(_userSession.FindFirst(claim => claim.Type == claimName).Value); 
        }
    }
}
