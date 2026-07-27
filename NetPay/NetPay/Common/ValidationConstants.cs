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

        // Expense
        public const int ExpenseNameMinLength = 5;
        public const int ExpenseNameMaxLength = 50;
        public const string ExpenseAmountRangeMinValue = "0.01";
        public const string ExpenseAmountRangeMaxValue = "100000";

        // Service
        public const int ServiceNameMinLength = 5;
        public const int ServiceNameMaxLength = 30;

        // Supplier
        public const int SupplierNameMinLength = 3;
        public const int SupplierNameMaxLength = 60;
    }
}
