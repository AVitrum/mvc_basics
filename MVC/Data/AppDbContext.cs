using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using MVC.Models;

namespace MVC.Data;

public class AppDbContext : IdentityDbContext<AppUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    
    public DbSet<Race> Races { get; set; }
    public DbSet<Club> Clubs { get; set; }
    public DbSet<Address> Addresses { get; set; }
}