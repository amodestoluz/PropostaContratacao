using domain.entities;
using infrastructure.data.models;

namespace infrastructure.data.mappers
{
    public static class PropostaDataMapper
    {
        public static PropostaModel ToPropostaModel(this Proposta domainEntity)
        {
            if (domainEntity == null)
            {
                return null;
            }

            return new PropostaModel(
                domainEntity.Id,
                domainEntity.Cliente,
                domainEntity.Valor,
                domainEntity.Status,
                domainEntity.DataCriacao,
                domainEntity.DataContratacao
            );
        }

        public static Proposta ToDomainEntity(this PropostaModel dataModel)
        {
            if (dataModel == null)
            {
                return null;
            }

            return Proposta.Reconstitute(
                         dataModel.Id,
                         dataModel.Cliente,
                         dataModel.Valor,
                         dataModel.Status,
                         dataModel.DataCriacao,
                         dataModel.DataContratacao
                     );

        }

        public static IEnumerable<Proposta> ToDomainEntity(this IEnumerable<PropostaModel> dataModels)
        {
            if (dataModels == null)
            {
                return Enumerable.Empty<Proposta>();
            }
            return dataModels.Select(ToDomainEntity);
        }
    }
}
