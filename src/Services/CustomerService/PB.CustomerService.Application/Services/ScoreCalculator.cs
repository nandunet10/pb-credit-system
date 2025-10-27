namespace PB.CustomerService.Application.Services
{
    public class ScoreCalculator : IScoreCalculator
    {
        public int Calculate(CustomerData customerData)
        {
            int score = 0;

            // Pontuação baseada na renda
            score += CalculateIncomeScore(customerData.Income);

            // Pontuação baseada na idade
            score += CalculateAgeScore(customerData.DateOfBirth);

            // Limitar score entre 0 e 1000
            return Math.Clamp(score, 0, 1000);
        }

        private int CalculateIncomeScore(decimal income)
        {
            return income switch
            {
                >= 10000 => 400,  // Renda alta
                >= 5000 => 250,   // Renda média-alta
                >= 2000 => 150,   // Renda média
                >= 1000 => 80,    // Renda baixa
                _ => 50           // Renda muito baixa
            };
        }

        private int CalculateAgeScore(DateTime dateOfBirth)
        {
            var age = DateTime.Today.Year - dateOfBirth.Year;
            if (dateOfBirth.Date > DateTime.Today.AddYears(-age))
                age--;

            return age switch
            {
                >= 35 => 350,  // Idade madura
                >= 30 => 300,  // Adulto estabelecido
                >= 25 => 200,  // Adulto jovem
                >= 21 => 150,  // Jovem adulto
                _ => 100       // Recém maior de idade
            };
        }
    }
}
