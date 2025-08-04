using domain.@enum;


namespace application.dto
{
    public record AlterarStatusPropostaCommand(Guid id, StatusPropostaEnum status);
}
