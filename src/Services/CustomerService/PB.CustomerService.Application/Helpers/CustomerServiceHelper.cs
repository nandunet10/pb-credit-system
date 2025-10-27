namespace PB.CustomerService.Application.Helpers
{
    public static class CustomerServiceHelper
    {
        public static bool BeValidCPF(string cpf)
        {
            if (string.IsNullOrWhiteSpace(cpf))
                return false;

            cpf = cpf.Replace(".", "").Replace("-", "").Trim();

            if (cpf.Length != 11)
                return false;

            return cpf.Distinct().Count() > 1;
        }

        public static bool BeAtLeast18YearsOld(DateTime dateOfBirth)
        {
            return dateOfBirth <= DateTime.Today.AddYears(-18);
        }
    }
}
