using application.dto;
using application.ports;
using application.usecases.interfaces;

namespace application.adapters
{
    public class ContratacaoPropostaService : IContratacaoPropostaService
    {
        private readonly IContratarPropostaUseCase contratarPropostaUseCase;

        public ContratacaoPropostaService(IContratarPropostaUseCase contratarPropostaUseCase)
        {
            this.contratarPropostaUseCase = contratarPropostaUseCase;
        }

        public  async Task Contratar(ContratarPropostaCommand command)
        {
            await this.contratarPropostaUseCase.ExecuteAsync(command);
        }
    }
}
