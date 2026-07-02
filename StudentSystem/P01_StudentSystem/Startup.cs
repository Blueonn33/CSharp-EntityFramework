using P01_StudentSystem.Data;

public class Program
{
    static void Main(string[] args)
    {
        try
        {
            using StudentSystemContext dbContext = new();

            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);

            if (ex.InnerException != null)
            {
                Console.WriteLine(ex.InnerException.Message);
            }
        }
    }
}
