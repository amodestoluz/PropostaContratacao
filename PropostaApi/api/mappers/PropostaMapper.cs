using application.dto;
using System.Globalization;

namespace proposta.mappers
{
    public static class PropostaMapper
    {
        public static PropostaApiResponse ToApiResponse(this PropostaResponse applicationDto)
        {
            if (applicationDto == null)
            {
                return null;
            }

            return new PropostaApiResponse(
                applicationDto.id,
                applicationDto.cliente,
                applicationDto.valor.ToString("C", new CultureInfo("pt-BR")),
                applicationDto.status,
                applicationDto.dataCriacao.ToString("dd/MM/yyyy")
            );
        }

        public static IEnumerable<PropostaApiResponse> ToApiResponse(this IEnumerable<PropostaResponse> applicationDtos)
        {
            if (applicationDtos == null)
            {
                return Enumerable.Empty<PropostaApiResponse>();
            }

            return applicationDtos.Select(ToApiResponse);
        }
    }
}
