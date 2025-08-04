using application.dto;
using application.ports;
using application.usecases.interfaces;
using proposta.mappers;

namespace application.usecases.implementations
{
    public class GetPropostaByIdUseCase : IGetPropostaByIdUseCase
    {
        private readonly IPropostaRepository _propostaRepository;

        public GetPropostaByIdUseCase(IPropostaRepository propostaRepository)
        {
            _propostaRepository = propostaRepository;
        }

        public async Task<PropostaResponse> ExecuteAsync(Guid id)
        {
            var proposta = await _propostaRepository.GetByIdAsync(id);
            return proposta.ToResponse();
        }
    }
}
