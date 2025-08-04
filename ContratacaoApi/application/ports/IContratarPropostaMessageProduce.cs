using application.dto;

namespace application.ports
{
    public interface IContratarPropostaMessageProduce
    {
        Task ProduceMessageAsync(ContratarPropostaCommand message);
    }
}
