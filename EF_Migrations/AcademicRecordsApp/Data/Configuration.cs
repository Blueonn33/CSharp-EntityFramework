namespace AcademicRecordsApp.Data
{
    internal class Configuration
    {
        public static string ConnectionString
            => @"Server=PREDATOR\SQLEXPRESS;Database=AcademicRecordsDB;Trusted_Connection=True;Encrypt=False;";
    }
}
