using application.dto;
using application.ports;
using application.usecases.interfaces;
using Microsoft.Extensions.Logging;

namespace application.usecases.implementations
{
    public class AlterarStatusPropostaUseCase : IAlterarStatusPropostaUseCase
    {
        private readonly IPropostaRepository _propostaRepository;
        private readonly ILogger<AlterarStatusPropostaUseCase> _logger;

        public AlterarStatusPropostaUseCase(IPropostaRepository propostaRepository, ILogger<AlterarStatusPropostaUseCase> logger)
        {
            _propostaRepository = propostaRepository;
            _logger = logger;
        }

        public async Task ExecuteAsync(AlterarStatusPropostaCommand command)
        {
            _logger.LogInformation($"chamando alteração de proposta para o status: {command.status}");
            var proposta = await _propostaRepository.GetByIdAsync(command.id);
            _logger.LogInformation($"status atual da proposta: {proposta.Status}");

            proposta.AlterarStatus(command.status);
            await _propostaRepository.UpdateAsync(proposta);
        }
    }
}
