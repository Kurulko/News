using Microsoft.EntityFrameworkCore;
using News.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace News.Db;

public class NewsContext : DbContext
{
    public DbSet<Article> Articles { get; set; } = null!;
    public DbSet<Author> Authors { get; set; } = null!;

    public NewsContext()
        => Database.EnsureCreated();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string connectionStr = "Server=(localdb)\\mssqllocaldb; Database=News_v11; Trusted_Connection=True;";
        optionsBuilder.UseSqlServer(connectionStr);
    }
}
