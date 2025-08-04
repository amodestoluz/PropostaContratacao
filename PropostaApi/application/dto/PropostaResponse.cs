
namespace application.dto
{
    public record PropostaResponse(Guid id, string cliente, decimal valor, string status, DateTime dataCriacao);
}
