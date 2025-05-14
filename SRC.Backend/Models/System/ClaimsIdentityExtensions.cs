using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System;
using System.Linq;

namespace SRC.Backend.Models.System
{
    public static class ClaimsIdentityExtensions
    {
        public static Claim GetClaim(this ClaimsIdentity identity, string key)
        {
            return identity.Claims.Where(m => m.Type == key).FirstOrDefault();
        }

        public static SRCLoginMeta LoginMeta(this IIdentity identity)
        {
            //string editor = ((ClaimsIdentity)User.Identity).Claims.Where(m => m.Type == "Account").First().Value;


            SRCLoginMeta meta = new SRCLoginMeta();

            if (!((ClaimsIdentity)identity).IsAuthenticated)
            {
                throw new Exception("no login");
            }

            meta.UserId = new Guid(((ClaimsIdentity)identity).GetClaim(UserClaims.ClaimsKey.USERID).Value);
            meta.Account = ((ClaimsIdentity)identity).GetClaim(UserClaims.ClaimsKey.ACCOUNT).Value;
            meta.UserName = ((ClaimsIdentity)identity).GetClaim(UserClaims.ClaimsKey.USER_NAME).Value;
            meta.Unit = ((ClaimsIdentity)identity).GetClaim(UserClaims.ClaimsKey.Unit).Value;
            string roleCode = ((ClaimsIdentity)identity).GetClaim(UserClaims.ClaimsKey.ROLE_CODE).Value;

            if (!string.IsNullOrWhiteSpace(roleCode))
            {
                meta.RoleCode = roleCode.Split(",", StringSplitOptions.RemoveEmptyEntries).ToList();
            }
            else
            {
                meta.RoleCode = new List<string>();
            }

            return meta;

        }
    }
}
