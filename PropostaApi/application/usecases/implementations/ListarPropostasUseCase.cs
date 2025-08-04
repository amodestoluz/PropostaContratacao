using application.dto;
using application.ports;
using application.usecases.interfaces;
using proposta.mappers;

namespace application.usecases.implementations
{
    public class ListarPropostasUseCase : IListarPropostasUseCase
    {
        private readonly IPropostaRepository _propostaRepository;

        public ListarPropostasUseCase(IPropostaRepository propostaRepository)
        {
            _propostaRepository = propostaRepository;
        }

        public async Task<IEnumerable<PropostaResponse>> ExecuteAsync()
        {
            var propostas = await _propostaRepository.GetAllAsync();
            return propostas.Select(p => p.ToResponse());
        }
    }
}
