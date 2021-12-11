using Microsoft.EntityFrameworkCore;
using server.Entities;

namespace server.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions options) : base(options) {}

    public DbSet<Job> Jobs => Set<Job>();
    public DbSet<Log> Logs => Set<Log>();
}