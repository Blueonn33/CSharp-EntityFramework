namespace NetPay.Common
{
    public static class ValidationConstants
    {
        // Household
        public const int HouseholdContactPersonMinLength = 5;
        public const int HouseholdContactPersonMaxLength = 50;
        public const int HouseholdEmailMinLength = 6;
        public const int HouseholdEmailMaxLength = 80;
        public const int HouseholdPhoneNumberLength = 15;
        public const string HouseholdPhoneNumberRegexPattern = @"^\+\d{3}/\d{3}-\d{6}$";
    }
}
