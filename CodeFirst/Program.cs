using CodeFirst.Data;

var context = new BlogDbContext();

context.Database.EnsureCreated();
