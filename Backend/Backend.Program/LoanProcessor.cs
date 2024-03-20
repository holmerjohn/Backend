using Backend.Models;
using Microsoft.Extensions.Logging;

namespace Backend.Program
{
    internal class LoanProcessor : ILoanProcessor
    {
        private readonly IEntityRepository<Loan> _loanRepository;
        private readonly ILogger<LoanProcessor> _logger;
        private readonly BackendConfiguration _configuration;

        public LoanProcessor(
            IEntityRepository<Loan> loanRepository,
            ILogger<LoanProcessor> logger, 
            BackendConfiguration configuration)
        { 
            _loanRepository = loanRepository;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task ProcessLoanFilesAsync()
        {
            _logger.LogInformation("Simulating processing");

            var loan = await _loanRepository.GeyByIdAsync(Guid.Empty);
        }
    }
}
