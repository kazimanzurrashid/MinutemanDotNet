namespace Minuteman
{
    public class UserActivitySettings
    {
        private const int DefaultDb = 0;
        private const UserActivityDrilldownType DefaultDrilDown =
            UserActivityDrilldownType.Minute;

        private const string DefaultKeyPrefix = "minuteman";
        private const string DefaultKeySeparator = ":";
 
        public UserActivitySettings() : 
            this(
            DefaultDb,
            DefaultDrilDown,
            DefaultKeyPrefix,
            DefaultKeySeparator)
        {
        }

        public UserActivitySettings(
            int db,
            UserActivityDrilldownType drilldown)
            : this(db, drilldown, DefaultKeyPrefix, DefaultKeySeparator)
        {
        }

        public UserActivitySettings(int db)
            : this(
            db,
            DefaultDrilDown,
            DefaultKeyPrefix,
            DefaultKeySeparator)
        {
        }

        public UserActivitySettings(UserActivityDrilldownType drilldown)
            : this(
            DefaultDb,
            drilldown,
            DefaultKeyPrefix,
            DefaultKeySeparator)
        {
        }

        public UserActivitySettings(
            int db,
            UserActivityDrilldownType drilldown,
            string keyPrefix,
            string keySeparator)
        {
            Validation.ValidateDb(db);

            Db = db;
            Drilldown = drilldown;
            KeyPrefix = keyPrefix ?? string.Empty;
            KeySeparator = keySeparator ?? string.Empty;
        }

        public int Db { get; private set; }

        public UserActivityDrilldownType Drilldown { get; private set; }

        public string KeyPrefix { get; private set; }

        public string KeySeparator { get; private set; }
    }
}