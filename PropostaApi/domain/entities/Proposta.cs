using domain.@enum;

namespace domain.entities
{
    public class Proposta
    {
        public Guid Id { get; private set; }
        public string Cliente { get; private set; }
        public decimal Valor { get; private set; }
        public StatusPropostaEnum Status { get; private set; }
        public DateTime DataCriacao { get; private set; }
        public DateTime? DataContratacao { get; private set; }


        public Proposta(string cliente, decimal valor)
        {
            if (string.IsNullOrWhiteSpace(cliente))
                throw new ArgumentException("Cliente não pode ser vazio.", nameof(cliente));
            if (valor <= 0)
                throw new ArgumentException("Valor da proposta deve ser maior que zero.", nameof(valor));

            Id = Guid.NewGuid();
            Cliente = cliente;
            Valor = valor;
            Status = StatusPropostaEnum.EmAnalise;
            DataCriacao = DateTime.UtcNow;
            DataContratacao = null;
        }

        private Proposta(Guid id, 
            string cliente, 
            decimal valor, 
            StatusPropostaEnum status,
            DateTime dataCriacao,
            DateTime? dataContratacao)
        {
            Id = id;
            Cliente = cliente;
            Valor = valor;
            Status = status;
            DataCriacao = dataCriacao;
            DataContratacao = dataContratacao;
        }

        public static Proposta Reconstitute(Guid id, 
            string cliente,
            decimal valor, 
            StatusPropostaEnum status,
            DateTime dataCriacao,
            DateTime? dataContratacao)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("ID da proposta não pode ser vazio na reconstituição.", nameof(id));
            if (string.IsNullOrWhiteSpace(cliente))
                throw new ArgumentException("Cliente não pode ser vazio na reconstituição.", nameof(cliente));
            if (valor <= 0)
                throw new ArgumentException("Valor da proposta deve ser maior que zero na reconstituição.", nameof(valor));

            return new Proposta(id, cliente, valor, status, dataCriacao, dataContratacao);
        }

 
        public void AlterarStatus(StatusPropostaEnum novoStatus)
        {
            // Regra 1: Só pode Contratar se estiver Aprovada
            if (novoStatus == StatusPropostaEnum.Contratada && Status != StatusPropostaEnum.Aprovada)
            {
                throw new InvalidOperationException($"Proposta não pode ser contratada. Status atual: {Status}. Apenas propostas Aprovadas podem ser contratadas.");
            }

            // Regra 2: Não pode recusar (Rejeitada) se já estiver Contratada
            if (novoStatus == StatusPropostaEnum.Rejeitada && Status == StatusPropostaEnum.Contratada)
            {
                throw new InvalidOperationException($"Proposta já contratada não pode ser recusada.");
            }

            // Se for contratada, defina a DataContratacao.
            // Se mudar para outro status e já tiver DataContratacao, limpe-a.
            if (novoStatus == StatusPropostaEnum.Contratada)
            {
                Status = novoStatus;
                if (DataContratacao == null) // Garante que só define uma vez
                {
                    DataContratacao = DateTime.UtcNow;
                }
            }
            else
            {
                Status = novoStatus;
                if (DataContratacao != null) // Limpa DataContratacao se o status não for Contratada
                {
                    DataContratacao = null;
                }
            }
        }
    }
}
