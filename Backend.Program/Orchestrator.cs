using Backend.Domain;

namespace Backend.Program
{
    public class Orchestrator
    {
        private readonly IActionEngine _actionEngine;
        private readonly IFactEngine _factEngine;
        private readonly ILoanActionProcessor _loanActionProcessor;
        private readonly BackendConfiguration _backendConfiguration;

        public Orchestrator(
            IActionEngine actionEngine,
            IFactEngine factEngine,
            ILoanActionProcessor loanActionProcessor,
            BackendConfiguration backendConfiguration)
        {
            _actionEngine = actionEngine;
            _factEngine = factEngine;
            _loanActionProcessor = loanActionProcessor;
            _backendConfiguration = backendConfiguration;
        }

        public async Task Run()
        {
            var factsPath = Path.Combine(_backendConfiguration.CurrentDirectory, "facts.json");
            var actionsPath = Path.Combine(_backendConfiguration.CurrentDirectory, "actions.json");
            using (var fileStream = File.OpenRead(actionsPath))
            {
                await _actionEngine.LoadActionsAsync(fileStream);
            }
            using (var fileStream = File.OpenRead(factsPath))
            {
                await _factEngine.LoadFactsAsync(fileStream);
            }

            await _loanActionProcessor.ProcessEntityActionsAsync(_actionEngine.Actions);
        }
    }
}
