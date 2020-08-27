using System.Linq;
using Authorization.Other;

namespace Authorization
{
    public class Authorizer
    {
        public AuthorizationDB.User Authorize(Credentials credentials)
        {
            using (var db = new AuthorizationDB.AuthorizationContext())
            {
                AuthorizedUser = db.Users
                    .AsEnumerable()
                    .FirstOrDefault(u => credentials.Equals(u));
            }
            return AuthorizedUser;
        }

        public AuthorizationDB.User AuthorizedUser { get; private set; } = null;
    }
}
