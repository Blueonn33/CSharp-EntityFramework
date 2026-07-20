namespace MoviesApp.Common
{
    public static class EntityValidation
    {
        // Title
        public const int MovieTitleMaxLength = 100;

        // Genre
        public const int MovieGenreMinLength = 3;
        public const int MovieGenreMaxLength = 50;

        // ReleaseDate
        public const string MovieReleaseDateRegexPattern = @"^\d{4}\-\d{2}\-\d{2}$";

        // Director
        public const int MovieDirectorMinLength = 5;
        public const int MovieDirectorMaxLength = 100;

        // Duration
        public const int MovieDurationMinValue = 1;
        public const int MovieDurationMaxValue = 500;

        // Description
        public const int MovieDescriptionMinLength = 10;
        public const int MovieDescriptionMaxLength = 2000;

        // ImageUrl
        public const int MovieImageUrlMaxLength = 2048;
    }
}
