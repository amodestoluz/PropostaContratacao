using application.dto;
using application.ports;
using application.usecases.interfaces;
using domain.entities;
using Microsoft.Extensions.Logging;

namespace application.usecases.implementation
{
    public class ContratarPropostaUseCase : IContratarPropostaUseCase
    {
        private readonly IPropostaExternalService _propostaExternalService;
        private readonly ILogger<ContratarPropostaUseCase> _logger; 
        private readonly IContratarPropostaMessageProduce _contratacaoMessageProducer;

        public ContratarPropostaUseCase(IPropostaExternalService propostaExternalService, ILogger<ContratarPropostaUseCase> logger, IContratarPropostaMessageProduce contratacaoMessageProducer)
        {
            _propostaExternalService = propostaExternalService;
            _logger = logger;
            _contratacaoMessageProducer = contratacaoMessageProducer;
        }
        public async Task ExecuteAsync(ContratarPropostaCommand command)
        {
            try
            {

                var proposta = await _propostaExternalService.GetPropostaByIdAsync(command.IdProposta);
              
                if (proposta == null)
                {
                    _logger.LogWarning($"Proposta com ID '{command.IdProposta}' não encontrada na PropostaApi.");
                    throw new ArgumentException($"Proposta com ID '{command.IdProposta}' não encontrada na PropostaApi.");
                }
                _logger.LogInformation($"Proposta recuperada: ID={proposta.Id}, Cliente={proposta.Cliente}, Valor={proposta.Valor}, Status={proposta.Status}, DataCriacao={proposta.DataCriacao}");

                proposta.PodeSerContratada();
                _logger.LogInformation($"Proposta {proposta.Id} pode ser contratada (Status: {proposta.Status}).");
                await _contratacaoMessageProducer.ProduceMessageAsync(command);
                _logger.LogInformation($"Mensagem de proposta CONTRATADA enviada para SQS para ID: {proposta.Id}");


            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Erro de validação ao contratar proposta: {ex.Message}");
                throw new InvalidOperationException($"Não foi possível contratar a proposta: {ex.Message}", ex);
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Erro de operação inválida ao contratar proposta: {ex.Message}");
                throw;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Erro de comunicação com PropostaApi: {ex.Message}");
                throw new ApplicationException("Erro de comunicação com o serviço de Propostas. Tente novamente mais tarde.", ex);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro inesperado ao contratar proposta: {ex.Message}");
                throw new ApplicationException("Ocorreu um erro inesperado ao tentar contratar a proposta.", ex);
            }
        }
    }
}
