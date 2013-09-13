namespace Minuteman
{
    public class ActivitySettings
    {
        private const int DefaultDb = 0;
        private const ActivityTimeframe DefaultTimeframe =
            ActivityTimeframe.Hour;

        private const string DefaultKeyPrefix = "minuteman";
        private const string DefaultKeySeparator = ":";
 
        public ActivitySettings() : 
            this(
            DefaultDb,
            DefaultTimeframe,
            DefaultKeyPrefix,
            DefaultKeySeparator)
        {
        }

        public ActivitySettings(
            int db,
            ActivityTimeframe timeframe)
            : this(db, timeframe, DefaultKeyPrefix, DefaultKeySeparator)
        {
        }

        public ActivitySettings(int db)
            : this(
            db,
            DefaultTimeframe,
            DefaultKeyPrefix,
            DefaultKeySeparator)
        {
        }

        public ActivitySettings(ActivityTimeframe timeframe)
            : this(
            DefaultDb,
            timeframe,
            DefaultKeyPrefix,
            DefaultKeySeparator)
        {
        }

        public ActivitySettings(
            int db,
            ActivityTimeframe timeframe,
            string keyPrefix,
            string keySeparator)
        {
            Validation.ValidateDb(db);

            Db = db;
            Timeframe = timeframe;
            KeyPrefix = keyPrefix ?? string.Empty;
            KeySeparator = keySeparator ?? string.Empty;
        }

        public int Db { get; private set; }

        public ActivityTimeframe Timeframe { get; private set; }

        public string KeyPrefix { get; private set; }

        public string KeySeparator { get; private set; }
    }
}