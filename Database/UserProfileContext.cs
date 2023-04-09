using Microsoft.EntityFrameworkCore;
using Models;

namespace Database
{
    public class UserProfileContext : DbContext
    {
        public UserProfileContext(DbContextOptions <UserProfileContext> options) : base(options){}
        public DbSet<UserModel> Users { get; set; }
    }
}