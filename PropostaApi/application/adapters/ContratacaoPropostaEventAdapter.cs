using application.dto;
using application.ports;
using Microsoft.Extensions.Logging;

namespace application.adapters
{
    public class ContratacaoPropostaEventAdapter : IContratacaoPropostaConsumer
    {
        private readonly IPropostaService _propostaService;
        private readonly ILogger<ContratacaoPropostaEventAdapter> _logger;

        public ContratacaoPropostaEventAdapter(
            IPropostaService propostaService,
            ILogger<ContratacaoPropostaEventAdapter> logger)
        {
            _propostaService = propostaService;
            _logger = logger;
        }

        public async Task Consume(AlterarStatusPropostaCommand command)
        {
            _logger.LogInformation($"Adaptador de evento SQS recebido para proposta ID: {command.id}");
            await _propostaService.AlterarStatusPropostaAsync(command);
        }
    }
}
