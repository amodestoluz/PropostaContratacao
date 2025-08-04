using application.dto;
using domain.entities;

namespace proposta.mappers
{
    public static class PropostaApplicationMapper
    {
        public static PropostaResponse ToResponse(this Proposta proposta)
        {
            if (proposta == null)
            {
                return null;
            }

            return new PropostaResponse(
                proposta.Id,
                proposta.Cliente,
                proposta.Valor,
                proposta.Status.ToString(),
                proposta.DataCriacao
            );
        }

        public static IEnumerable<PropostaResponse> ToResponse(this IEnumerable<Proposta> listProposta)
        {
            if (listProposta == null)
            {
                return Enumerable.Empty<PropostaResponse>();
            }

            return listProposta.Select(ToResponse);
        }
    }
}
