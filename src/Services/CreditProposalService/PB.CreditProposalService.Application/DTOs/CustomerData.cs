namespace PB.CreditProposalService.Application.DTOs
{
    public class CustomerData
    {
        public Guid CustomerId { get; set; }
        public decimal MonthlyIncome { get; set; }
        public DateTime BirthDate { get; set; }
        public int InitialScore { get; set; }

        public CustomerData(Guid customerId, decimal monthlyIncome, DateTime birthDate, int initialScore)
        {
            CustomerId = customerId;
            MonthlyIncome = monthlyIncome;
            BirthDate = birthDate;
            InitialScore = initialScore;
        }

        // Construtor vazio para deserialização
        public CustomerData() { }
    }
}