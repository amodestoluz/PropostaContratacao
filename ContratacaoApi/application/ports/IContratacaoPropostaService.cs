using application.dto;

namespace application.ports
{
    public interface IContratacaoPropostaService
    {
        Task Contratar(ContratarPropostaCommand command);

    }
}
