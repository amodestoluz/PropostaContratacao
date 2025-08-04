using application.dto;

namespace application.ports
{
    public interface IPropostaService
    {
        Task<PropostaResponse> CriarPropostaAsync(CriarPropostaCommand command);
        Task<PropostaResponse> GetPropostaByIdAsync(Guid id);
        Task<IEnumerable<PropostaResponse>> ListarPropostasAsync();
        Task AlterarStatusPropostaAsync(AlterarStatusPropostaCommand command);
    }
}
