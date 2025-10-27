namespace PB.CustomerService.Domain.Entities
{
    public class Address
    {
        public string Street { get; private set; }
        public string Number { get; private set; }
        public string Complement { get; private set; }
        public string Neighborhood { get; private set; }
        public string City { get; private set; }
        public string State { get; private set; }
        public string ZipCode { get; private set; }

        private Address() { }

        public Address(
            string street,
            string number,
            string complement,
            string neighborhood,
            string city,
            string state,
            string zipCode)
        {
            Street = street ?? throw new ArgumentNullException(nameof(street));
            Number = number ?? throw new ArgumentNullException(nameof(number));
            Complement = complement;
            Neighborhood = neighborhood ?? throw new ArgumentNullException(nameof(neighborhood));
            City = city ?? throw new ArgumentNullException(nameof(city));
            State = state ?? throw new ArgumentNullException(nameof(state));
            ZipCode = zipCode ?? throw new ArgumentNullException(nameof(zipCode));
        }
    }
}
