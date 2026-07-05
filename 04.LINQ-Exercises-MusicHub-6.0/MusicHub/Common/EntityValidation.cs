namespace MusicHub.Common
{
    public static class EntityValidation
    {
        // Song
        public const int SongNameMaxLength = 20;
        public const string SongPriceColumnType = "DECIMAL(9, 4)";

        // Album
        public const int AlbumNameMaxLength = 40;

        // Performer
        public const int PerformerFirstNameMaxLength = 20;
        public const int PerformerLastNameMaxLength = 20;
        public const string PerformerNetWorthColumnType = "DECIMAL(12, 3)";
    }
}