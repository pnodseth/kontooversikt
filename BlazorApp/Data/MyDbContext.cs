using BlazorApp.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp.Data;

public class MyDbContext : DbContext
{
    public MyDbContext(DbContextOptions<MyDbContext> options)
        : base(options)
    { }

    public DbSet<Chore> Chores { get; set; }
    
    
}