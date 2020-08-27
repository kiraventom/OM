using AuthorizationDB;
using System;

namespace Authorization.Other
{
    public class Credentials
    {
        public Credentials(string login, string password)
        {
            Login = login;
            PasswordHash = Hasher.GetHash(password);
        }

        public string Login { get; }
        public string PasswordHash { get; }

        public override bool Equals(object obj)
        {
            var sc = StringComparer.OrdinalIgnoreCase;
            return obj switch
            {
                Credentials credentials => sc.Equals(this.Login, credentials.Login) && sc.Equals(this.PasswordHash, credentials.PasswordHash),
                User user => sc.Equals(this.Login, user.Login) && sc.Equals(this.PasswordHash, user.PasswordHash),
                _ => false
            };
        }

        public override int GetHashCode() => HashCode.Combine(this.Login, this.PasswordHash);
    }
}
