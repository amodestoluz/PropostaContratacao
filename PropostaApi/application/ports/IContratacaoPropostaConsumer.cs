using application.dto;

namespace application.ports
{
    public interface IContratacaoPropostaConsumer
    {
        Task Consume(AlterarStatusPropostaCommand command);

    }
}
