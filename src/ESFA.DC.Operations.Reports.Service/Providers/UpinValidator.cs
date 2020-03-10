namespace ESFA.DC.Operations.Reports.Service.Providers
{
    using System.Text.RegularExpressions;

    public static class UpinValidator
    {
        private static Regex regex = new Regex(@"^\d{6}$");

        public static bool IsValid(string upin)
        {
            if (string.IsNullOrWhiteSpace(upin))
            {
                return false;
            }

            return regex.IsMatch(upin);
        }
    }
}
