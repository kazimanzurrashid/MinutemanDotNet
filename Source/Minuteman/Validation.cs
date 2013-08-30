namespace Minuteman
{
    using System;
    using System.Linq;

    internal static class Validation
    {
        public static void ValidateString(
            string value,
            string message,
            string paramName)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(message, paramName);
            }
        }

        public static void ValidateDb(int db)
        {
            if (db < 0)
            {
                throw new ArgumentOutOfRangeException(
                    "db",
                    ErrorMessages.Validation_ValidateDb_Negative);
            }
        }

        public static void ValidateEventName(string eventName)
        {
            ValidateString(
                eventName,
                ErrorMessages.Validation_ValidateEventName_Required,
                "eventName");
        }

        public static void ValidateUsers(params long[] users)
        {
            if (!users.Any())
            {
                throw new ArgumentException(
                    ErrorMessages.Validation_ValidateUsers_Required, "users");
            }

            if (users.Any(user => user < 0))
            {
                throw new ArgumentOutOfRangeException(
                    "users",
                    ErrorMessages.Validation_ValidateUsers_Negative);
            }
        }
    }
}