using Microsoft.AspNetCore.Authentication.Cookies;
using System.Collections.Generic;
using System.Security.Claims;

namespace SRC.Backend.Models.System
{
    public class UserClaims
    {
        public class ClaimsKey
        {
            public const string USERID = "UID";
            public const string ACCOUNT = "Account";
            public const string USER_NAME = "UserName";
            public const string ROLE_CODE = "RoleCode";
            public const string Unit = "Unit";
            //public const string EMail = ClaimTypes.Email;
        }

        public ClaimsIdentity Create(SRCLoginMeta user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsKey.USERID, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimsKey.ACCOUNT, user.Account),
                new Claim(ClaimsKey.USER_NAME, user.UserName),
                new Claim(ClaimsKey.Unit, (user.Unit.HasValue ? user.Unit.Value.ToString() : "")),
                new Claim(ClaimsKey.ROLE_CODE, string.Join(",",user.RoleCode)),
                //new Claim(ClaimTypes.Email, user.Email),
                
                //new Claim(ClaimTypes.Role, "Administrator"),
            };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            return claimsIdentity;
        }
    }
}
