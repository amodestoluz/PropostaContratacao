using application.dto;
using application.ports;
using application.usecases.interfaces;
using domain.entities;
using proposta.mappers;

namespace application.usecases.implementations
{
    public class CriarPropostaUseCase : ICriarPropostaUseCase
    {
        private readonly IPropostaRepository _propostaRepository;


        public CriarPropostaUseCase(IPropostaRepository propostaRepository)
        {
            _propostaRepository = propostaRepository;
        }

        public async Task<PropostaResponse> ExecuteAsync(CriarPropostaCommand command)
        {
            var proposta = new Proposta(command.cliente, command.valor);
            await _propostaRepository.AddAsync(proposta);
            return proposta.ToResponse();
        }
    }
}
