using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;

namespace AuthorizationDB
{
    public class AuthorizationContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("Data Source=authorization.db");
    }

    public class User
    {
        [Key]
        public long Id { get; set; }
        public string Login { get; set; }
        public string PasswordHash { get; set; }
        public AccessLevel Level { get; set; }

        public enum AccessLevel { User = 0, Admin = 1 };

        public override bool Equals(object obj) => obj is User user && this.Id == user.Id && this.Login == user.Login && this.PasswordHash == user.PasswordHash && this.Level == user.Level;
        public override int GetHashCode() => HashCode.Combine(this.Id, this.Login, this.PasswordHash, this.Level);
    }
}
