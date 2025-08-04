using application.dto;


namespace application.usecases.interfaces
{
    public interface IContratarPropostaUseCase
    {
        Task ExecuteAsync(ContratarPropostaCommand command);
    }
}
