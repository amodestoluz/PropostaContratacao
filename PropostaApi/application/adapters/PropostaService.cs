using application.dto;
using application.ports;
using application.usecases.interfaces;
using domain.@enum;

namespace application.adapters
{
    public class PropostaService : IPropostaService
    {
        private readonly ICriarPropostaUseCase _criarPropostaUseCase;
        private readonly IGetPropostaByIdUseCase _getPropostaByIdUseCase;
        private readonly IListarPropostasUseCase _listarPropostasUseCase;
        private readonly IAlterarStatusPropostaUseCase _alterarStatusPropostaUseCase;

        public PropostaService(ICriarPropostaUseCase criarPropostaUseCase, IGetPropostaByIdUseCase getPropostaByIdUseCase, IListarPropostasUseCase listarPropostasUseCase, IAlterarStatusPropostaUseCase alterarStatusPropostaUsecASE)
        {
            _criarPropostaUseCase = criarPropostaUseCase;
            _getPropostaByIdUseCase = getPropostaByIdUseCase;
            _listarPropostasUseCase = listarPropostasUseCase;
            _alterarStatusPropostaUseCase = alterarStatusPropostaUsecASE;
        }



        public async Task AlterarStatusPropostaAsync(AlterarStatusPropostaCommand command)
        {
            await _alterarStatusPropostaUseCase.ExecuteAsync(command);
        }

        public async Task<PropostaResponse> CriarPropostaAsync(CriarPropostaCommand command)
        {
            return await _criarPropostaUseCase.ExecuteAsync(command);
        }

        public Task<PropostaResponse> GetPropostaByIdAsync(Guid id)
        {
            return _getPropostaByIdUseCase.ExecuteAsync(id);
        }

        public Task<IEnumerable<PropostaResponse>> ListarPropostasAsync()
        {
            return _listarPropostasUseCase.ExecuteAsync();
        }
    }
}
