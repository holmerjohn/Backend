using Backend.Domain;
using Backend.Domain.Facts;
using Backend.Extensions;
using Backend.Repositories;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Backend.Program
{
    public class FactEngine : IFactEngine
    {
        private readonly IFactRepository _factRepository;
        private readonly ILogger<FactEngine> _logger;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public FactEngine(
            IFactRepository factRepository,
            ILogger<FactEngine> logger,
            JsonSerializerOptions jsonSerializerOptions)
        {
            _factRepository = factRepository;
            _logger = logger;
            _jsonSerializerOptions = jsonSerializerOptions;
        }

        public async Task LoadFactsAsync(Stream utf8json, CancellationToken cancellationToken = default)
        {
            var inputFacts = await JsonSerializer.DeserializeAsync<IEnumerable<Models.Fact>>(utf8json, 
                _jsonSerializerOptions, cancellationToken: cancellationToken);
            foreach (var inputFact in inputFacts)
            {
                var dbFact = await _factRepository.GetByNameAsync(inputFact.Name);
                dbFact.EntityType = inputFact.EntityType;
                foreach (var inputCondition in inputFact.Conditions)
                {
                    var dbCondition = new FactCondition()
                    {
                        Id = Guid.NewGuid().ToString(),
                        FactId = dbFact.Id,
                        Field = inputCondition.Field,
                        Comparator = inputCondition.Comparator,
                        Value = inputCondition.Value?.ToString()
                    };
                    if (inputCondition.Value?.GetType() == typeof(JsonElement))
                    {
                        var jsonElement = (JsonElement)inputCondition.Value;
                        dbCondition.PropertyType = jsonElement.GetPropertyType();
                    }
                    else
                    {
                        dbCondition.PropertyType = inputCondition.Value.GetPropertyType();
                    }
                    dbFact.Conditions.Add(dbCondition);
                }
            }
            await _factRepository.SaveChangesAsync(cancellationToken);
        }

        public async Task ProcessLoanFactsAsync(Loan loan, CancellationToken cancellationToken = default)
        {

        }

         
    //    SetValueFieldsFronJson(jsonElement);
                    
    }
}
