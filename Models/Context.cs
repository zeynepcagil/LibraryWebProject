using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Library_Project.Models;

public class Context : DbContext
{
    public Context(DbContextOptions<Context> options) : base(options) { }

    public DbSet<Book> Books { get; set; }

    public DbSet<User> Users { get; set; }
    public DbSet<Loan> Loans { get; set; }
    public DbSet<Review> Reviews { get; set; }
}
