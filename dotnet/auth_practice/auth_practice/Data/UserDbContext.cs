using Microsoft.EntityFrameworkCore;
using auth_practice.Entities;
using auth_practice.Models;

namespace auth_practice.Data;

public class UserDbContext(DbContextOptions<UserDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
}

public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
}
