using CodeFirst.Data;
using Microsoft.EntityFrameworkCore;

var context = new BlogDbContext();

context.Database.EnsureCreated();
context.Database.Migrate();