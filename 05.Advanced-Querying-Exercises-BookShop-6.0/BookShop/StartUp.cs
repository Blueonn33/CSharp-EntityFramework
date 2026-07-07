using BookShop.Models.Enums;
using System.Text;

namespace BookShop
{
    using Data;
    using Initializer;

    public class StartUp
    {
        public static void Main()
        {
            using var dbContext = new BookShopContext();
            DbInitializer.ResetDatabase(dbContext);

            string command = Console.ReadLine()!;

            string result = GetBooksByAgeRestriction(dbContext, command);

            Console.WriteLine(result);
        }

        public static string GetBooksByAgeRestriction(BookShopContext dbContext, string command)
        {
            StringBuilder sb = new();

            AgeRestriction? ageRestriction = null;

            if (command != null && Enum.GetValues<AgeRestriction>().Any(ev => ev.ToString().ToLowerInvariant() == command.ToLowerInvariant()))
            {
                ageRestriction = Enum.Parse<AgeRestriction>(command, true);
            }

            if (!ageRestriction.HasValue)
            {
                return sb.ToString().TrimEnd();
            }

            var ageRestrictedBooks = dbContext.Books
                .Where(b => b.AgeRestriction == ageRestriction)
                .ToArray();

            return string.Join(Environment.NewLine, ageRestrictedBooks.Select(b => b.Title));
        }
    }
}


