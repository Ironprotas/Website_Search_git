
using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;

namespace SearchApp.Data;


public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<SearchItem> SearchItems { get; set; }
}