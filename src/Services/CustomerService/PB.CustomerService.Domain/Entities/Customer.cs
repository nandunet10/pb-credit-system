using PB.CustomerService.Domain.Enums;
using PB.CustomerService.Domain.Exceptions;

namespace PB.CustomerService.Domain.Entities
{
    public class Customer
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string CPF { get; private set; }
        public string Email { get; private set; }
        public DateTime DateOfBirth { get; private set; }
        public decimal Income { get; private set; }
        public string PhoneNumber { get; private set; }
        public Address Address { get; private set; }
        public CustomerStatus Status { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }

        // EF Constructor
        private Customer() { }

        public Customer(
            string name,
            string cpf,
            string email,
            DateTime dateOfBirth,
            decimal income,
            string phoneNumber,
            Address address)
        {
            Id = Guid.NewGuid();
            Name = name ?? throw new ArgumentNullException(nameof(name));
            CPF = cpf ?? throw new ArgumentNullException(nameof(cpf));
            Email = email ?? throw new ArgumentNullException(nameof(email));
            DateOfBirth = dateOfBirth;
            Income = income;
            PhoneNumber = phoneNumber;
            Address = address ?? throw new ArgumentNullException(nameof(address));
            Status = CustomerStatus.Active;
            CreatedAt = DateTime.UtcNow;

            Validate();
        }

        public void Update(string name, string email, string phoneNumber, decimal income)
        {
            Name = name ?? Name;
            Email = email ?? Email;
            PhoneNumber = phoneNumber ?? PhoneNumber;
            Income = income > 0 ? income : Income;
            UpdatedAt = DateTime.UtcNow;

            Validate();
        }

        public void Deactivate()
        {
            Status = CustomerStatus.Inactive;
            UpdatedAt = DateTime.UtcNow;
        }

        public int GetAge()
        {
            var today = DateTime.Today;
            var age = today.Year - DateOfBirth.Year;
            if (DateOfBirth.Date > today.AddYears(-age)) age--;
            return age;
        }

        private void Validate()
        {
            if (string.IsNullOrWhiteSpace(Name))
                throw new DomainException("Nome é obrigatório");

            if (Name.Length < 3 || Name.Length > 200)
                throw new DomainException("Nome deve ter entre 3 e 200 caracteres");

            if (!IsValidCPF(CPF))
                throw new DomainException("CPF inválido");

            if (!IsValidEmail(Email))
                throw new DomainException("Email inválido");

            if (DateOfBirth > DateTime.Today.AddYears(-18))
                throw new DomainException("Cliente deve ter no mínimo 18 anos");

            if (Income < 0)
                throw new DomainException("Renda não pode ser negativa");
        }

        private static bool IsValidCPF(string cpf)
        {
            if (string.IsNullOrWhiteSpace(cpf))
                return false;

            cpf = cpf.Replace(".", "").Replace("-", "").Trim();

            if (cpf.Length != 11)
                return false;

            if (cpf.Distinct().Count() == 1)
                return false;

            return true;
        }

        private static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
